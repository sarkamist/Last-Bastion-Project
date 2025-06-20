using System;

[Serializable]
public class DamageData : ICloneable
{
    public float amount;
    public DamageType type;

    public DamageData(float damageAmount, DamageType damageType)
    {
        amount = damageAmount;
        type = damageType;
    }

    public object Clone()
    {
        return MemberwiseClone();
    }
}
