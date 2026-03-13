using UnityEngine;

public class PlayerScanAbility : PlayerAbility
{
    [Header("스캔 설정")]
    [SerializeField] private Material _scanMaterial;
    [SerializeField] private float _maxRadius = 50f;
    [SerializeField] private float _expandSpeed = 30f;
    [SerializeField] private float _scanWidth = 2f;
    [SerializeField] private float _scanIntensity = 3f;
    [SerializeField] private Color _scanColor = new Color(0.4f, 0.8f, 1f, 1f);
    [SerializeField] private float _falloff = 1.5f;
    [SerializeField] private float _cooldown = 5f;
    [SerializeField] private KeyCode _scanKey = KeyCode.Q;

    [Header("방향 설정")]
    [SerializeField] [Range(10f, 180f)] private float _scanAngle = 60f;
    [SerializeField] private float _angleSoftness = 0.1f;

    private static readonly int PropOrigin = Shader.PropertyToID("_ScanOrigin");
    private static readonly int PropRadius = Shader.PropertyToID("_ScanRadius");
    private static readonly int PropWidth = Shader.PropertyToID("_ScanWidth");
    private static readonly int PropIntensity = Shader.PropertyToID("_ScanIntensity");
    private static readonly int PropColor = Shader.PropertyToID("_ScanColor");
    private static readonly int PropFalloff = Shader.PropertyToID("_ScanFalloff");
    private static readonly int PropDirection = Shader.PropertyToID("_ScanDirection");
    private static readonly int PropAngle = Shader.PropertyToID("_ScanAngle");
    private static readonly int PropAngleSoftness = Shader.PropertyToID("_ScanAngleSoftness");

    private float _currentRadius;
    private bool _isScanning;
    private float _cooldownTimer;

    private void Start()
    {
        ResetMaterial();
    }

    private void Update()
    {
        if (!photonView.IsMine) return;
        if (_owner.IsDead) return;

        UpdateCooldown();
        HandleInput();
        UpdateScan();
    }

    private void HandleInput()
    {
        if (Input.GetKeyDown(_scanKey) && !_isScanning && _cooldownTimer <= 0f)
        {
            StartScan(transform.position, transform.forward);
        }
    }

    private void StartScan(Vector3 origin, Vector3 direction)
    {
        _isScanning = true;
        _currentRadius = 0f;
        _cooldownTimer = _cooldown;

        _scanMaterial.SetVector(PropOrigin, origin);
        _scanMaterial.SetVector(PropDirection, direction);
        _scanMaterial.SetFloat(PropAngle, _scanAngle);
        _scanMaterial.SetFloat(PropAngleSoftness, _angleSoftness);
        _scanMaterial.SetFloat(PropWidth, _scanWidth);
        _scanMaterial.SetFloat(PropIntensity, _scanIntensity);
        _scanMaterial.SetColor(PropColor, _scanColor);
        _scanMaterial.SetFloat(PropFalloff, _falloff);
    }

    private void UpdateScan()
    {
        if (!_isScanning) return;

        _currentRadius += _expandSpeed * Time.deltaTime;
        _scanMaterial.SetFloat(PropRadius, _currentRadius);

        if (_currentRadius >= _maxRadius)
        {
            StopScan();
        }
    }

    private void UpdateCooldown()
    {
        if (_cooldownTimer > 0f)
        {
            _cooldownTimer -= Time.deltaTime;
        }
    }

    private void StopScan()
    {
        _isScanning = false;
        ResetMaterial();
    }

    private void ResetMaterial()
    {
        if (_scanMaterial == null) return;

        _scanMaterial.SetFloat(PropRadius, 0f);
        _scanMaterial.SetFloat(PropIntensity, 0f);
    }

    private void OnDisable()
    {
        ResetMaterial();
    }
}