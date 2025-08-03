namespace StateMachine.Runtime {
    public interface IStateMachineNodeExecutor<in TNode> where TNode : RuntimeNode {
        bool Execute(TNode node, StateMachineDirector ctx);
    }
}