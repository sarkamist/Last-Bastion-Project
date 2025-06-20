using UnityEngine;

public class IncomeUpgrade : Upgrade
{
    #region Properties
    [Header("Upgrade Parameters")]
    [SerializeField]
    private float _incomeRateBonus;
    public float IncomeRateBonus
    {
        get => _incomeRateBonus;
        set => _incomeRateBonus = value;
    }
    #endregion

    public override void Apply(GameObject upgradeable)
    {
        if (PlayerResources.Instance != null)
        {
            PlayerResources.Instance.IncomeRatio += IncomeRateBonus;
        }

        Destroy(gameObject);
    }
}
