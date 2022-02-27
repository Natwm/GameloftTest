using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Basic_ObstacleBehaviours : MonoBehaviour, IDamageable
{
    #region Param
    
    [SerializeField] private GameObject visual;

    [Space]
    [Header("Param")]
    [SerializeField] private int _Health;
    [Tooltip ("The value that will be added to the timer when this object get destroyed")]
    [SerializeField] private int _IncreaseTimerValue;

    protected Sequence damageSequence;
    Sequence deathSequence;

    private float baseScale;

    [Header("Animation")]
    [SerializeField] private float scaleUptime;
    [SerializeField] private float scaleDowntime;

    [Header("VFX")]
    [SerializeField] private ParticleSystem smokeParticule;
    [SerializeField] private GameObject collisionOBJ;

    public SpriteRenderer indicator;
    public List<Color> lifeIndicator;

    [Header("Hit Animation")]
    [SerializeField] private float colorAnimationSpeed = 0.2f;
    [SerializeField] private Color hitColor = Color.white;
    private Color baseColor;
    private Material m_Mat;

    [Space]
    [Header("Life Indicator")]
    [SerializeField] private Transform indicator_Holder;
    [SerializeField] private float indicator_Space;
    [SerializeField] private GameObject indicator_Left;
    [SerializeField] private GameObject indicator_Right;
    [SerializeField] private GameObject indicator_Center;

    [Space]
    [Header("Audio")]
    [SerializeField] private AudioClip death_Sound;
    private AudioSource _Audio;

    #endregion

    private void Awake()
    {
        baseScale = transform.localScale.x;

        damageSequence = DOTween.Sequence();
        deathSequence = DOTween.Sequence();

        damageSequence.AppendCallback(() => StartCoroutine(HitFlash()))
                        .Append(transform.DOScale(baseScale + .5f, scaleUptime))
                        .Append(transform.DOScale(baseScale, scaleDowntime));

        damageSequence.Pause();

        deathSequence.AppendCallback(() => DestructionParticule())
                        .Append(transform.DOScale(baseScale + .5f, scaleUptime))
                        .Append(transform.DOScale(0, scaleDowntime))
                        .AppendCallback(() => visual.SetActive(false));

        deathSequence.Pause();

        /*try
        {*/
            m_Mat = visual.GetComponent<Renderer>().material;
            baseColor = m_Mat.GetColor("_DiffuseColor");
        /*}
        catch
        {

        }*/
        

    }

    private void Start()
    {
        SetUpLifeIndicator();
        _Audio = GetComponent<AudioSource>();
        //indicator.color = lifeIndicator[_Health];
    }


    public void DestructionParticule()
    {
        print("ok");
        smokeParticule.Play();
    }

    public IEnumerator HitFlash()
    {
        StartCoroutine(LerpColor(hitColor, colorAnimationSpeed));
        yield return new WaitForSeconds(colorAnimationSpeed);
        StartCoroutine(LerpColor(baseColor, colorAnimationSpeed));
    }

    IEnumerator LerpColor(Color endValue, float duration)
    {
        float time = 0;
        Color startValue = m_Mat.GetColor("_DiffuseColor");
        while (time < duration)
        {
            m_Mat.SetColor("_DiffuseColor", Color.Lerp(startValue, endValue, time / duration));
            time += Time.deltaTime;
            yield return null;
        }
        m_Mat.SetColor("_DiffuseColor", endValue);
    }

    /// <summary>
    /// Animation of a sprite to increase the game feel
    /// </summary>
    /// <returns> Time before the circle disappear</returns>
    protected IEnumerator ImpactCircle()
    {
        collisionOBJ.transform.DOScale(Vector3.one, 0.18f);
        yield return new WaitForSeconds(0.25f);
        collisionOBJ.transform.DOScale(Vector3.zero, 0.18f);
    }

    private void SetUpLifeIndicator()
    {
        Vector2 pos = Vector2.zero;
        GameObject elt = Instantiate(indicator_Left, indicator_Holder);
        elt.transform.localPosition = pos;

        for (int i = 0; i < _Health-2; i++)
        {
            pos += new Vector2(indicator_Space, 0);
            elt = Instantiate(indicator_Center, indicator_Holder);
            elt.transform.localPosition = pos;
        }

        pos += new Vector2(indicator_Space, 0);
        elt = Instantiate(indicator_Right, indicator_Holder);
        elt.transform.localPosition = pos;
    }


    #region Interfaces

    public virtual void GetDamage(int _amountOfDamage)
    {
        _Audio.Play();

        damageSequence.Play();
        StartCoroutine(ImpactCircle());

        _Health -= _amountOfDamage;

        if (IsDead())
            Dead();
        else
            indicator_Holder.GetChild(_Health).GetComponent<SpriteRenderer>().color = Color.black;
    }

    public bool IsDead()
    {
        return _Health <= 0;
    }

    public virtual void Dead()
    {
        _Audio.clip = death_Sound;
        _Audio.Play();
        
        GetComponent<Collider>().isTrigger = true;

        GameManager.instance.IncreaseTimer(_IncreaseTimerValue);

        transform.DOKill();
        deathSequence.Play();

        Destroy(gameObject, 2f);
    }
    #endregion
}
