using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class AIController : MonoBehaviour
{
    public StateMachine StateMachine {get; private set;}
    public NavMeshAgent Agent {get; private set;}
    public Animator Animator {get; private set; } // Not needed since we're not using animations
    public Transform[] Waypoints;
    public Transform Player;
    public float SightRange = 10f;
    public float AttackRange = 2f;
    public LayerMask PlayerLayer;
    public StateType currentState;

    // Toggle this in the Inspector or via Visual Scripting to enable/disable a Raycast line-of-sight check
    public bool UseLineOfSight = false;

    private bool canSeePlayerVS;

    void Start()
    {
        Agent = GetComponent<NavMeshAgent>();
        Animator = GetComponent<Animator>(); 
    
        StateMachine = new StateMachine();
        StateMachine.AddState(new IdleState(this));
        StateMachine.AddState(new PatrolState(this));
        StateMachine.AddState(new ChaseState(this));
        StateMachine.AddState(new AttackState(this));  // Add the new AttackState

        StateMachine.TransitionToState(StateType.Idle);
    }

    void Update()
    {
        StateMachine.Update();
        Animator.SetFloat("CharacterSpeed", Agent.velocity.magnitude);

        canSeePlayerVS = CanSeePlayer();
        Variables.Object(gameObject).Set("canSeePlayerVS", canSeePlayerVS);
        currentState = StateMachine.GetCurrentStateType();
    }

    public bool CanSeePlayer(){
        float distanceToPlayer = Vector3.Distance(transform.position, Player.position);

        if(distanceToPlayer <= SightRange){
            // Optionally, add line of sight checks here using Raycast
            if(UseLineOfSight){
                Vector3 origin = transform.position + Vector3.up;
                Vector3 direction = (Player.position + Vector3.up) - origin;
                RaycastHit hit;

                if(Physics.Raycast(origin, direction, out hit, SightRange, PlayerLayer)){
                    if(hit.transform == Player){
                        return true;
                    }
                }

                return false;
            } else{
                return true;
            }
            
        }
        return false;
    }

    // New method to check if the AI is within attack range
    public bool IsPlayerInAttackRange(){
        float distanceToPlayer = Vector3.Distance(transform.position, Player.position);

        return distanceToPlayer <= AttackRange;
    }
}
