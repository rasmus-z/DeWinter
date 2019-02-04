using UFlow;
using System;

namespace Ambition
{
    public class DelegateState : UState<Action>
    {
        private Action _action;

        public override void SetData(Action action) => _action = action;
        public override void OnEnterState() => _action?.Invoke();
    }
}
