using Photon.Pun;
using UnityEngine;

public class PlayerItemDropAbility : PlayerAbility
{
    private const float DROP_RADIUS = 2f;

    public void DropItems()
    {
        if (!photonView.IsMine) return;

        int itemCount = Random.Range(3, 6);
        Vector3 deathPosition = transform.position;

        for (int i = 0; i < itemCount; i++)
        {
            Vector2 randomOffset = Random.insideUnitCircle * DROP_RADIUS;
            Vector3 spawnPosition = deathPosition + new Vector3(randomOffset.x, 0.5f, randomOffset.y);

            PhotonNetwork.Instantiate("ScoreItem", spawnPosition, Quaternion.identity);
        }
    }
}