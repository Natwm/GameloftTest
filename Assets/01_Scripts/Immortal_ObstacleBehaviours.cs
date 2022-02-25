using DG.Tweening;

public class Immortal_ObstacleBehaviours : Basic_ObstacleBehaviours
{
    public void Start()
    {
        
    }

    public override void GetDamage(int _amountOfDamage)
    {
        damageSequence.Play();
        StartCoroutine(ImpactCircle());
    }
}
