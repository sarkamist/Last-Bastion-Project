using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class Attacheable : MonoBehaviour
{
    #region Properties
    [Header("Attacheable Parameters")]
    [SerializeField, ReadOnly]
    private Attacher _attacher;
    public Attacher Attacher
    {
        get => _attacher;
        private set => _attacher = value;
    }

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

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public virtual void AttachTo(Attacher attacher)
    {
        Attacher = attacher;
    }
}
