using UnityEngine;

namespace StateMachine.Runtime {
    public class StartNodeExecutor : IStateMachineNodeExecutor<StartRuntimeNode> {
        public bool Execute(StartRuntimeNode node, StateMachineDirector ctx) {
            Debug.Log("Starting State Machine");
            return true;
        }
    }
}