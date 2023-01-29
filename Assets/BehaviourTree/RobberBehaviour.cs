﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace BehaviourTreeUtility
{
    public class RobberBehaviour : MonoBehaviour
    {
        BehaviourTree tree;
        public GameObject diamond;
        public GameObject door;
        public GameObject van;
        NavMeshAgent agent;

        public enum ActionState
        {
            IDLE,
            WORKING
        }

        public ActionState state = ActionState.IDLE;
        Node.Status treeStatus = Node.Status.RUNNING;

        private void Start()
        {
            agent = GetComponent<NavMeshAgent>();
            tree = new BehaviourTree();

            Sequence steal = new Sequence("Steal Something");
            Leaf goToDiamond = new Leaf("Go To Diamond", GoToDiamond);
            Leaf goToDoor = new Leaf("Go To Door", GoToDoor);
            Leaf goToVan = new Leaf("Go To Van", GoToVan);

            steal.AddChild(goToDiamond);
            steal.AddChild(goToDoor);
            steal.AddChild(goToVan);

            tree.AddChild(steal);

            tree.PrintTree();
            tree.Process();

        }

        public Node.Status GoToDiamond()
        {
            return GoToLocation(diamond.transform.position);
        }

        public Node.Status GoToDoor()
        {
            return GoToLocation(door.transform.position);
        }

        public Node.Status GoToVan()
        {
            return GoToLocation(van.transform.position);
        }

        Node.Status GoToLocation(Vector3 destination)
        {
            float distanceToTarget = Vector3.Distance(destination, transform.position);
            if (state == ActionState.IDLE)
            {
                agent.SetDestination(destination);
                state = ActionState.WORKING;
            }
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

        private void Update()
        {
            if (treeStatus == Node.Status.RUNNING)
            {
                treeStatus = tree.Process();
            }
        }
    }
}