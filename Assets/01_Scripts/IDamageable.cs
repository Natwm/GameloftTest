
public interface IDamageable
{
    public void GetDamage(int _amountOfDamage);

    public bool IsDead();

    public abstract void Dead();
}
