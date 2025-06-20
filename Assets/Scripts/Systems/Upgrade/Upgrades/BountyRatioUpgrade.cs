using UnityEngine;

public class BountyRatioUpgrade : Upgrade
{
    #region Properties
    [Header("Upgrade Parameters")]
    [SerializeField]
    private float _bountyRatioBonus;
    public float BountyRatioBonus
    {
        get => _bountyRatioBonus;
        set => _bountyRatioBonus = value;
    }
    #endregion

    public override void Apply(GameObject upgradeable)
    {
        if (PlayerResources.Instance != null)
        {
            PlayerResources.Instance.BountyRatio += BountyRatioBonus;
        }

        Destroy(gameObject);
    }
}
