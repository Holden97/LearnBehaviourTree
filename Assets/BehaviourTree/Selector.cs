using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTreeUtility
{
    public class Selector : Node
    {
        public Selector() { }
        public Selector(string name) : base(name) { }

        public override Status Process()
        {
            Status childStatus = children[currentChild].Process();

            switch (childStatus)
            {
                case Status.SUCCESS:
                    {
                        currentChild = 0;
                        return Status.SUCCESS;
                    }
                case Status.RUNNING:
                    return Status.RUNNING;
                case Status.FAILURE:
                    currentChild++;
                    if (currentChild >= children.Count)
                    {
                        currentChild = 0;
                        return Status.FAILURE;
                    }
                    else
                    {
                        return Status.RUNNING;
                    }
                default:
                    return Status.FAILURE;
            }
        }
    }
}
