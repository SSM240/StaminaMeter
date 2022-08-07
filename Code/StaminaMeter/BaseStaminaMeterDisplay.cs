using System;
using Microsoft.Xna.Framework;
using Monocle;

namespace Celeste.Mod.StaminaMeter
{
    public abstract class BaseStaminaMeterDisplay : Component
    {
        protected Player player;
        protected float drawStamina;
        protected float currentStamina;
        protected float displayTimer;
        protected Level level;

        protected Color color;
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
            currentStamina = player.Stamina;
            drawStamina = currentStamina;
        }

        public override void Update()
        {
            base.Update();
            currentStamina = player.Stamina;
            if (!StaminaMeterModule.Settings.ShowMoreThanMaxStamina)
            {
                currentStamina = Calc.Min(currentStamina, 110f);
            }
            drawStamina = Calc.Approach(drawStamina, currentStamina, 250f * Engine.DeltaTime);
            if (drawStamina != 110f && !player.Dead)
            {
                displayTimer = 0.75f;
            }
            else if (displayTimer > 0f)
            {
                displayTimer -= Engine.DeltaTime;
            }

            Visible = (displayTimer > 0f) && !(level.Paused && StaminaMeterModule.Settings.HideWhilePaused);
        }

        public override void Render()
        {
            fillColor = Calc.HexToColor(StaminaMeterModule.Settings.FillColor);
            lineColor = Calc.HexToColor(StaminaMeterModule.Settings.LineColor);
            if (currentStamina < 20f)
            {
                color = Calc.HexToColor(StaminaMeterModule.Settings.LowStaminaColor);
                colorDark = Color.Lerp(color, fillColor, 0.5f);
            }
            else
            {
                color = Calc.HexToColor(StaminaMeterModule.Settings.NormalStaminaColor);
                colorDark = Color.Lerp(color, fillColor, 0.5f);
            }
        }
    }
}
