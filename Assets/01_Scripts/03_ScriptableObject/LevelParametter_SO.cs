using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelParametter
{
    public int second, minute;

    public float GetParametterInSecond()
    {
        return minute * 60 + second;
    }
}


[CreateAssetMenu(fileName = "New LevelParametter", menuName = "New Scriptable Object/New CaseContener")]
public class LevelParametter_SO : ScriptableObject
{
    [SerializeField] private LevelParametter initialTimer;
    [SerializeField] private LevelParametter[] parametter = new LevelParametter[3];

    public LevelParametter InitialTimer { get => initialTimer; set => initialTimer = value; }

    public int GetScore(float time)
    {
        int parametterValue;
        int amountOfStar = 0;

        foreach (var item in parametter)
        {
            parametterValue = Mathf.RoundToInt(item.GetParametterInSecond());
            if (time > parametterValue)
                amountOfStar++;
            else
                return amountOfStar;
        }

        return amountOfStar;
    }
}
