﻿#pragma warning disable 0414

using System;
using UnityEngine;
using UnityEngine.Events;
using Util;
#if (UNITY_EDITOR)
using UnityEditor;
using UnityEditor.Callbacks;
#endif


namespace UFlow
{
    public class UStateObject
    {
        public UnityEvent OnEnterState;
        public UnityEvent OnExitState;
    }

    public class UConfigurableMachine : ScriptableObject, IDirectedGraphObjectConfig
    {
        [SerializeField]
        DirectedGraph<UStateObject> _machine;

#if (UNITY_EDITOR)
        SerializedObject _obj;

        [SerializeField]
        private int _index = -1;

        [SerializeField]
        private bool _isState = false;

        [OnOpenAsset(1)]
        public static bool OpenMachineConfig(int instanceID, int line)
        {
            UConfigurableMachine config = EditorUtility.InstanceIDToObject(instanceID) as UConfigurableMachine;
            return (config != null) && (null != GraphEditorWindow.Show(config));
        }

        [MenuItem("Assets/Create/Create Machine Config")]
        public static void CreateIncident()
        {
            ScriptableObjectUtil.CreateScriptableObject<UConfigurableMachine>("New Machine");
        }

        public SerializedObject GraphObject =>
            _obj ?? (_obj = new SerializedObject(this));

        public SerializedProperty GraphProperty => GraphProperty.FindPropertyRelative("_machine");

        public void Select(int index, bool isNode)
        {
            _index = index;
            _isState = isNode;
        }

        public void InitNodeData(SerializedProperty nodeData)
        {
            
        }

        public void InitLinkData(SerializedProperty linkData)
        {
            
        }

#endif
        public GUIContent GetGUIContent(int nodeIndex)
        {
            return new GUIContent(this.name);
        }
    }
}
