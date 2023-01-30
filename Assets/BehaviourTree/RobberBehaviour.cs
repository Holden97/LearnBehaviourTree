using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace BehaviourTreeUtility
{
    public class RobberBehaviour : MonoBehaviour
    {
        BehaviourTree tree;
        public GameObject diamondPlinth;
        public GameObject backDoor;
        public GameObject frontDoor;
        public GameObject van;

        [Range(0, 1000)] public int money = 800;
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
            Selector gotoDoor = new Selector("Go To Door");
            Leaf goToBackDoor = new Leaf("Go To Back Door", GoToBackDoor);
            Leaf hasMoney = new Leaf("Has Enough Money", HasEnoughMoney);
            Leaf goToFrontDoor = new Leaf("Go To Front Door", GoToFrontDoor);
            Leaf stealTheDiamond = new Leaf("steal the diamond", StealTheDiamond);
            gotoDoor.AddChild(goToFrontDoor);
            gotoDoor.AddChild(goToBackDoor);
            Leaf goToDiamond = new Leaf("Go To Diamond", GoToDiamond);
            Leaf goToVan = new Leaf("Go To Van", GoToVan);
            Inverter invertMoney = new Inverter("Invert Money");
            invertMoney.AddChild(hasMoney);
            steal.AddChild(invertMoney);
            steal.AddChild(gotoDoor);
            steal.AddChild(goToDiamond);
            steal.AddChild(stealTheDiamond);
            steal.AddChild(goToVan);

            tree.AddChild(steal);

            tree.PrintTree();
            tree.Process();

        }

        public Node.Status GoToDiamond()
        {
            return GoToLocation(diamondPlinth.transform.position);
        }

        public Node.Status HasEnoughMoney()
        {
            if (money > 500)
            {
                return Node.Status.SUCCESS;
            }
            else
            {
                return Node.Status.FAILURE;
            }
        }

        public Node.Status GoToBackDoor()
        {
            return GoToDoor(backDoor);
        }

        public Node.Status StealTheDiamond()
        {
            if (diamondPlinth.GetComponentInChildren<Holder>().hasDiamond)
            {
                diamondPlinth.GetComponentInChildren<Diamond>().transform.SetParent(transform);
                return Node.Status.SUCCESS;
            }
            else
            {
                return Node.Status.FAILURE;
            }
        }

        public Node.Status GoToFrontDoor()
        {
            return GoToDoor(frontDoor);
        }

        public Node.Status GoToVan()
        {
            var s = GoToLocation(van.transform.position);
            if (s == Node.Status.SUCCESS)
            {
                money += 50;
                Destroy(GetComponentInChildren<Diamond>().gameObject);
                return Node.Status.SUCCESS;
            }
            else
            {
                return s;
            }
        }

        public Node.Status GoToDoor(GameObject door)
        {
            Node.Status s = GoToLocation(door.transform.position);
            if (s == Node.Status.SUCCESS)
            {
                if (!door.GetComponent<Lock>().isLocked)
                {
                    door.SetActive(false);
                    return Node.Status.SUCCESS;
                }
                else
                {
                    return Node.Status.FAILURE;
                }
            }
            else
            {
                return s;
            }
        }

        Node.Status GoToLocation(Vector3 destination)
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

        private void Update()
        {
            treeStatus = tree.Process();
        }
    }
}
