using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentColiderController : MonoBehaviour
{
    private static bool isAttracted = false;
    
    public static bool IsAttracted { get => isAttracted; set => isAttracted = value; }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Attraction")
        {
            isAttracted=true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
