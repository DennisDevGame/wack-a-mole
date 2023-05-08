using System;
using UnityEngine;
using UnityEngine.Serialization;
using WackAMole.Managers;

namespace WackAMole.Controllers
{
    public class GameController : MonoBehaviour
    {
        [SerializeField] private MoleGameController moleGameController;
        [SerializeField] private UIController moleUIController;
        [SerializeField] private ScoreManager scoreManager;
        
        private void Start()
        {
            moleGameController.OnGameStarted += HandleOnGameStarted;
            moleGameController.OnGameEnded += HandleOnGameEnded;
            moleGameController.OnScoreChanged += HandleOnScoreChanged;
            moleGameController.GameDurationChanged += HandleOnGameTimingChanged;
            scoreManager.OnHighScoreAchieved += HandleOnHighScoreAchieved;
            scoreManager.OnNoHighScoreAchieved += HandleOnNoHighScoreAchieved;
            
            moleUIController.UpdateCountdownText(0);
            moleUIController.UpdateHighScores(scoreManager.LoadScores());
        }

        private void HandleOnGameTimingChanged(int timeLeft)
        {
            moleUIController.UpdateCountdownText(timeLeft);
        }

        public void StartGame() => moleGameController.StartGame();

        private void HandleOnGameEnded(int endScore) => scoreManager.CheckForHighScore(endScore);

        private void HandleOnGameStarted() => moleUIController.UpdateScoreText(0);

        private void HandleOnScoreChanged(int score) => moleUIController.UpdateScoreText(score);
        
        private void HandleOnHighScoreAchieved(int rank, int score)
        {
            moleUIController.ShowHighScoreInput(rank, (playerName) => 
            {
                scoreManager.SaveScore(playerName, score);
                moleUIController.UpdateHighScores(scoreManager.LoadScores());
            });
        }
        
        private void HandleOnNoHighScoreAchieved() => moleUIController.EnableGeneralUI(true);

        private void OnDestroy()
        {
            moleGameController.OnGameStarted -= HandleOnGameStarted;
            moleGameController.OnGameEnded -= HandleOnGameEnded;
            moleGameController.OnScoreChanged -= HandleOnScoreChanged;
            moleGameController.GameDurationChanged -= HandleOnGameTimingChanged;
            scoreManager.OnHighScoreAchieved -= HandleOnHighScoreAchieved;
            scoreManager.OnNoHighScoreAchieved -= HandleOnNoHighScoreAchieved;
        }
    }
}
