using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Patterns.Observer;

namespace BrightSouls.AI
{
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

            owner.SetMovementControl(AICharacter.AIMovementControlType.NavAgent);

            lastStoppingDistance = owner.NavAgent.stoppingDistance;
            lastSpeed = owner.NavAgent.speed;

            owner.NavAgent.stoppingDistance = reachDistance;
            owner.CurrentMoveSpeed = owner.WalkMoveSpeed;
            owner.NavAgent.SetDestination(wayPoints[GetNextWayPointId()].transform.position);
        }

        public override void BehaviourUpdate()
        {
            if (owner.NavAgent.remainingDistance < reachDistance)
            {
                owner.Notify(Message.AI_ReachedWaypoint);
                owner.NavAgent.stoppingDistance = lastStoppingDistance;
                owner.CurrentMoveSpeed = lastSpeed;
            }
        }

        public override void BehaviourEnd()
        {
            owner.NavAgent.stoppingDistance = lastStoppingDistance;
            owner.CurrentMoveSpeed = lastSpeed;
        }

        private int GetNextWayPointId()
        {
            if (wayPoints.Length == 0)
            {
                return 0;
            }
            else
            {
                if(currentWaypoint < wayPoints.Length)
                {
                    currentWaypoint++;
                }
                else
                {
                    currentWaypoint = 1;
                }
            }
            return currentWaypoint - 1;
        }
    }
}