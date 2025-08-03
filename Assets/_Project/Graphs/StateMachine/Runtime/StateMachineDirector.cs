using System.Collections.Generic;
using UnityEngine;

namespace StateMachine.Runtime {
    public class StateMachineDirector : MonoBehaviour {
        [Header("Graph")]
        public StateMachineRuntimeGraph RuntimeGraph;
        
        Dictionary<System.Type, object> executors;
        private RuntimeNode currentNode;

        void Awake() {
            executors = new Dictionary<System.Type, object> {
                { typeof(StartRuntimeNode), new StartNodeExecutor() },
                { typeof(StateRuntimeNode), new StateNodeExecutor() },
                { typeof(TransitionRuntimeNode), new TransitionNodeExecutor() }
            };
        }

        void Start() {
            if (RuntimeGraph == null) {
                Debug.LogError("No runtime graph assigned!");
                return;
            }
            
            currentNode = RuntimeGraph.Nodes[0];
        }

        void Update() {
            if (currentNode == null) return;

            if (!executors.TryGetValue(currentNode.GetType(), out var executor)) {
                Debug.LogError($"No executor found for node type: {currentNode.GetType()}");
                currentNode = null;
                return;
            }

            if (currentNode is StateRuntimeNode stateNode) {
                var stateExecutor = (IStateMachineNodeExecutor<StateRuntimeNode>)executor;
                stateExecutor.Execute(stateNode, this);

                foreach (var nextIndex in stateNode.NextNodeIndices) {
                    var nextNode = RuntimeGraph.Nodes[nextIndex];

                    if (nextNode is TransitionRuntimeNode transitionNode) {
                        var transitionExecutor = (IStateMachineNodeExecutor<TransitionRuntimeNode>)executors[typeof(TransitionRuntimeNode)];

                        if (transitionExecutor.Execute(transitionNode, this) && transitionNode.NextNodeIndices.Count > 0) {
                            currentNode = RuntimeGraph.Nodes[transitionNode.NextNodeIndices[0]];
                            return;
                        }
                    }
                }
            }
            else if (currentNode is TransitionRuntimeNode transitionNode) {
                var transitionExecutor = (IStateMachineNodeExecutor<TransitionRuntimeNode>)executor;

                if (transitionExecutor.Execute(transitionNode, this) && transitionNode.NextNodeIndices.Count > 0) {
                    currentNode = RuntimeGraph.Nodes[transitionNode.NextNodeIndices[0]];
                }
                else {
                    currentNode = null;
                }
            }
            else if (currentNode is StartRuntimeNode startNode) {
                var startExecutor = (IStateMachineNodeExecutor<StartRuntimeNode>)executor;
                startExecutor.Execute(startNode, this);
                currentNode = startNode.NextNodeIndices.Count > 0
                    ? RuntimeGraph.Nodes[startNode.NextNodeIndices[0]]
                    : null;
            }
        }
    }
}