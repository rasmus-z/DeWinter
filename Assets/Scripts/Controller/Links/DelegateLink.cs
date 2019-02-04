using UFlow;
using System;

namespace Ambition
{
    public class DelegateLink : ULink<Func<bool>>
    {
        private Func<bool> _delegate;

        public override void SetValue(Func<bool> delgate) => _delegate = delgate;

        public override bool Validate() => _delegate != null && _delegate();
    }
}
