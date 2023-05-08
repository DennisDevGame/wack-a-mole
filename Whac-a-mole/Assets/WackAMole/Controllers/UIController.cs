using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using WackAMole.Managers;
using WackAMole.UI;

namespace WackAMole.Controllers
{
    public class UIController : MonoBehaviour
    {
        [SerializeField] private Button playButton;
        [SerializeField] private TextMeshProUGUI wackLabel;
        [SerializeField] private GameController gameController;
        [SerializeField] private GameObject generalUI;
        [SerializeField] private GameObject highScoreInputPanel;
        [SerializeField] private TMP_InputField playerNameInputField;
        [SerializeField] private Button submitHighScoreButton;
        [SerializeField] private Transform highScoreListParent;
        [SerializeField] private RankingListObject highScoreEntryPrefab;
        [SerializeField] private TextMeshProUGUI countdownText;
        
        private const string PrefixWackLabel = "Wacks: ";

        private void Awake()
        {
            playButton.onClick.AddListener(() =>
            {
                gameController.StartGame();
                EnableGeneralUI(false);
            });

            submitHighScoreButton.onClick.AddListener(SubmitHighScore);

            EnableGeneralUI(true);
            highScoreInputPanel.SetActive(false);
        }

        public void UpdateScoreText(int newScore)
        {
            wackLabel.text = PrefixWackLabel + newScore;
        }

        public void EnableGeneralUI(bool on)
        {
            generalUI.SetActive(on);
        }

        public void ShowHighScoreInput(int rank, Action<string> onPlayerNameSubmitted)
        {
            EnableGeneralUI(false);
            highScoreInputPanel.SetActive(true);
            submitHighScoreButton.onClick.RemoveAllListeners();
            submitHighScoreButton.onClick.AddListener(() =>
            {
                onPlayerNameSubmitted?.Invoke(playerNameInputField.text);
                highScoreInputPanel.SetActive(false);
                EnableGeneralUI(true);
            });
        }
        
        public void UpdateCountdownText(int remainingTime)
        {
            countdownText.text = "Time Left: " + remainingTime + "s";
        }
        
        private void SubmitHighScore()
        {
            submitHighScoreButton.onClick.Invoke();
            submitHighScoreButton.onClick.RemoveListener(SubmitHighScore);
        }

        public void UpdateHighScores(List<ScoreEntry> highScores)
        {
            foreach (Transform child in highScoreListParent)
            {
                Destroy(child.gameObject);
            }

            foreach (var highScore in highScores)
            {
                var entry = Instantiate(highScoreEntryPrefab, highScoreListParent);
                entry.SetRankingName(highScore.PlayerName);
                entry.SetRankingScore(highScore.Score);
            }
        }
    }
}
