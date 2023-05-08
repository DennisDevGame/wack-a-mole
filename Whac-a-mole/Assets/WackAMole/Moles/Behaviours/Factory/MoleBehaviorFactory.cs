using System;

namespace WackAMole.Moles.Behaviours.Factory
{
    public static class MoleBehaviorFactory
    {
        public enum MoleBehaviorType
        {
            Normal,
            Fast,
            Armored
        }
        
        public static IMoleBehavior CreateMoleBehavior(MoleBehaviorType moleBehaviorType)
        {
            switch (moleBehaviorType)
            {
                case MoleBehaviorType.Normal:
                    return new NormalMoleBehavior();
                case MoleBehaviorType.Fast:
                    return new FastMoleBehavior();
                case MoleBehaviorType.Armored:
                    return new ArmoredMoleBehavior();
                default:
                    return new NormalMoleBehavior();
            }
        }
    }
}
