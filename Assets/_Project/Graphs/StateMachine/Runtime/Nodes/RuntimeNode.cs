using System;
using System.Collections.Generic;

namespace StateMachine.Runtime {
    [Serializable]
    public abstract class RuntimeNode {
        public List<int> NextNodeIndices = new();
    }
}