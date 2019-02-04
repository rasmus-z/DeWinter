#if (UNITY_EDITOR)
using System;
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using Util;
namespace UFlow
{
    [CustomEditor(typeof(UMachineObject))]
    public class UMachineInspector : Editor
    {
        private int _lastIndex;
        private SerializedProperty _isNode;
        private SerializedProperty _index;
        private int _Index
        {
            get
            {
                return _index != null ? _index.intValue : -1;
            }
            set
            {
                if (_index != null) _index.intValue = value;
            }
        }

        private bool _IsNode
        {
            get
            {
                return _isNode != null && _isNode.boolValue;
            }
            set
            {
                if (_isNode != null) _isNode.boolValue = value;
            }
        }


        private void Apply() => serializedObject.ApplyModifiedProperties();

        private void OnEnable()
        {
            _index = serializedObject.FindProperty("_index");
            _Index = -1;
            _isNode = serializedObject.FindProperty("_isNode");
            Apply();
        }

        private void OnDisable()
        {
            _Index = -1;
            Apply();
        }

        public override void OnInspectorGUI()
        {
            SerializedProperty list;
            bool isNode = _IsNode;
            int index = _Index;
            EditorGUI.BeginChangeCheck();
            if (index >= 0)
            {
                list = serializedObject.FindProperty(isNode ? "Graph.Nodes" : "Graph.LinkData");
                if (list == null || _Index >= list.arraySize) _Index = index = -1;
                else
                {
                    SerializedProperty prop = list.GetArrayElementAtIndex(index);
                    if (prop == null) _Index = index = -1;
                    else if (!isNode) DrawLink(prop);
                    else DrawNode(prop, index != _lastIndex);
                }
            }
            if (index < 0)
            {
                // Todo: A Quick reference to nodes and links would be nice
            }
            EditorGUI.EndChangeCheck();
            _lastIndex = index;
        }

        private void DrawNode(SerializedProperty node, bool select)
        {
            if (select)
            {
                Array.Find(Resources.FindObjectsOfTypeAll<EditorWindow>(), w => w.titleContent.text == "Inspector").Focus();
                EditorGUI.FocusTextInControl("ID");
                TextEditor txt = (TextEditor)GUIUtility.GetStateObject(typeof(TextEditor), GUIUtility.keyboardControl);
                txt?.SelectAll();
            }
            EditorGUILayout.PropertyField(node.FindPropertyRelative("ID"));
            EditorGUILayout.PropertyField(node.FindPropertyRelative("OnEnterState"));
            EditorGUILayout.PropertyField(node.FindPropertyRelative("OnExitState"));
            EditorGUILayout.PropertyField(node.FindPropertyRelative("IsExit"));
        }

        private void DrawLink(SerializedProperty node)
        {
            //    node.getp
            //private uint _index;

            //[SerializeField, HideInInspector]
            //private UMachineObject _machine;

            //protected void Activate(bool valid, bool waitForValid) => _machine?.Activate(_index, valid, waitForValid);
            //protected void Activate(bool valid) => _machine?.Activate(_index, valid, false);
            //protected void Activate() => _machine?.Activate(_index, true, false);

            //// If Validate has zero listeners AND WaitForActivate is false, this is considered a default link.
            //public UnityEvent OnValidate; // These methods are evaluated immediately. Must call Activate() to validate correctly.
            //public bool WaitForActivate; // This persists the state until a callback calls Activate().
        }
    }
}
#endif
