using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Basic_ObstacleBehaviours : MonoBehaviour, IDamageable
{
    #region Param
    [SerializeField] private int _Health;
    protected Sequence damageSequence;
    Sequence deathSequence;

    [Header("Animation")]
    [SerializeField] private float scaleUptime;
    [SerializeField] private float scaleDowntime;

    [Header("VFX")]
    [SerializeField] private ParticleSystem smokeParticule;
    [SerializeField] private GameObject collisionOBJ;

    public SpriteRenderer indicator;
    public List<Color> lifeIndicator;

    #endregion

    private void Awake()
    {
        damageSequence = DOTween.Sequence();
        deathSequence = DOTween.Sequence();

        damageSequence.AppendCallback(() => DamageParticule())
                        .Append(transform.DOScale(1.5f, scaleUptime))
                        .Append(transform.DOScale(1, scaleDowntime));

        damageSequence.Pause();

        deathSequence.AppendCallback(() => DamageParticule())
                        .Append(transform.DOScale(1.5f, scaleUptime))
                        .Append(transform.DOScale(0, scaleDowntime));

        deathSequence.Pause();
    }

    private void Start()
    {
        indicator.color = lifeIndicator[_Health];
    }

    public void DamageParticule()
    {
        smokeParticule.Play();
    }

    /// <summary>
    /// Animation of a sprite to increase the game feel
    /// </summary>
    /// <returns> Time before the circle disappear</returns>
    protected IEnumerator ImpactCircle()
    {
        collisionOBJ.transform.DOScale(Vector3.one, 0.2f);
        yield return new WaitForSeconds(0.3f);
        collisionOBJ.transform.DOScale(Vector3.zero, 0.2f);
    }

    #region Interfaces

    public virtual void GetDamage(int _amountOfDamage)
    {
        damageSequence.Play();
        StartCoroutine(ImpactCircle());
        _Health -= _amountOfDamage;
        if (IsDead())
            Dead();
        else
            indicator.color = lifeIndicator[_Health];
    }

    public bool IsDead()
    {
        return _Health <= 0;
    }

    public virtual void Dead()
    {
        GetComponent<Collider>().isTrigger = true;
        GameManager.instance.ReduceAmountofElement();
        transform.DOKill();
        deathSequence.Play();
        Destroy(gameObject,0.35f);
    }
    #endregion
}
