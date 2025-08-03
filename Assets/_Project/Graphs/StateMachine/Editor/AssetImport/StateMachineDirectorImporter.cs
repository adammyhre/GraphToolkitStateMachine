#region Imports
using System;
using System.Collections.Generic;
using System.Linq;
using StateMachine.Editor;
using StateMachine.Runtime;
using Unity.GraphToolkit.Editor;
using UnityEditor.AssetImporters;
using UnityEngine;
#endregion

namespace StateMachine.Editor {
    [ScriptedImporter(1, StateMachineDirectorGraph.AssetExtension)]
    internal class StateMachineDirectorImporter : ScriptedImporter {
        public override void OnImportAsset(AssetImportContext ctx) {
            var graph = GraphDatabase.LoadGraphForImporter<StateMachineDirectorGraph>(ctx.assetPath);
            
            if (graph == null) {
                Debug.LogError($"Failed to load State Machine Director graph asset: {ctx.assetPath}");
                return;
            }
            
            var startNodeModel = graph.GetNodes().OfType<StartNode>().FirstOrDefault();
            
            if (startNodeModel == null) {
                return;
            }
            
            var runtimeAsset = ScriptableObject.CreateInstance<StateMachineRuntimeGraph>();
            var nodeMap = new Dictionary<INode, int>();
            
            // First pass: Create all runtime nodes (without connections)
            CreateRuntimeNodes(startNodeModel, runtimeAsset, nodeMap);
            
            // Second pass: Set up connections using the indices
            SetupConnections(startNodeModel, runtimeAsset, nodeMap);
            
            ctx.AddObjectToAsset("RuntimeAsset", runtimeAsset);
            ctx.SetMainObject(runtimeAsset);
        }

        void CreateRuntimeNodes(INode startNode, StateMachineRuntimeGraph runtimeGraph, Dictionary<INode, int> nodeMap) {
            var nodesToProcess = new Queue<INode>();
            nodesToProcess.Enqueue(startNode);

            while (nodesToProcess.Count > 0) {
                var currentNode = nodesToProcess.Dequeue();
                
                if (nodeMap.ContainsKey(currentNode)) continue;
                
                var runtimeNodes = TranslateNodeModelToRuntimeNodes(currentNode);

                foreach (var runtimeNode in runtimeNodes) {
                    nodeMap[currentNode] = runtimeGraph.Nodes.Count;
                    runtimeGraph.Nodes.Add(runtimeNode);
                }
                
                // Queue up all connected nodes
                for (int i = 0; i < currentNode.outputPortCount; i++) {
                    var port = currentNode.GetOutputPort(i);

                    if (port.isConnected) {
                        nodesToProcess.Enqueue(port.firstConnectedPort.GetNode());
                    }
                }
            }
        }

        void SetupConnections(INode startNode, StateMachineRuntimeGraph runtimeGraph, Dictionary<INode, int> nodeMap) {
            foreach (var kvp in nodeMap) {
                var editorNode = kvp.Key;
                var runtimeIndex = kvp.Value;
                var runtimeNode = runtimeGraph.Nodes[runtimeIndex];

                for (int i = 0; i < editorNode.outputPortCount; i++) {
                    var port = editorNode.GetOutputPort(i);

                    if (port.isConnected && nodeMap.TryGetValue(port.firstConnectedPort.GetNode(), out int nextIndex)) {
                        runtimeNode.NextNodeIndices.Add(nextIndex);
                    }
                }
            }
        }

        static List<RuntimeNode> TranslateNodeModelToRuntimeNodes(INode nodeModel) {
            var returnedNodes = new List<RuntimeNode>();

            switch (nodeModel) {
                case StartNode:
                    returnedNodes.Add(new StartRuntimeNode());
                    break;
                case StateNode stateNode:
                    string stateName = "New State";
                    stateNode.GetNodeOptionByName("stateName")?.TryGetValue(out stateName);
                    returnedNodes.Add(
                        new StateRuntimeNode
                        {
                            StateName = stateName
                        });
                    break;
                case TransitionNode transitionNode:
                    var conditionPort = transitionNode.GetInputPortByName("condition");
                    var condition = GetInputPortValue<StateCondition>(conditionPort);
                    returnedNodes.Add(
                        new TransitionRuntimeNode
                        {
                            Condition = condition
                        });
                    break;
                default:
                    throw new ArgumentException($"Unsupported node type: {nodeModel.GetType()}");
            }
            
            return returnedNodes;
        }

        static T GetInputPortValue<T>(IPort port) {
            T value = default;

            if (port.isConnected) {
                switch (port.firstConnectedPort.GetNode()) {
                    case IVariableNode variableNode:
                        variableNode.variable.TryGetDefaultValue<T>(out value);
                        return value;
                    case IConstantNode constantNode:
                        constantNode.TryGetValue<T>(out value);
                        return value;
                    default:
                        break;
                }
            }
            else {
                port.TryGetValue(out value);
            }
            
            return value;
        }
    }
}