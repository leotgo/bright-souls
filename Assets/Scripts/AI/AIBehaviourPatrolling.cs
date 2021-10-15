using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Patterns.Observer;

namespace BrightSouls.AI
{
    public class AIBehaviourPatrolling : AIBehaviour
    {
        /* ------------------------ Inspector-Assigned Fields ----------------------- */

        [SerializeField] private float reachDistance = 2f;
        [SerializeField] private Waypoint[] wayPoints;

        /* ------------------------------ Runtime Data ------------------------------ */

        // TODO Assign/retrieve this data from state machine controller
        private float lastStoppingDistance = 0f;
        private float lastSpeed = 0f;
        private int currentWaypoint = 0;

        /* -------------------------- State Machine Events -------------------------- */

        public override void OnBehaviourStart(AICharacter agent)
        {
            if (wayPoints.Length == 0 || wayPoints == null)
            {
                agent.Notify(Message.AI_ReachedWaypoint);
                return;
            }

            agent.SetMovementControl(AICharacter.AIMovementControlType.NavAgent);

            lastStoppingDistance = agent.NavAgent.stoppingDistance;
            lastSpeed = agent.NavAgent.speed;

            agent.NavAgent.stoppingDistance = reachDistance;
            agent.CurrentMoveSpeed = agent.WalkMoveSpeed;
            agent.NavAgent.SetDestination(wayPoints[GetNextWayPointId()].transform.position);
        }

        public override void OnBehaviourUpdate(AICharacter agent)
        {
            if (agent.NavAgent.remainingDistance < reachDistance)
            {
                agent.Notify(Message.AI_ReachedWaypoint);
                agent.NavAgent.stoppingDistance = lastStoppingDistance;
                agent.CurrentMoveSpeed = lastSpeed;
            }
        }

        public override void OnBehaviourEnd(AICharacter agent)
        {
            agent.NavAgent.stoppingDistance = lastStoppingDistance;
            agent.CurrentMoveSpeed = lastSpeed;
        }

        /* --------------------------------- Helpers -------------------------------- */

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

        /* -------------------------------------------------------------------------- */
    }
}