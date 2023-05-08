using TMPro;
using UnityEngine;

namespace WackAMole.UI
{
    public class RankingListObject : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI rankingName;
        [SerializeField] private TextMeshProUGUI rankingScore;

        public void SetRankingName(string name) => rankingName.text = name;
        public void SetRankingScore(int score) => rankingScore.text = score.ToString();
    }
}
