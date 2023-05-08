using System;

namespace WackAMole.Moles.Behaviours
{
    public interface IMoleBehavior
    {
        void Show(Mole mole);
        void Hide(Mole mole);
        void OnHit(Mole mole);
        float GetDisplayDuration();
        
        void SetAppearance(Mole mole);
        
        event Action OnWacked;
    }
}
