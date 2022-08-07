using System;
using Microsoft.Xna.Framework;
using Monocle;

namespace Celeste.Mod.StaminaMeter
{
    public class SmallStaminaMeterDisplay : BaseStaminaMeterDisplay
    {
        public override void Update()
        {
            base.Update();
            Visible = Visible && StaminaMeterModule.Settings.SmallMeterEnabled;
        }

        // TODO MAYBE SOMEDAY: make this less full of magic numbers
        public override void Render()
        {
            if (!StaminaMeterModule.Settings.SmallMeterEnabled)
            {
                return;
            }

            base.Render();

            Vector2 playerPosition = level.Camera.CameraToScreen(player.Position) * 6f;
            // support for mirror mode
            if (SaveData.Instance != null && SaveData.Instance.Assists.MirrorMode)
            {
                playerPosition.X = 1920f - playerPosition.X;
            }
            // support for upside down mode in extended variants
            if (Input.MoveY.Inverted)
            {
                playerPosition.Y = 1080f - playerPosition.Y + 96f;
            }

            switch (StaminaMeterModule.Settings.SmallMeterPosition)
            {
                case SmallMeterPositions.Above:
                    meterPosition = new Vector2(playerPosition.X - 48f, playerPosition.Y - 114f);
                    break;
                case SmallMeterPositions.Below:
                    meterPosition = new Vector2(playerPosition.X - 48f, playerPosition.Y + 6f);
                    break;
                default:  // just in case
                    goto case SmallMeterPositions.Above;
            }

            // outline
            Draw.Rect(meterPosition.X - 1f, meterPosition.Y - 1f, 98f, 14f, lineColor);
            // fill
            Draw.Rect(meterPosition.X, meterPosition.Y, 96f, 12f, fillColor);

            // show stamina
            if (drawStamina > 0f)
            {
                // no, just drawing two rectangles at 50% opacity does not work. i've tried.
                if (drawStamina > currentStamina)
                {
                    Draw.Rect(meterPosition.X, meterPosition.Y, 96f * (drawStamina / 110f), 12f, colorDark);
                    if (currentStamina > 0)
                    {
                        Draw.Rect(meterPosition.X, meterPosition.Y, 96f * (currentStamina / 110f), 12f, color);
                    }
                }
                else
                {
                    if (currentStamina > 0)
                    {
                        Draw.Rect(meterPosition.X, meterPosition.Y, 96f * (currentStamina / 110f), 12f, colorDark);
                    }
                    Draw.Rect(meterPosition.X, meterPosition.Y, 96f * (drawStamina / 110f), 12f, color);
                }
            }

            // low stamina marker
            Draw.Rect(meterPosition.X + (192f / 11f), meterPosition.Y, 1, 12f, lineColor);
        }
    }

    public enum SmallMeterPositions
    {
        Above,
        Below
    }
}
