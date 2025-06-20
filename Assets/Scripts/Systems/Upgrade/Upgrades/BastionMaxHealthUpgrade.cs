using UnityEngine;

public class BastionMaxHealthUpgrade : Upgrade
{
    #region Properties
    [Header("Upgrade Parameters")]
    [SerializeField]
    private float _maximumHealthBonus;
    public float MaximumHealthBonus
    {
        get => _maximumHealthBonus;
        set => _maximumHealthBonus = value;
    }
    #endregion

    public override void Apply(GameObject upgradeable)
    {
        Damageable damageable = upgradeable.GetComponent<Damageable>();
        if (damageable != null)
        {
            damageable.MaxHealth += MaximumHealthBonus;
            damageable.GainHealth(gameObject, MaximumHealthBonus);
        }

        Destroy(gameObject);
    }
}
