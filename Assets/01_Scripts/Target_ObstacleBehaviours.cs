using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target_ObstacleBehaviours : Basic_ObstacleBehaviours
{
    public override void Dead()
    {
        GameManager.instance.ReduceAmountofElement();
        base.Dead();
    }
}
