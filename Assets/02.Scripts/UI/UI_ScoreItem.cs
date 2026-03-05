using TMPro;
using UnityEngine;

public class UI_ScoreItem : MonoBehaviour
{
    [SerializeField] private TMP_Text _rankText;
    [SerializeField] private TMP_Text _nicknameText;
    [SerializeField] private TMP_Text _scoreText;

    public void SetData(int rank, string nickname, int score)
    {
        _rankText.text = rank.ToString();
        _nicknameText.text = nickname;
        _scoreText.text = score.ToString();
    }

    public void Clear()
    {
        _rankText.text = "";
        _nicknameText.text = "";
        _scoreText.text = "";
    }
}