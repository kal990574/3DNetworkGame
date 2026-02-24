using UnityEngine;

public class CopyPosition : MonoBehaviour
{
    [SerializeField] private bool _x, _y, _z;
    [SerializeField] private Transform _target;

    public void SetTarget(Transform target)
    {
        _target = target;
    }

    private void LateUpdate()
    {
        if (_target == null) return;

        transform.position = new Vector3(
            _x ? _target.position.x : transform.position.x,
            _y ? _target.position.y : transform.position.y,
            _z ? _target.position.z : transform.position.z);
    }
}