using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackStart : StateMachineBehaviour
{
    private Transform transform;
    private Vector2 inputDirection;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {   
        // player can switch its facing direction when attempting attack at the very beginning 
        transform = animator.GetComponent<Transform>();
        inputDirection = animator.GetComponent<PlayerController>().InputDirection();
        int temp = (int)transform.localScale.x;
        int faceDir = inputDirection.x < 0 ? -1
            : inputDirection.x > 0 ? 1 : temp;
        transform.localScale = new Vector3(faceDir, transform.localScale.y, transform.localScale.z);
        animator.GetComponent<PlayerController>().isAttack = true;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    // override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    // {
        
    // }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
