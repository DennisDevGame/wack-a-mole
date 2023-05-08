using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using WackAMole.Managers;
using WackAMole.Moles;

namespace WackAMole.Controllers
{
    public class MoleGameController : MonoBehaviour
    {
        public event Action OnGameStarted;
        public event Action<int> OnGameEnded;
        
        public event Action<int> OnScoreChanged;
        public event Action<int> GameDurationChanged;
        
        [SerializeField] private MoleSpawner moleSpawner;
        [SerializeField] private float gameDuration = 60f;

        private const string MoleTag = "Mole";
        
        private int moleScore;
        private bool inGame;

        public void StartGame()
        {
            moleSpawner.Init(this);
            inGame = true;
            
            OnGameStarted?.Invoke();
            StartCoroutine(GameTimer());
        }

        public void EndGame()
        {
            inGame = false;
            moleSpawner.ResetSpawner();
            OnGameEnded?.Invoke(moleScore);

            moleScore = 0;
        }
        
        private void Update()
        {
            if(!inGame)
                return;
            
            MissedWackCheck();
        }

        private void MissedWackCheck()
        {
            if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
            {
                var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out var hit))
                {
                    if (!hit.collider.CompareTag(MoleTag))
                    {
                        HandleMiss();
                    }
                }
            }
        }

        public void RegisterMole(Mole mole) => mole.OnMoleWacked += HandleOnMoleHit;

        public void UnRegisterMole(Mole mole) => mole.OnMoleWacked -= HandleOnMoleHit;

        private void HandleOnMoleHit(Mole mole)
        {
            moleScore++;
            OnScoreChanged?.Invoke(moleScore);
        }
        
        private void HandleMiss()
        {
            Debug.LogError("u missed....");
        }
        
        private IEnumerator GameTimer()
        {
            float remainingTime = gameDuration;
            while (remainingTime > 0f)
            {
                remainingTime -= Time.deltaTime;
                yield return null;
                GameDurationChanged?.Invoke((int)remainingTime);
            }
            EndGame();
        }
    }
}
