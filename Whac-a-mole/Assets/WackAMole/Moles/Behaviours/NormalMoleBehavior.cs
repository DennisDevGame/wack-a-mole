using System;
using UnityEngine;
using WackAMole.Moles.Behaviours.Factory;

namespace WackAMole.Moles.Behaviours
{
    public class NormalMoleBehavior : IMoleBehavior
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
            Debug.Log("A normal mole has been wacked");
            Hide(mole);
            OnWacked?.Invoke();
        }

        public float GetDisplayDuration()
        {
            return 3f;
        }
        
        public void SetAppearance(Mole mole)
        {
            mole.SetMoleColor(MoleBehaviorFactory.MoleBehaviorType.Normal); 
        }
        
        public event Action OnWacked;
    }
}
