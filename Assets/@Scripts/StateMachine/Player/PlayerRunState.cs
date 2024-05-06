public class PlayerRunState : PlayerBaseState
{
    public PlayerRunState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
    {
    }

    public override void Enter()
    {
        StartAnimation(stateMachine.Player.AnimationData.RunParameterHash); 
    }

    public override void Update()
    {
        base.Update();
        stateMachine.Player.Move(nearestEnemy,stateMachine.Player.PlayerData.speed); 
    }

    public override void Exit()
    {
        StopAnimation(stateMachine.Player.AnimationData.RunParameterHash); 
    }
}