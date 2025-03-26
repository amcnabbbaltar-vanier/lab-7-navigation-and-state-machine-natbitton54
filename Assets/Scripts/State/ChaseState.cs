public class ChaseState : IState
{
    private AIController aiController;
    public StateType Type => StateType.Chase;
    public ChaseState(AIController aiController)
    {
        this.aiController = aiController;
    }

    public void Enter()
    {
        aiController.Animator.SetBool("IsChasing", true);
    }

    public void Execute()
    {
        if (!aiController.CanSeePlayer())
        {
            aiController.StateMachine.TransitionToState(StateType.Patrol);
            return;
        }
        if (aiController.IsPlayerInAttackRange())
        {
            aiController.StateMachine.TransitionToState(StateType.Attack);
            return;
        }


        aiController.Agent.destination = aiController.Player.position;
     }

    public void Exit()
    {
    }
}
