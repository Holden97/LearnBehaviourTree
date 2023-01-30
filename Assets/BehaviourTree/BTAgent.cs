using Math.Sort;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static BehaviourTreeUtility.RobberBehaviour;

namespace BehaviourTreeUtility
{
    public class BTAgent : MonoBehaviour
    {
        protected BehaviourTree tree;
        protected NavMeshAgent agent;


        protected virtual void CreteTree(BehaviourTree tree)
        {

        }

        protected ActionState state = ActionState.IDLE;
        protected Node.Status treeStatus = Node.Status.RUNNING;

        private void Start()
        {
            agent = GetComponent<NavMeshAgent>();
            tree = new BehaviourTree();
            CreteTree(tree);
            tree.PrintTree();
            tree.Process();
        }

        protected Node.Status GoToLocation(Vector3 destination)
        {
            float distanceToTarget = Vector3.Distance(destination, transform.position);
            if (state == ActionState.IDLE)
            {
                agent.SetDestination(destination);
                state = ActionState.WORKING;
            }
            //pathEndPosition是SetDestination设置的GameObject的位置，而destination是agent最终要走到的目标点
            //以此例来说，pathEndPosition是门正中央的位置，而destination是门下方人物可以到达的NavMesh上的某一点
            else if (Vector3.Distance(agent.pathEndPosition, destination) >= 2)
            {
                state = ActionState.IDLE;
                return Node.Status.FAILURE;
            }
            else if (distanceToTarget < 2)
            {
                state = ActionState.IDLE;
                return Node.Status.SUCCESS;
            }
            return Node.Status.RUNNING;
        }

        protected void Update()
        {
            treeStatus = tree.Process();

        }
    }
}
