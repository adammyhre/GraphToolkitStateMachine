using UnityEngine;

namespace StateMachine.Runtime {
    public class StateNodeExecutor : IStateMachineNodeExecutor<StateRuntimeNode> {
        public bool Execute(StateRuntimeNode node, StateMachineDirector ctx) {
            Debug.Log($"Entering state: {node.StateName}");
            return true;
        }
    }
}