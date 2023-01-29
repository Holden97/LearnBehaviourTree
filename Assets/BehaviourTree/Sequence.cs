using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTreeUtility
{
    public class Sequence : Node
    {
        public Sequence() { }
        public Sequence(string name) : base(name)
        {

        }

        public override Status Process()
        {
            Status childStatus = children[currentChild].Process();
            if (childStatus == Status.RUNNING)
            {
                return Status.RUNNING;
            }
            if (childStatus == Status.FAILURE)
            {
                return childStatus;
            }

            currentChild++;
            if (currentChild >= children.Count)
            {
                currentChild = 0;
                return Status.SUCCESS;
            }

            return Status.RUNNING;
        }
    }
}
