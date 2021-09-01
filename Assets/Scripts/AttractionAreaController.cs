using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttractionAreaController : MonoBehaviour
{
    
    
    
    
    void  IncreaseScale()
    {
        
        transform.localScale = new Vector3(x:0.4f,y:0.4f ,z :0.4f)*PlayerController.AttractionRange*10f;
       // if (PlayerController.IsFall)
       // {
       //   transform.localScale = new Vector3(x:5f,y:0.05f ,z :5f)*PlayerController.AttractionRange*0f;
       // }
       // else
       // {
           // transform.localScale = new Vector3(x:5f,y:0.05f ,z :5f)*PlayerController.AttractionRange*7f;
      //  }
       
        
    }

    private void Update()
    {
       
        IncreaseScale();
    }
}
