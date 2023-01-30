using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTreeUtility
{
    public class Inverter : Node
    {
        public Inverter(string n)
        {
            name = n;
        }

        public override Status Process()
        {
            var s = children[currentChild].Process();
            switch (s)
            {
                case Status.SUCCESS:
                    return Status.FAILURE;
                case Status.RUNNING:
                    return Status.RUNNING;
                case Status.FAILURE:
                    return Status.SUCCESS;
                default:
                    return Status.FAILURE;
            }

        }
    }
}
