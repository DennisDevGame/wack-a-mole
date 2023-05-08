using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

namespace WackAMole.Managers
{
    public class ScoreManager : MonoBehaviour
    {
        //Rank,score
        public event Action<int,int> OnHighScoreAchieved;
        public event Action OnNoHighScoreAchieved;

        [SerializeField] private int scoreboardSize = 10;

        public void CheckForHighScore(int score)
        {
            var scores = LoadScores();

            if (scores.Count < scoreboardSize || scores.Last().Score < score)
            {
                OnHighScoreAchieved?.Invoke(scores.Count < scoreboardSize ? scores.Count : scoreboardSize - 1, score);
            }
            
            OnNoHighScoreAchieved?.Invoke();
        }

        public void SaveScore(string playerName, int score)
        {
            var scores = LoadScores();
            var newEntry = new ScoreEntry(playerName, score);

            scores.Add(newEntry);
            scores = scores.OrderByDescending(entry => entry.Score).Take(scoreboardSize).ToList();

            for (int i = 0; i < scores.Count; i++)
            {
                PlayerPrefs.SetString($"Scoreboard_Name_{i}", scores[i].PlayerName);
                PlayerPrefs.SetInt($"Scoreboard_Score_{i}", scores[i].Score);
            }

            PlayerPrefs.Save();
        }

        public List<ScoreEntry> LoadScores()
        {
            var scores = new List<ScoreEntry>();

            for (int i = 0; i < scoreboardSize; i++)
            {
                if (PlayerPrefs.HasKey($"Scoreboard_Name_{i}") && PlayerPrefs.HasKey($"Scoreboard_Score_{i}"))
                {
                    string playerName = PlayerPrefs.GetString($"Scoreboard_Name_{i}");
                    int score = PlayerPrefs.GetInt($"Scoreboard_Score_{i}");

                    scores.Add(new ScoreEntry(playerName, score));
                }
                else
                {
                    break;
                }
            }

            return scores;
        }
    }

    [Serializable]
    public class ScoreEntry
    {
        public string PlayerName;
        public int Score;

        public ScoreEntry(string playerName, int score)
        {
            PlayerName = playerName;
            Score = score;
        }
    }
}
