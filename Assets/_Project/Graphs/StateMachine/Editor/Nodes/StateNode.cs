using System;
using Unity.GraphToolkit.Editor;

namespace StateMachine.Editor {
    [Serializable]
    internal class StateNode : StateMachineNode {
        const int MAX_TRANSITIONS = 2;
        const string TRANSITION_PORT_PREFIX = "Transition";

        protected override void OnDefineOptions(INodeOptionDefinition context) {
            context.AddNodeOption(
                optionName: "stateName",
                dataType: typeof(string),
                optionDisplayName: "State Name",
                tooltip: "Name of this state",
                defaultValue: "New State"
            );
        }

        protected override void OnDefinePorts(IPortDefinitionContext context) {
            context.AddInputPort(EXECUTION_PORT_DEFAULT_NAME)
                .WithDisplayName(string.Empty)
                .WithConnectorUI(PortConnectorUI.Arrowhead)
                .Build();

            for (int i = 0; i < MAX_TRANSITIONS; i++) {
                context.AddOutputPort($"{TRANSITION_PORT_PREFIX}{i}")
                    .WithDisplayName($"Transition {i + 1}")
                    .WithConnectorUI(PortConnectorUI.Arrowhead)
                    .Build();
            }
        }
    }
}