using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AttackState : IState
{
    private AIController aIController;

    public StateType Type => StateType.Attack;

    public AttackState(AIController aIController)
    {
        this.aIController = aIController;
    }

    public void Enter()
    {
        aIController.Animator.SetBool("isAttacking", true);
        aIController.Agent.isStopped = true;
    }

    public void Execute()
    {
        if (Vector3.Distance(aIController.transform.position, aIController.Player.position) > aIController.AttackRange)
        {
            aIController.StateMachine.TransitionToState(StateType.Chase);
            return;
        }
    }

    public void Exit()
    {
        aIController.Animator.SetBool("isAttacking", true);
        aIController.Agent.isStopped = false;
    }


}