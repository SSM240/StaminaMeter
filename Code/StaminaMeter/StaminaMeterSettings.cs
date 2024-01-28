using System;

namespace Celeste.Mod.StaminaMeter
{
    public class StaminaMeterSettings : EverestModuleSettings
    {
        private static readonly string[] SmallPosStrings = new string[]
        {
            "S_POS_ABOVE",
            "S_POS_BELOW"
        };

        private static readonly string[] LargePosStrings = new string[]
        {
            "L_POS_TOPLEFT",
            "L_POS_TOPCENTER",
            "L_POS_TOPRIGHT",
            "L_POS_BOTTOMRIGHT",
            "L_POS_BOTTOMCENTER",
            "L_POS_BOTTOMLEFT"
        };

        public bool SmallMeterEnabled { get; set; } = false;
        public bool LargeMeterEnabled { get; set; } = false;

        public bool StaminaMeterEnabled { get; set; } = true;
        public bool StarFlyTimerEnabled { get; set; } = true;

        public SmallMeterPositions SmallMeterPosition { get; set; } = SmallMeterPositions.Above;
        public LargeMeterPositions LargeMeterPosition { get; set; } = LargeMeterPositions.TopRight;

        public string NormalStaminaColor { get; set; } = "00ff00";
        public string LowStaminaColor { get; set; } = "ff0000";

        public string NormalStarFlyTimeColor { get; set; } = "ffff00";
        public string LowStarFlyTimeColor { get; set; } = "ff0000";

        [SettingIgnore]
        public string FillColor { get; set; } = "000000";
        [SettingIgnore]
        public string LineColor { get; set; } = "3c3c3c";

        [SettingSubText("MODOPTIONS_STAMINAMETER_SHOWMORETHANMAXSTAMINA_DESCRIPTION")]
        public bool ShowMoreThanMaxStamina { get; set; } = true;

        public bool HideWhilePaused { get; set; } = false;

        // called automatically by Everest to override the menu entry creation
        public void CreateSmallMeterPositionEntry(TextMenu menu, bool inGame)
        {
            menu.Add(
                new TextMenu.Slider(
                    Dialog.Clean("MODOPTIONS_STAMINAMETER_SMALLMETERPOSITION"),  // label
                    i => Dialog.Clean(SmallPosStrings[i]),  // option choices shown
                    0,                                      // min
                    SmallPosStrings.Length - 1,             // max
                    (int)SmallMeterPosition                 // initial value
                )                                           
                .Change(UpdateSmallMeterPosition)           // called when setting is changed
            );
        }

        public void CreateLargeMeterPositionEntry(TextMenu menu, bool inGame)
        {
            menu.Add(
                new TextMenu.Slider(
                    Dialog.Clean("MODOPTIONS_STAMINAMETER_LARGEMETERPOSITION"),
                    i => Dialog.Clean(LargePosStrings[i]),
                    0,
                    LargePosStrings.Length - 1,
                    (int)LargeMeterPosition
                )
                .Change(UpdateLargeMeterPosition)
            );
        }

        private void UpdateSmallMeterPosition(int pos)
        {
            SmallMeterPosition = (SmallMeterPositions)pos;
        }
        private void UpdateLargeMeterPosition(int pos)
        {
            LargeMeterPosition = (LargeMeterPositions)pos;
        }
    }
}
