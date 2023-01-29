﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTreeUtility
{
    public class Leaf : Node
    {
        public delegate Status Tick();
        public Tick ProcessMethod;

        public override Status Process()
        {
            if (ProcessMethod != null)
                return ProcessMethod();
            return Status.FAILURE;
        }

        public Leaf() { }

        public Leaf(string n, Tick pm)
        {
            name = n;
            ProcessMethod = pm;
        }
    }
}
