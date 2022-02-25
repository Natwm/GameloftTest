using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugManager : MonoBehaviour
{


    public static DebugManager instance;

    public bool isMobil;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.Log("");
        }
        else
            instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
