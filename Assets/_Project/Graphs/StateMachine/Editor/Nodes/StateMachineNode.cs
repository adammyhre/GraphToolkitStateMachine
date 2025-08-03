using System;
using Unity.GraphToolkit.Editor;

namespace StateMachine.Editor {
    [Serializable]
    internal abstract class StateMachineNode : Node {
        public const string EXECUTION_PORT_DEFAULT_NAME = "ExecutionPort";
    }
}