using UnityEngine;

public class AnimationData
{
    private string _idleParameterName = "Idle";
    private string _runParameterName = "Run";
    private string _attackParameterName = "Attack";
    private string _deadParameterName = "Dead"; 

    public int IdleParameterHash { get; private set; }
    public int RunParameterHash { get; private set; }
    public int AttackParameterHash { get; private set; }
    public int DeadParameterHash { get; private set; }

    public AnimationData()
    {
        IdleParameterHash = Animator.StringToHash(_idleParameterName);
        RunParameterHash = Animator.StringToHash(_runParameterName);
        AttackParameterHash = Animator.StringToHash(_attackParameterName);
        DeadParameterHash = Animator.StringToHash(_deadParameterName);
    }
}