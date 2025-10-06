public class StateMachine
{
    public PlayerStates.State previousState;
    public PlayerStates.State currentState;
    public BaseAbility[] arrayOfAbilities;

    public void ChangeState(PlayerStates.State newState)
    {
         foreach (BaseAbility ability in arrayOfAbilities)
        {
            if (ability.thisAbilityState == newState)
            {
                if (!ability.isPermitted)
                {
                    return;
                }
                
            }
        }

        foreach (BaseAbility ability in arrayOfAbilities)
        {
            if (ability.thisAbilityState == currentState)
            {
                ability.ExitAbility();
                previousState = currentState;
            }
        }

        foreach (BaseAbility ability in arrayOfAbilities)
        {
            if (ability.thisAbilityState == newState)
            {
                if (ability.isPermitted)
                {
                    currentState = newState;
                    ability.EnterAbility();
                }
                break;
            }
        }
    }


    public void ForceChange(PlayerStates.State newState)
    {
        previousState = currentState;
        currentState = newState;
    }
}
