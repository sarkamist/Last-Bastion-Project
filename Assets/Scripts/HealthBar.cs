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

    [SerializeField]
    public Image _healthBarFill;
    public Image HealthBarFill
    {
        get => _healthBarFill;
        set => _healthBarFill = value;
    }
    #endregion

    void Start()
    {
        Damageable damageable = GetComponent<Damageable>();
        if (damageable != null)
        {
            DamageableRef = damageable;
            DamageableRef.DamageTaken += OnDamageTaken;
        }
        else
        {
            DamageableRef = null;
        }
    }

    void OnDamageTaken(Damageable.DamageTakenContext context) {
        if (HealthBarFill == null) return;

        HealthBarFill.fillAmount = context.origin.CurrentHealth / context.origin.MaxHealth;
    }
}
