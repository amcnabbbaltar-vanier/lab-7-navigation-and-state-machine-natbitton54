using System;
using System.Net.Sockets;

public class PatrolState : IState
{
    private AIController aiController;
    private int currentWayPointIndex = 0;
    public StateType Type => StateType.Patrol;

    public PatrolState(AIController aiController)
    {
        this.aiController = aiController;
    }

    public void Enter()
    {
        //aiController.Animator.SetBool("isMoving", true);
        MoveToNextWayPoint();
    }


    public void Execute()
    {
        if (aiController.CanSeePlayer())
        {
            aiController.StateMachine.TransitionToState(StateType.Chase);
            return;
        }

        if (!aiController.Agent.pathPending && aiController.Agent.remainingDistance <= aiController.Agent.stoppingDistance)
        {
            MoveToNextWayPoint();
        }
    }

    public void Exit()
    {

    }

    private void MoveToNextWayPoint()
    {
        if (aiController.Waypoints.Length == 0)
            return;

        aiController.Agent.destination = aiController.Waypoints[currentWayPointIndex].position;
        currentWayPointIndex = (currentWayPointIndex + 1) % aiController.Waypoints.Length;
    }
}
