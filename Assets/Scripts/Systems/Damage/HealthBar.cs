using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Damageable))]
public class HealthBar : MonoBehaviour
{
    #region Properties
    [SerializeField, ReadOnly]
    private Damageable _damageableRef;
    public Damageable DamageableRef
    {
        get => _damageableRef;
        private set => _damageableRef = value;
    }

    [SerializeField, ReadOnly]
    private Camera _cameraRef;
    public Camera CameraRef
    {
        get => _cameraRef;
        private set => _cameraRef = value;
    }

    [SerializeField]
    public GameObject _healthBarCanvas;
    public GameObject HealthBarCanvas
    {
        get => _healthBarCanvas;
        set => _healthBarCanvas = value;
    }

    [SerializeField]
    public Image _healthBarFill;
    public Image HealthBarForeground
    {
        get => _healthBarFill;
        set => _healthBarFill = value;
    }

    [SerializeField, ReadOnly]
    private float _targetFill;
    public float TargetFill
    {
        get => _targetFill;
        private set => _targetFill = value;
    }

    [SerializeField]
    public float _animationSpeed = 2f;
    public float AnimationSpeed
    {
        get => _animationSpeed;
        set => _animationSpeed = value;
    }
    #endregion

    void Start()
    {
        Damageable damageable = GetComponent<Damageable>();
        if (damageable != null)
        {
            DamageableRef = damageable;
            DamageableRef.DamageTakenEvent += OnDamageTaken;
        }
        else
        {
            DamageableRef = null;
        }

        CameraRef = Camera.main;

        _targetFill = 1f;
    }

    void Update() {
        HealthBarCanvas.transform.rotation = CameraRef.transform.rotation;
        HealthBarForeground.fillAmount = Mathf.MoveTowards(HealthBarForeground.fillAmount, TargetFill, AnimationSpeed * Time.deltaTime);
    }

    void OnDamageTaken(Damageable.DamageTakenContext context) {
        if (HealthBarForeground == null) return;

        TargetFill = (float) (context.origin.CurrentHealth / context.origin.MaxHealth);
    }
}
