using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Patterns.Observer;

public class AIBehaviourPatrolling : AIBehaviour
{

    public float reachDistance = 2f;
    public Waypoint[] wayPoints;

    private float lastStoppingDistance = 0f;
    private float lastSpeed = 0f;
    private int currentWaypoint = 0;

    public override void BehaviourStart()
    {
        if (wayPoints.Length == 0 || wayPoints == null)
        {
            owner.Notify(Message.AI_ReachedWaypoint);
            return;
        }

        owner.SetMovementControl(AICharacter.MovementControlType.NavAgent);

        lastStoppingDistance = owner.navAgent.stoppingDistance;
        lastSpeed = owner.navAgent.speed;

        owner.navAgent.stoppingDistance = reachDistance;
        owner.CurrentMoveSpeed = owner.walkMoveSpeed;
        owner.navAgent.SetDestination(wayPoints[GetNextWayPointId()].transform.position);
    }

    public override void BehaviourUpdate()
    {
        if (owner.navAgent.remainingDistance < reachDistance)
        {
            owner.Notify(Message.AI_ReachedWaypoint);
            owner.navAgent.stoppingDistance = lastStoppingDistance;
            owner.CurrentMoveSpeed = lastSpeed;
        }
    }

    public override void BehaviourEnd()
    {
        owner.navAgent.stoppingDistance = lastStoppingDistance;
        owner.CurrentMoveSpeed = lastSpeed;
    }

    private int GetNextWayPointId()
    {
        if (wayPoints.Length == 0)
            return 0;
        else
            currentWaypoint = (currentWaypoint < wayPoints.Length) ? currentWaypoint + 1 : 1;
        return currentWaypoint - 1;
    }

}
