using System;
using StateMachine.Runtime;
using Unity.GraphToolkit.Editor;

namespace StateMachine.Editor {
    [Serializable]
    internal class TransitionNode : StateMachineNode {
        StateCondition condition;
        
        const string CONDITION_PORT = "condition";

        protected override void OnDefinePorts(IPortDefinitionContext context) {
            context.AddInputPort(EXECUTION_PORT_DEFAULT_NAME)
                .WithDisplayName(string.Empty)
                .WithConnectorUI(PortConnectorUI.Arrowhead)
                .Build();
            
            context.AddOutputPort(EXECUTION_PORT_DEFAULT_NAME)
                .WithDisplayName(string.Empty)
                .WithConnectorUI(PortConnectorUI.Arrowhead)
                .Build();
            
            context.AddInputPort<StateCondition>(CONDITION_PORT)
                .WithDisplayName("Condition")
                .WithDefaultValue(StateCondition.IsWalking)
                .Build();
        }
    }
}