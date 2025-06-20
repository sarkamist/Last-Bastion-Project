using System.Collections.Generic;
using UnityEngine;

public abstract class Upgrade : MonoBehaviour
{
    #region Properties
    [Header("Shop Parameters")]
    [SerializeField]
    private string _displayName = "Attacheable";
    public string DisplayName
    {
        get => _displayName;
        set => _displayName = value;
    }

    [SerializeField]
    private int _goldCost = 10;
    public int GoldCost
    {
        get => _goldCost;
        set => _goldCost = value;
    }

    [SerializeField]
    private List<string> _tooltipDescription = new List<string> { "Buy this upgrade to add into your Bastion's defenses." };
    public List<string> TooltipDescription
    {
        get => _tooltipDescription;
        set => _tooltipDescription = value;
    }
    #endregion

    public abstract void Apply(GameObject upgradeable);
}
