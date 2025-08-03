using System;
using System.Collections.Generic;
using System.Linq;
using Unity.GraphToolkit.Editor;
using UnityEditor;

namespace StateMachine.Editor {
    [Serializable]
    [Graph(AssetExtension)] 
    internal class StateMachineDirectorGraph : Graph {
        internal const string AssetExtension = "stmch";

        [MenuItem("Assets/Create/State Machine/State Machine Director Graph")]
        static void CreateAssetFile() {
            GraphDatabase.PromptInProjectBrowserToCreateNewAsset<StateMachineDirectorGraph>("State Machine Graph");
        }

        public override void OnGraphChanged(GraphLogger infos) {
            base.OnGraphChanged(infos);
            // TODO Add error checking / validation
        }
    }
}