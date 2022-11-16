using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using WaypointsFree;

public class NpcCtrl : MonoBehaviour
{
    public enum EndReachedBehavior
    {
        STOP,
        LOOP,
        PINGPONG
    } 
    private NavMeshAgent agent;
    public WaypointsGroup waypoints;
    public int count;
    private int addNumber = 1;
    
    public EndReachedBehavior _endReachedBehavior;
    [Header("Chasing Option")]
    public bool isChasing;
    public GameObject target;
    
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        agent.SetDestination(waypoints.waypoints[count].XY);
    }

    // Update is called once per frame
    void Update()
    {
        if(isChasing){
            agent.SetDestination(target.transform.position);
        }
        else{
            if(agent.remainingDistance < 0.5){
                setNextWayPoint();
            }
        }
    }

    void setNextWayPoint(){
        count += addNumber;
        
        if(waypoints.waypoints.Count <= count || count < 0){
            switch (_endReachedBehavior)
            {
                case EndReachedBehavior.STOP:
                    addNumber = 0;
                break;
                case EndReachedBehavior.LOOP:
                    count = 0;
                break;
                case EndReachedBehavior.PINGPONG:
                    addNumber = -addNumber;
                return;
            }
        }

        agent.SetDestination(waypoints.waypoints[count].XY);
    }
    
}
