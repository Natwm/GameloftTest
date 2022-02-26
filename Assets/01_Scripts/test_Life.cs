using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test_Life : MonoBehaviour
{
    public int life;

    public GameObject Left_bordure;
    public GameObject elt;
    public GameObject Right_bordure;

    // Start is called before the first frame update
    void Start()
    {
        setUpLife();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void setUpLife()
    {
        Vector2 pos = Vector2.zero;
        Instantiate(Left_bordure, pos, Quaternion.identity);
        for (int i = 0; i < life; i++)
        {
            pos += new Vector2(0.2f, 0);
            Instantiate(elt,pos,Quaternion.identity);
        }
        pos += new Vector2(0.2f, 0);
        Instantiate(Right_bordure, pos, Quaternion.identity);
    }
}
