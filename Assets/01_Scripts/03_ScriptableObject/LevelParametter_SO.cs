using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelParametter
{
    public int second, minute;
}


[CreateAssetMenu(fileName = "New LevelParametter", menuName = "New Scriptable Object/New CaseContener")]
public class LevelParametter_SO : ScriptableObject
{
    [SerializeField] private LevelParametter[] parametter = new LevelParametter[3];

    public int GetScore(float time)
    {
        int parametterValue;
        int amountOfStar = 0;

        foreach (var item in parametter)
        {
            parametterValue = item.second + (item.minute * 60);
            if (time < parametterValue)
                amountOfStar++;
            else
                return amountOfStar;
        }

        return amountOfStar;
    }
}
