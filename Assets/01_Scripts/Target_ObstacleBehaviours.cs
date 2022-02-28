using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Target_ObstacleBehaviours : Basic_ObstacleBehaviours
{
    public GameObject TargetIndicator;
    private void Start()
    {
        SetUpLifeIndicator();
        _Audio = GetComponent<AudioSource>();

        transform.DOScale(1.05f, 1f).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);
        TargetIndicator.GetComponent<SpriteRenderer>().DOColor(Color.red, 1f).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);
    }

    public override void Dead()
    {
        GameManager.instance.ReduceAmountofElement();
        base.Dead();
    }
}
