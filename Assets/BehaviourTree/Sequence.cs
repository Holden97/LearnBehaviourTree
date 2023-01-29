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
            if (childStatus == Status.RUNNING || childStatus == Status.FAILURE)
            {
                return childStatus;
            }
            else
            {
                currentChild++;
            }

            if (currentChild >= children.Count)
            {
                currentChild = 0;
                return Status.SUCCESS;
            }
            else
            {
                return Status.RUNNING;
            }
        }
    }
}
