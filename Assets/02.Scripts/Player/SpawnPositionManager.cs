using UnityEngine;

public class SpawnPositionManager : MonoBehaviour
{
    [SerializeField] private Transform[] _spawnPoints;

    public Vector3 GetRandomSpawnPosition()
    {
        if (_spawnPoints == null || _spawnPoints.Length == 0)
        {
            return Vector3.zero;
        }
        
        int index = Random.Range(0, _spawnPoints.Length);
        return _spawnPoints[index].position;
    }

    public Quaternion GetRandomSpawnRotation()
    {
        if (_spawnPoints == null || _spawnPoints.Length == 0)
        {
            return Quaternion.identity;
        }
        int index = Random.Range(0, _spawnPoints.Length);
        return _spawnPoints[index].rotation;
    }

    public (Vector3 position, Quaternion rotation) GetRandomSpawnPoint()
    {
        if (_spawnPoints == null || _spawnPoints.Length == 0)
        {
            return (Vector3.zero, Quaternion.identity);
        }

        int index = Random.Range(0, _spawnPoints.Length);
        Transform point = _spawnPoints[index];
        return (point.position, point.rotation);
    }
}
