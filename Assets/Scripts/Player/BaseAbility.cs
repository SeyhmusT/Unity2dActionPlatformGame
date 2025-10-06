using UnityEngine;

public class BaseAbility : MonoBehaviour
{
    protected Player player;
    public PlayerStates.State thisAbilityState;
    public bool isPermitted = true;
    protected GatherInput linkedInput;
    protected PhysicsControl linkedPhysics;
    protected StateMachine linkedStateMachine;
    protected Animator linkedAnimator;
    [SerializeField] protected AudioSource source;
    [SerializeField] protected AudioClip clip;

    protected virtual void Start()
    {
        Initialization();

    }
    public virtual void EnterAbility()
    {

    }
    public virtual void ExitAbility()
    {

    }
    public virtual void ProcessAbility()
    {

    }

    public virtual void ProcessFixedAbility()
    {

    }
    public virtual void UpdateAnimator()
    {

    }

    protected virtual void Initialization()
    {
        player = GetComponent<Player>();
        if (player != null)
        {
            linkedInput = player.gatherInput;
            linkedStateMachine = player.stateMachine;
            linkedAnimator = player.anim;
            linkedPhysics = player.physicsControl;
        }
    }


}
