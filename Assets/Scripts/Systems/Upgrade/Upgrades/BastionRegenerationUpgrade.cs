using UnityEngine;

public class BastionRegenerationUpgrade : Upgrade
{
    #region Properties
    [Header("Upgrade Parameters")]
    [SerializeField]
    private float _regenerationRateBonus;
    public float RegenerationRateBonus
    {
        get => _regenerationRateBonus;
        set => _regenerationRateBonus = value;
    }
    #endregion

    public override void Apply(GameObject upgradeable)
    {
        Damageable damageable = upgradeable.GetComponent<Damageable>();
        if (damageable != null)
        {
            damageable.RegenerationRate += RegenerationRateBonus;
        }

        Destroy(gameObject);
    }
}
