using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomIdle : StateMachineBehaviour
{
    float time = 0;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    //override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //}

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        time += Time.deltaTime;
        
        if(time < 5f){
            return;
        }

        List<(int, float)> randomList = new List<(int, float)>();

        randomList.Add((1, 40));
        randomList.Add((2, 40));
        
        float sum = 0;
        foreach(var a in randomList){
            sum += a.Item2;
        }
        
        float random = Random.Range(0, sum);
        Debug.Log(random);
        Debug.Log(sum);
        sum = 0;
        
        foreach(var a in randomList){
            sum += a.Item2;
            if(random < sum){
                animator.Play("idle" + a.Item1);
                break;
            }
        }

        time = 0;
    }

    // // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    // override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    // {
       
    //     Debug.Log("OnStateExit");
    // }

    // override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    // {
    //     // Implement code that processes and affects root motion
    // }

    // // OnStateIK is called right after Animator.OnAnimatorIK()
    // override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    // {
    //    // Implement code that sets up animation IK (inverse kinematics)
    //     Debug.Log("OnStateIK");
    // }

    private void _delayRandom(){
        
    }
}
