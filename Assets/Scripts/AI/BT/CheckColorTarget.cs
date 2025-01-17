using AI;
using AI.BT;
using BehaviourTree;
using Managers;
using Player;

public class CheckColorTarget : Node
{
    private EnemyShield owner;

    private void SelectTarget()
    {
        PlayerController[] players = PlayerManager.Players.ToArray();
        PlayerController target = (players[0].PColor != owner.GetShieldColor()) ? players[0] : players[1];
        SetDataInBlackboard("Target", target);
        SetDataInBlackboard("WaitTime", owner.data.delaySwitchTarget);
        GetData<TaskWaitForSeconds>("WaitNode").FinalCountdown = null;
    }

    public override NodeState Evaluate(Node root)
    {
        owner = GetData<EnemyShield>("caster");
        PlayerColor color = owner.GetShieldColor();
        if (color == PlayerColor.None) return NodeState.Success;

        PlayerController target = GetData<PlayerController>("Target");
        if (target != null && target.Color.PColor == color)
            SelectTarget();
        /*else
            SelectTarget();*/
        return NodeState.Failure;
    }
}