using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UI_Score : MonoBehaviour
{
    [SerializeField] private List<UI_ScoreItem> _scoreItems;
    [SerializeField] private float _updateInterval = 0.5f;

    private float _updateTimer;

    private void Update()
    {
        _updateTimer += Time.deltaTime;
        if (_updateTimer < _updateInterval) return;

        _updateTimer = 0f;
        UpdateScoreboard();
    }

    private void UpdateScoreboard()
    {
        IReadOnlyList<PlayerController> topPlayers = FindObjectsByType<PlayerController>(FindObjectsSortMode.None)
            .OrderByDescending(player => player.Stat.Score)
            .Take(_scoreItems.Count)
            .ToArray();

        for (int i = 0; i < _scoreItems.Count; i++)
        {
            if (i < topPlayers.Count)
            {
                _scoreItems[i].SetData(
                    i + 1,
                    topPlayers[i].PhotonView.Owner.NickName,
                    topPlayers[i].Stat.Score
                );
            }
            else
            {
                _scoreItems[i].Clear();
            }
        }
    }
}