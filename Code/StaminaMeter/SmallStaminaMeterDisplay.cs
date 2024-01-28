using System;
using Microsoft.Xna.Framework;
using Monocle;

namespace Celeste.Mod.StaminaMeter
{
    public class SmallStaminaMeterDisplay : BaseStaminaMeterDisplay
    {
        private static Vector2 screenSize = new Vector2(1920, 1080);
        private static Vector2 meterSize = new Vector2(96, 12);

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
                playerPosition.X = screenSize.X - playerPosition.X;
            }
            // support for upside down mode in extended variants
            if (Input.MoveY.Inverted)
            {
                playerPosition.Y = screenSize.Y - playerPosition.Y + 96f;
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
            Draw.Rect(meterPosition.X - 1f, meterPosition.Y - 1f, meterSize.X + 2, meterSize.Y + 2, lineColor);
            // fill
            Draw.Rect(meterPosition.X, meterPosition.Y, meterSize.X, meterSize.Y, fillColor);

            // show the meter
            if (lightRatio > 0f)
            {
                // no, just drawing two rectangles at 50% opacity does not work. i've tried.
                if (lightRatio > darkRatio)
                {
                    Draw.Rect(meterPosition.X, meterPosition.Y, meterSize.X * darkRatio, meterSize.Y, colorDark);
                    if (darkRatio > 0)
                    {
                        Draw.Rect(meterPosition.X, meterPosition.Y, meterSize.X * lightRatio, meterSize.Y, colorLight);
                    }
                }
                else
                {
                    if (darkRatio > 0)
                    {
                        Draw.Rect(meterPosition.X, meterPosition.Y, meterSize.X * darkRatio, meterSize.Y, colorDark);
                    }
                    Draw.Rect(meterPosition.X, meterPosition.Y, meterSize.X * lightRatio, meterSize.Y, colorLight);
                }
            }

            // low marker
            Draw.Rect(meterPosition.X + meterSize.X * lowMarkRatio, meterPosition.Y, 1, meterSize.Y, lineColor);
        }
    }

    public enum SmallMeterPositions
    {
        Above,
        Below
    }
}
