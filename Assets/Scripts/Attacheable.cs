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
