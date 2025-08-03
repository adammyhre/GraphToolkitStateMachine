using System;

namespace StateMachine.Runtime {
    [Serializable]
    public class TransitionRuntimeNode : RuntimeNode {
        public StateCondition Condition;
    }
}