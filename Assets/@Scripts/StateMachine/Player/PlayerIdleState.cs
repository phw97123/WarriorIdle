public class PlayerIdleState : PlayerBaseState
{
    public PlayerIdleState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
    {
    }

    public override void Enter()
    {
        StartAnimation(stateMachine.Player.AnimationData.IdleParameterHash); 
    }

    public override void Update()
    {
        if (stateMachine.Player.isDead)
            return;
       
        base.Update();
    }

    public override void Exit() 
    { 
        StopAnimation(stateMachine.Player.AnimationData.IdleParameterHash); 
    }
}