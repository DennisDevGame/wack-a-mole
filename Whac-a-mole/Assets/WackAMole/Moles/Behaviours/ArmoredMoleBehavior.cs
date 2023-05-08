using System;
using UnityEngine;
using WackAMole.Moles.Behaviours.Factory;

namespace WackAMole.Moles.Behaviours
{
    public class ArmoredMoleBehavior : IMoleBehavior
    {
        private int hitsRequired = 2;
        private int currentHits = 0;

        public void Show(Mole mole)
        {
            mole.ShowMoleAnimation(); // Regular show animation
        }

        public void Hide(Mole mole)
        {
            mole.HideMoleAnimation(); // Regular hide animation
        }

        public void OnHit(Mole mole)
        {
            currentHits++;

            if (currentHits >= hitsRequired)
            {
                Debug.Log("An armored mole has been wacked");
                Hide(mole);
                OnWacked?.Invoke();
            }
        }
        
        public void SetAppearance(Mole mole)
        {
            mole.SetMoleColor(MoleBehaviorFactory.MoleBehaviorType.Armored); 
        }

        public float GetDisplayDuration()
        {
            return 4f;
        }

        public event Action OnWacked;
    }
}

