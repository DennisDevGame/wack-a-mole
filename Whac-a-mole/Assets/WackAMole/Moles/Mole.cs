using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using WackAMole.Moles.Behaviours;
using WackAMole.Moles.Behaviours.Factory;

namespace WackAMole.Moles
{
    public class Mole : MonoBehaviour
    {
        public event Action<Mole> OnMoleWacked;

        [SerializeField] private Animator animator;
        [SerializeField] private List<ParticleSystem> wackParticle;
        [SerializeField] private Material[] moleMaterials;
        [SerializeField] private SkinnedMeshRenderer moleMeshRenderer;
        
        private float displayDuration;
        private bool isWacked;
        
        private static readonly int Up = Animator.StringToHash("Up");
        private static readonly int Down = Animator.StringToHash("Down");
        
        public IMoleBehavior MoleBehavior;

        public void Initialize(IMoleBehavior moleBehavior)
        {
            MoleBehavior = moleBehavior;
            displayDuration = moleBehavior.GetDisplayDuration();
            MoleBehavior.OnWacked += HandleOnMoleWacked;
            MoleBehavior.SetAppearance(this);
        }
        
        //No a perfect solution but it just to show a visual difference between the type for now
        public void SetMoleColor(MoleBehaviorFactory.MoleBehaviorType moleType)
        {
            switch (moleType)
            {
                case MoleBehaviorFactory.MoleBehaviorType.Normal:
                    moleMeshRenderer.material = moleMaterials[0];
                    break;
                case MoleBehaviorFactory.MoleBehaviorType.Fast:
                    moleMeshRenderer.material = moleMaterials[1];
                    break;
                case MoleBehaviorFactory.MoleBehaviorType.Armored:
                    moleMeshRenderer.material = moleMaterials[2];
                    break;
            }
        }

        private void HandleOnMoleWacked()
        {
            isWacked = true;
            var randomIndex = UnityEngine.Random.Range(0, wackParticle.Count);
            wackParticle[randomIndex].Play();
            OnMoleWacked?.Invoke(this);
        }

        public void Show()
        {
            isWacked = false;
            MoleBehavior.Show(this);
        }
        
        public void ShowMoleAnimation()
        {
            animator.SetTrigger(Up);
            
            StartCoroutine(HideAfterDuration());
        }

        public void HideMoleAnimation()
        {
            animator.SetTrigger(Down);
            
            StopCoroutine(HideAfterDuration());
        }

        public void OnMouseDown()
        {
            if(isWacked)
                return;
            
            MoleBehavior.OnHit(this);
        }
    
        private IEnumerator HideAfterDuration()
        {
            yield return new WaitForSeconds(displayDuration);
            MoleBehavior.Hide(this);
        }
    }
}
