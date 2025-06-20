using UnityEngine;

public class Attacheable : Upgrade
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
    #endregion

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public override void Apply(GameObject upgradeable)
    {
        Attacher = upgradeable.GetComponent<Attacher>();
        if (Attacher != null) Attacher.AddAttachment(this);
    }
}
