using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AgentController : MonoBehaviour
{
    [SerializeField] private GameObject _dest;
    [SerializeField] private Animator AgentAnimator;
    [SerializeField] private NavMeshAgent _agent;
    private int i;
    public float range;
    //private bool isAttracted = false;
    
    private void Awake()
    {
        _agent.GetComponent<NavMeshAgent>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

   

    // Update is called once per frame
    void Update()
    {
        range = PlayerController.AttractionRange * 40f;
        float dist = Vector3.Distance(this.transform.position, _dest.transform.position);
        if(PlayerController.IsFall)
        {
            AgentAnimator.SetTrigger("isLaughing");
            _agent.stoppingDistance = 10;
        }
        else if(PlayerController.IsFall==false)
        {_agent.stoppingDistance = 0;
            if (dist < range)
            {
                _agent.destination = _dest.transform.position;
                AgentAnimator.SetTrigger("isCatWalking");
                if (i < 1)
                {
                    PlayerController.AgentNumber = PlayerController.AgentNumber+1;
                    i =i+1;
                }
                

            }
            else if(dist>range)
            {
                if (i >= 1)
                {
                    PlayerController.AgentNumber = PlayerController.AgentNumber-1;
                    i = 0;
                    Debug.Log("agent is out of range");
                }
               
            }

        }
        if(PlayerController.IsFinish)
        {
            AgentAnimator.SetTrigger("isFinish");
            _agent.stoppingDistance = 5;
        }

      
       
    }
}
