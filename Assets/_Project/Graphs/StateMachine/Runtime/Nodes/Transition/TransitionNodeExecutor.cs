using UnityEngine;

namespace StateMachine.Runtime {
    public class TransitionNodeExecutor : IStateMachineNodeExecutor<TransitionRuntimeNode> {
        public bool Execute(TransitionRuntimeNode node, StateMachineDirector ctx) {
            var mediator = ctx.GetComponent<StateMachineMediator>();

            if (mediator == null) {
                Debug.LogError("No StateMachineMediator found on StateMachineDirector!");
                return false;
            }
            
            bool condition = mediator.EvaluateCondition(node.Condition);
            Debug.Log($"Evaluating transition condition: {node.Condition} = {condition}");
            return condition;
        }
    }
}