using System.Collections.Generic;
using UnityEngine;

namespace StateMachine.Runtime {
    public class StateMachineRuntimeGraph : ScriptableObject {
        [SerializeReference]
        public List<RuntimeNode> Nodes = new();
    }
}