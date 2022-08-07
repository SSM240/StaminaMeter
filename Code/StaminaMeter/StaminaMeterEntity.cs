using System;
using Monocle;

namespace Celeste.Mod.StaminaMeter
{
    class StaminaMeterEntity : Entity
    {
        private SmallStaminaMeterDisplay smallStaminaDisplay;
        private LargeStaminaMeterDisplay largeStaminaDisplay;

        public StaminaMeterEntity()
        {
            Tag = Tags.TransitionUpdate | Tags.PauseUpdate | TagsExt.SubHUD;
        }

        public override void Added(Scene scene)
        {
            base.Added(scene);
            smallStaminaDisplay = new SmallStaminaMeterDisplay();
            largeStaminaDisplay = new LargeStaminaMeterDisplay();
            Add(smallStaminaDisplay);
            Add(largeStaminaDisplay);
        }
    }
}
