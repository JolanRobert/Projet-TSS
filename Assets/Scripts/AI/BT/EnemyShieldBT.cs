using System;
using System.Collections;
using System.Collections.Generic;
using AI;
using AI.BT;
using BehaviourTree;
using UnityEngine;
using UnityEngine.AI;
using Tree = BehaviourTree.Tree;

public class EnemyShieldBT : Tree
{
    [SerializeField] private EnemyShield enemy;
    [SerializeField] private NavMeshAgent agent;

    protected override Node InitTree()
    {
        origin = new Selector(
            new InitEnemyBlackboard(enemy),
            new TaskWaitForSeconds(),
            new Sequence(
                new CheckPlayerInVision(),
                new TaskPatrol()
            ),
            new CheckColorTarget(),
            new Sequence(
                new CanAttack(enemy.transform),
                new AttackProjectile()
            ),
            new MoveToTarget(agent)
        );
        return origin;
    }
}
