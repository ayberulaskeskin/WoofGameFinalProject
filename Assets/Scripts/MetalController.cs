using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;






public class MetalController : MonoBehaviour
{
    
   
    
    [Header("Metal")]
    [SerializeField] private Rigidbody _Rigidbody;
    
    
     void Awake()
    {
        
    }

     private void FixedUpdate()
     {
        // DecreaseScale?.Invoke();
     }

     private void OnTriggerEnter(Collider other)
     {
         //var audioSource = GetComponent<AudioSource>();
        if (other.CompareTag("Player"))
        {
            
                
                _Rigidbody.isKinematic = false;
                
                //audioSource.Play();
            
           
           
        }
        
       
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
