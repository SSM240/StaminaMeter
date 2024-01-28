using System;
using Microsoft.Xna.Framework;
using Monocle;
using MonoMod.Utils;

namespace Celeste.Mod.StaminaMeter
{
    public abstract class BaseStaminaMeterDisplay : Component
    {
        protected static float lowStamina = 20f;
        protected static float maxStamina = 110f;
        protected static float lowStarFlyTime = 0.5f;
        protected static float maxStarFlyTime = 2f;

        protected Level level;
        protected Player player;
        protected DynamicData playerData;

        protected float drawStamina;
        protected float currentStamina;
        protected float drawStarFlyTime;
        protected float currentStarFlyTime;

        protected enum DisplayTypes
        {
            Stamina,
            StarFlyTime
        }
        protected DisplayTypes displayType;
        protected float displayTimer;

        protected float lightRatio;
        protected float darkRatio;
        protected float lowMarkRatio;

        protected Color colorLight;
        protected Color colorDark;
        protected Color fillColor;
        protected Color lineColor;

        protected Vector2 meterPosition;

        public BaseStaminaMeterDisplay()
            : base(active: true, visible: false)
        {
        }

        public override void Added(Entity entity)
        {
            base.Added(entity);
            level = SceneAs<Level>();
            // hopefully getting player once is enough, it hasn't caused any issues yet
            player = level.Tracker.GetEntity<Player>();
            playerData = DynamicData.For(player);
            currentStamina = player.Stamina;
            currentStarFlyTime = playerData.Get<float>("starFlyTimer");
            drawStamina = currentStamina;
        }

        public override void Update()
        {
            base.Update();
            currentStamina = player.Stamina;
            currentStarFlyTime = playerData.Get<float>("starFlyTimer");
            if (!StaminaMeterModule.Settings.ShowMoreThanMaxStamina)
            {
                currentStamina = Calc.Min(currentStamina, maxStamina);
                currentStarFlyTime = Calc.Min(currentStarFlyTime, maxStarFlyTime);
            }
            drawStamina = Calc.Approach(drawStamina, currentStamina, 250f * Engine.DeltaTime);
            if (drawStamina != maxStamina && !player.Dead)
            {
                displayTimer = 0.75f;
            }
            else if (displayTimer > 0f)
            {
                displayTimer -= Engine.DeltaTime;
            }
            if (player.StateMachine == Player.StStarFly)
            {
                Visible = StaminaMeterModule.Settings.StarFlyTimerEnabled && !player.Dead;
            }
            else
            {
                Visible = StaminaMeterModule.Settings.StaminaMeterEnabled && displayTimer > 0f;
            }
            Visible = Visible && !(level.Paused && StaminaMeterModule.Settings.HideWhilePaused);
        }

        public override void Render()
        {
            fillColor = Calc.HexToColor(StaminaMeterModule.Settings.FillColor);
            lineColor = Calc.HexToColor(StaminaMeterModule.Settings.LineColor);
            if (player.StateMachine == Player.StStarFly)
            {
                displayType = DisplayTypes.StarFlyTime;
                lightRatio = currentStarFlyTime / maxStarFlyTime;
                darkRatio = lightRatio;
                lowMarkRatio = lowStarFlyTime / maxStarFlyTime;
                if (currentStarFlyTime < lowStarFlyTime)
                {
                    colorLight = Calc.HexToColor(StaminaMeterModule.Settings.LowStarFlyTimeColor);
                }
                else
                {
                    colorLight = Calc.HexToColor(StaminaMeterModule.Settings.NormalStarFlyTimeColor);
                }
            }
            else
            {
                displayType = DisplayTypes.Stamina;
                lightRatio = drawStamina / maxStamina;
                darkRatio = currentStamina / maxStamina;
                lowMarkRatio = lowStamina / maxStamina;
                if (currentStamina < lowStamina)
                {
                    colorLight = Calc.HexToColor(StaminaMeterModule.Settings.LowStaminaColor);
                }
                else
                {
                    colorLight = Calc.HexToColor(StaminaMeterModule.Settings.NormalStaminaColor);
                }
            }
            colorDark = Color.Lerp(colorLight, fillColor, 0.5f);
        }
    }
}
