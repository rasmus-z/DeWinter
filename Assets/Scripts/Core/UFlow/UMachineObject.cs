#pragma warning disable 0414

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
#if (UNITY_EDITOR)
using UnityEditor;
using UnityEditor.Callbacks;
#endif
using Util;

namespace UFlow
{
    [Serializable]
    public class UMachineObject : ScriptableObject, IDirectedGraphObjectConfig
    {
        public UMachineDirectedGraph Graph;

#if (UNITY_EDITOR)

        [SerializeField, HideInInspector]
        private int _index = -1;

        [SerializeField, HideInInspector]
        private bool _isNode;

        private SerializedObject _graphObject;

        private void OnEnable()
        {
            if (Graph == null) Graph = new UMachineDirectedGraph();
            _graphObject = new SerializedObject(this);
        }

        public void Select(int index, bool isNode)
        {
            GraphObject.FindProperty("_index").intValue = index;
            GraphObject.FindProperty("_isNode").boolValue = isNode;
        }

        public SerializedObject GraphObject
        {
            get {
                if (_graphObject == null)
                    _graphObject = new SerializedObject(this);
                return _graphObject;
            }
        }

        public SerializedProperty GraphProperty => GraphObject.FindProperty("Graph");

        public void InitNodeData(SerializedProperty nodeData)
        {
            nodeData.FindPropertyRelative("ID").stringValue = "New State";
        }

        public void InitLinkData(SerializedProperty linkData)
        {
            linkData.FindPropertyRelative("_machine");
        }

        [OnOpenAsset(1)]
        public static bool OpenMachine(int instanceID, int line)
        {
            UMachineObject machine = EditorUtility.InstanceIDToObject(instanceID) as UMachineObject;
            return (machine != null) && (null != GraphEditorWindow.Show(machine));
        }

        [MenuItem("Assets/Create/Create UFlow Machine")]
        public static void CreateIncident()
        {
            ScriptableObjectUtil.CreateScriptableObject<UMachineObject>("New UFlow Machine");
        }
        public GUIContent GetGUIContent(int nodeIndex)
        {
            GUI.color = (nodeIndex == 0 ? Color.cyan : Color.white);
            GUI.skin.button.alignment = TextAnchor.MiddleCenter;
            string str = Graph.Nodes[nodeIndex].ID;
            return new GUIContent(str);
        }
#endif
    }

#if (UNITY_EDITOR)
    [CustomEditor(typeof(UMachineObject))]
    public class UMachineConfigDrawer : Editor
    {
        
    }
#endif
}
