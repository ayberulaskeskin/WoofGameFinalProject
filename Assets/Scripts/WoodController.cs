using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;






public class WoodController : MonoBehaviour
{
    
   
    
    [Header("woods")]
    [SerializeField] private List<Rigidbody> Rigidbodies;
    
    
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
            foreach (var rigidbody in Rigidbodies)
            {
                
                rigidbody.isKinematic = false;
                
                //audioSource.Play();
            }
           
           
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
