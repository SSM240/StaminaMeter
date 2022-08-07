using System;

namespace Celeste.Mod.StaminaMeter
{
    public class StaminaMeterModule : EverestModule
    {
        public static StaminaMeterModule Instance;

        public StaminaMeterModule()
        {
            Instance = this;
        }

        public override Type SettingsType => typeof(StaminaMeterSettings);
        public static StaminaMeterSettings Settings => (StaminaMeterSettings)Instance._Settings;

        public override void Load()
        {
            On.Celeste.Level.LoadLevel += AddStaminaMeter;
        }

        public override void Unload()
        {
            On.Celeste.Level.LoadLevel -= AddStaminaMeter;
        }

        public void AddStaminaMeter(On.Celeste.Level.orig_LoadLevel orig, Level level, Player.IntroTypes playerIntro, bool isFromLoader)
        {
            orig(level, playerIntro, isFromLoader);

            // only try to add if a player exists
            // prevents crashing when loading a level without a player spawn object
            if (level.Tracker.GetEntity<Player>() != null)
            {
                level.Add(new StaminaMeterEntity());
            }
        }
    }
}
