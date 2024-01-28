using System;
using Microsoft.Xna.Framework;
using Monocle;

namespace Celeste.Mod.StaminaMeter
{
    public class LargeStaminaMeterDisplay : BaseStaminaMeterDisplay
    {
        private static Vector2 TopLeftPos = new Vector2(48, 48);
        private static Vector2 TopLeftPos2 = new Vector2(48, 146);
        private static Vector2 TopCenterPos = new Vector2(810, 48);
        private static Vector2 TopRightPos = new Vector2(1572, 48);
        private static Vector2 BottomRightPos =  new Vector2(1572, 997);
        private static Vector2 BottomCenterPos = new Vector2(810, 997);
        private static Vector2 BottomLeftPos = new Vector2(48, 997);

        private static Vector2 meterSize = new Vector2(300, 35);

        public override void Update()
        {
            base.Update();
            Visible = Visible && StaminaMeterModule.Settings.LargeMeterEnabled;
        }

        public override void Render()
        {
            base.Render();

            switch (StaminaMeterModule.Settings.LargeMeterPosition)
            {
                case LargeMeterPositions.TopLeft:
                    // move meter out of way of speedrun timer if it's on
                    meterPosition = (Settings.Instance.SpeedrunClock == SpeedrunType.Off)
                        ? TopLeftPos
                        : TopLeftPos2;
                    break;
                case LargeMeterPositions.TopCenter:
                    meterPosition = TopCenterPos;
                    break;
                case LargeMeterPositions.TopRight:
                    meterPosition = TopRightPos;
                    break;
                case LargeMeterPositions.BottomRight:
                    meterPosition = BottomRightPos;
                    break;
                case LargeMeterPositions.BottomCenter:
                    meterPosition = BottomCenterPos;
                    break;
                case LargeMeterPositions.BottomLeft:
                    meterPosition = BottomLeftPos;
                    break;
                default:  // just in case
                    goto case LargeMeterPositions.TopLeft;
            }

            // outline
            Draw.Rect(meterPosition.X - 2, meterPosition.Y - 2, meterSize.X + 4, meterSize.Y + 4, lineColor);
            // fill
            Draw.Rect(meterPosition.X, meterPosition.Y, meterSize.X, meterSize.Y, fillColor);

            // show the meter
            if (lightRatio > 0f)
            {
                // no, just drawing two rectangles at 50% opacity does not work. i've tried.
                if (lightRatio > darkRatio)
                {
                    Draw.Rect(meterPosition, meterSize.X * lightRatio, meterSize.Y, colorDark);
                    if (darkRatio > 0)
                    {
                        Draw.Rect(meterPosition, meterSize.X * darkRatio, meterSize.Y, colorLight);
                    }
                }
                else
                {
                    if (darkRatio > 0)
                    {
                        Draw.Rect(meterPosition, meterSize.X * darkRatio, meterSize.Y, colorDark);
                    }
                    Draw.Rect(meterPosition, meterSize.X * lightRatio, meterSize.Y, colorLight);
                }
            }

            // low marker
            Draw.Rect(meterPosition.X + meterSize.X * lowMarkRatio, meterPosition.Y, 2, meterSize.Y, lineColor);
        }
    }

    public enum LargeMeterPositions
    {
        TopLeft,
        TopCenter,
        TopRight,
        BottomRight,
        BottomCenter,
        BottomLeft
    }
}
