using System;
using System.Linq;
using System.Collections.Generic;
using Util;
using UnityEngine;
using UnityEngine.Events;
namespace UFlow
{
    [Serializable]
    public class UMachineState
    {
        public string ID;
        public UnityEvent OnEnterState;
        public UnityEvent OnExitState;

        // If IsExit is true, all Events will be fired as soon as the event is entered, and the machine will exit.
        // This option is not available if this state has outgoing links.
        public bool IsExit;
    }

    [Serializable]
    public class UMachineLink
    {
        internal uint _index;

        // If Validate has zero listeners AND WaitForActivate is false, this is considered a default link.
        public UnityEvent OnValidate; // These methods are evaluated immediately. Must call Activate() to validate correctly.
        public bool WaitForActivate; // This persists the state until a callback calls Activate().

        public bool IsDefault => !WaitForActivate && (OnValidate == null || OnValidate.GetPersistentEventCount() == 0);
    }

    [Serializable]
    public class UMachineDirectedGraph : DirectedGraph<UMachineState, UMachineLink>, IDisposable
    {
        private List<uint> _activeStates;
        private List<uint> _queuedStates;
        private bool _lockQueue; // lock while multiple links are being evaluated

        public void OnEnterMachine()
        {
            _activeStates = new List<uint>();
            _queuedStates = new List<uint>();
            for (uint i=((uint)LinkData.Length-1); i>=0; i--)
            {
                LinkData[i]._index = i;
            }
            EnterState(0);
        }

        public string[] GetActiveStates()
        {
            return _activeStates != null && Nodes != null
                ? (from i in _activeStates
                   where i >= 0 && i < Nodes.Length
                   select Nodes[i].ID).ToArray()
                 : new string[0];
        }

        private void EnterState(uint index)
        {
            if (!_activeStates.Contains(index) && index < Nodes.Length)
            {
                UMachineState s = Nodes[index];
                _activeStates.Add(index);
                s.OnEnterState.Invoke();
            }
            if (_queuedStates.All(i => _activeStates.Contains(i)))
            {
                // TODO Generate links
                uint[] links = _queuedStates.Distinct().ToArray();
                _lockQueue = true;

                Array.FindAll(Links, l => l.x == index);
                _queuedStates.Clear();
                _lockQueue = false;
            }
        }

        public void Activate(uint index, bool valid, bool waitForValid)
        {
            if (index < Links.Length)
            {
                Vector2Int link = Links[index];
                if (valid)
                {
                    if (!_queuedStates.Contains((uint)(link.y)))
                    {
                        _queuedStates.Add((uint)(link.y));
                    }
                    if (_activeStates.Contains((uint)(link.x)))
                    {
                        Nodes[link.x].OnExitState.Invoke();
                        _activeStates.Remove((uint)(link.x));
                    }
                }
                else if (!waitForValid)
                {
                    _lockQueue = true;
                    uint[] indices = Links.Where(l => l.x == link.x).Select((l, i) => (uint)i).ToArray();
                    indices = indices.Where(i => LinkData[i].IsDefault).ToArray();
                    Array.ForEach(indices, i => Activate(i, true, false));
                    _lockQueue = false;
                }
                if (!_lockQueue) EnterQueuedStates();
            }
        }

        private void EnterQueuedStates()
        {
            _lockQueue = true;
            List<uint> dequeued = _queuedStates;
            _queuedStates = new List<uint>();
            while (dequeued.Count > 0)
            {
                EnterState(dequeued[0]);
            }
            _lockQueue = false;
        }

        override public void Dispose()
        {
            _activeStates.Clear(); _activeStates = null;
            _queuedStates.Clear(); _queuedStates = null;
            base.Dispose();
        }
    }
}
