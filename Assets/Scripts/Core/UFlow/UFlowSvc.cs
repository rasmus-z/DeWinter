﻿using System;
using System.Collections.Generic;
using Core;
using Util;

namespace UFlow
{
	internal class UTransitionMap
	{
		internal string TargetState;

		internal UTransitionMap() {}
		internal UTransitionMap(string targetState)
		{
			TargetState = targetState;
		}

		internal virtual UTransition Create()
		{
			return new UBasicTransition(TargetState);
		}
	}

	internal class UTransitionMap<T>:UTransitionMap where T : UTransition, new()
	{
		internal object[] Params;

		internal UTransitionMap() {}
		internal UTransitionMap(string targetState, params object [] parms)
		{
			TargetState = targetState;
			Params = parms;
		}

		internal override UTransition Create()
		{
			T t = new T();
			t.Parameters = Params;
			t._targetState = TargetState;
			return t;
		}
	}

	public class UFlowSvc : IAppService
	{
		private Dictionary<string, Func<UState>> _states = new Dictionary<string, Func<UState>>();
		private Dictionary<string, Dictionary<string, List<UTransitionMap>>> _transitions = new Dictionary<string, Dictionary<string, List<UTransitionMap>>>();
		private Dictionary<string, string> _initialStates = new Dictionary<string, string>();

		// Machines that have been instantiated.
		internal Dictionary<string, UMachine> _machines = new Dictionary<string, UMachine>();

		public UMachine GetMachine(string MachineID)
		{
			UMachine mac;
			return _machines.TryGetValue(MachineID, out mac) ? mac : null;
		}

		public void RegisterState<S>(string stateID) where S : UState, new()
		{
			if (!_states.ContainsKey(stateID))
			{
				_states[stateID] = (Func<UState>)(() => {
					UState state = new S();
					if (state is IInitializable)
						((IInitializable)state).Initialize();
					return state;
				});
			}
		}

		public void RegisterState<S, T>(string stateID, T arg) where S : UState, IInitializable<T>, new()
		{
			if (!_states.ContainsKey(stateID))
			{
				_states[stateID] = (Func<UState>)(() => {
					UState state = new S();
					((IInitializable<T>)state).Initialize(arg);
					return state;
				});
			}
		}

		public void RegisterTransition<T>(string machineID, string originState, string targetState, params object[] args) where T : UTransition, new()
		{
			List<UTransitionMap> transitions = GetTransitionList(machineID, originState);
			UTransitionMap<T> trans = new UTransitionMap<T>(targetState, args);
			transitions.Add(trans);
		}

		public void RegisterTransition(string machineID, string originState, string targetState)
		{
			List<UTransitionMap> transitions = GetTransitionList(machineID, originState);
			UTransitionMap map = new UTransitionMap(targetState);
			transitions.Add(map);
		}

		private List<UTransitionMap> GetTransitionList(string machineID, string originState)
		{
			Dictionary<string, List<UTransitionMap>> stateTransitions;
			List<UTransitionMap> transitions;
			if (!_transitions.TryGetValue(machineID, out stateTransitions))
			{
				_initialStates[machineID] = originState;
				_transitions[machineID] = stateTransitions = new Dictionary<string, List<UTransitionMap>>();
			}
			if (!stateTransitions.TryGetValue(originState, out transitions))
				stateTransitions[originState] = transitions = new List<UTransitionMap>();
			return transitions;
		}

		internal UState BuildState(string stateID, string machineID=null)
		{
			UState s = _transitions.ContainsKey(stateID) ? new UMachine() : _states[stateID]();
			UMachine mac;
			s._machine = (machineID != null && _machines.TryGetValue(machineID, out mac))
				? mac
				: null;
			s.ID = stateID;
			mac = s as UMachine;
			if (mac != null)
			{
				mac._uflow = this;
				mac._initialState = _initialStates[stateID];
				_machines[stateID] = mac;
			}
			return s;
		}

		internal UTransition[] BuildTransitions(string machineID, string originState)
		{
			Dictionary<string, List<UTransitionMap>> stateTransitions;
			if (!_transitions.TryGetValue(machineID, out stateTransitions)) return null;

			UMachine mac;
			if (!_machines.TryGetValue(machineID, out mac)) return null;

			List<UTransitionMap> transitionMaps;
			if (!stateTransitions.TryGetValue(originState, out transitionMaps)) return null;

			List<UTransition> transitions = new List<UTransition>();
			UTransition trans;

			foreach (UTransitionMap map in transitionMaps)
			{
				trans = map.Create();
				trans._machine = mac;
				transitions.Add(trans);
			}
			return transitions.ToArray();
		}

		public void InvokeMachine(string machineID)
		{
			BuildState(machineID).OnEnterState();
		}
	}
}
