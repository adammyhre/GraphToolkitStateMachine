using System;
using Unity.GraphToolkit.Editor;

namespace StateMachine.Editor {
    [Serializable]
    internal class StartNode : StateMachineNode {
        protected override void OnDefinePorts(IPortDefinitionContext context) {
            context.AddOutputPort(EXECUTION_PORT_DEFAULT_NAME)
                .WithDisplayName(string.Empty)
                .WithConnectorUI(PortConnectorUI.Arrowhead)
                .Build();
        }
    }
} 