public class Damage
{
    public enum DamageType
    {
        Physical = 1,
        Magical = 2
    }

    public DamageType damageType { get; set; }
    public float damageAmount { get; set; }

    private Damage(DamageType damageType, float damageAmount)
    {
        this.damageType = damageType;
        this.damageAmount = damageAmount;
    }

    public static Damage Get(DamageType damageType, float damageAmount)
    {
        return new Damage(damageType, damageAmount);
    }
}
