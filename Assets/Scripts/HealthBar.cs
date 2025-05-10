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
        if (gameObject.GetComponent<Damageable>() is Damageable damageable)
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
        if (HealthBarFill is null) return;
        Debug.Log("DamageTaken");

        HealthBarFill.fillAmount = context.origin.CurrentHealth / context.origin.MaxHealth;
    }
}
