using System;
using UnityEngine;
using WackAMole.Moles.Behaviours.Factory;

namespace WackAMole.Moles.Behaviours
{
    public class FastMoleBehavior : IMoleBehavior
    {
        public void Show(Mole mole)
        {
            mole.ShowMoleAnimation();
        }

        public void Hide(Mole mole)
        {
            mole.HideMoleAnimation(); 
        }

        public void OnHit(Mole mole)
        {
            Debug.Log("A fast mole has been wacked");
            Hide(mole);
            OnWacked?.Invoke();
        }
    
        public void SetAppearance(Mole mole)
        {
            mole.SetMoleColor(MoleBehaviorFactory.MoleBehaviorType.Fast); 
        }

        public float GetDisplayDuration()
        {
            return 1.5f;
        }

        public event Action OnWacked;
    }
}
