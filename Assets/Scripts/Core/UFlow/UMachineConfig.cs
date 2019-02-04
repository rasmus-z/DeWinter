using System;
using UnityEngine;
namespace UFlow
{
    public class UMachineConfig : ScriptableObject
    {
        internal UMachineDirectedGraph _machine;
        internal uint _invokerIndex;

        protected void Activate() => _machine.Activate(_invokerIndex, true, false);
        protected void Fail() => _machine.Activate(_invokerIndex, false, false);
        protected void Wait() => _machine.Activate(_invokerIndex, false, true);
    }
}
