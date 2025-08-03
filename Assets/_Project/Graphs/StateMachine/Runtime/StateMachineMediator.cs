using UnityEngine;

namespace StateMachine.Runtime {
    public enum StateCondition { IsWalking, IsJumping }
    
    public class StateMachineMediator : MonoBehaviour {
        [Header("Conditions")]
        public bool isWalking;
        public bool isJumping;

        public bool EvaluateCondition(StateCondition condition) {
            switch (condition) {
                case StateCondition.IsWalking:
                    return isWalking;
                case StateCondition.IsJumping:
                    return isJumping;
                default:
                    Debug.LogError($"Unhandled condition: {condition}");
                    return false;
            }
        }
    }
}