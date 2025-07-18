using UnityEditor;
using UnityEngine;

namespace MagmaHeart.Core.SceneLoading
{
    [CustomEditor(typeof(SceneLoader))]
    public class SceneLoaderEditor : Editor
    {
        private SerializedProperty m_bootstraps;

        public void OnEnable()
        {
            m_bootstraps = serializedObject.FindProperty("m_bootstrapsKeyValues");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.UpdateIfRequiredOrScript();
            EditorGUILayout.PropertyField(m_bootstraps, new GUIContent("Bootstraps"));
            serializedObject.ApplyModifiedProperties();
        }
    }

    [CustomPropertyDrawer(typeof(SceneBootstrapKeyValue))]
    public class SceneBootstrapKeyValueDrawer : PropertyDrawer
    {
        private SerializedProperty m_scenePath;
        private SerializedProperty m_sceneName;
        private SerializedProperty m_bootstrap;
        private float m_singleLineHeight = EditorGUIUtility.singleLineHeight;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            int totalLines = 1;

            if (property.isExpanded)
                ++totalLines;

            return m_singleLineHeight * totalLines;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            m_scenePath = property.FindPropertyRelative("m_scenePath");
            m_sceneName = property.FindPropertyRelative("m_sceneName");
            m_bootstrap = property.FindPropertyRelative("m_bootstrap");

            Rect foldoutBox = new Rect(position.xMin, position.yMin, position.size.x, m_singleLineHeight);
            property.isExpanded = EditorGUI.Foldout(foldoutBox, property.isExpanded, label);

            if (property.isExpanded)
            {
                DrawSceneProperty(position);
                DrawBootstrapProperty(position);
            }

            EditorGUI.EndProperty();
        }

        private void DrawSceneProperty(Rect position)
        {
            EditorGUIUtility.labelWidth = 50;
            Rect drawArea = new Rect(position.xMin,
            position.yMin + m_singleLineHeight,
            position.size.x * 0.45f,
            m_singleLineHeight);

            SceneAsset oldSceneAsset = null;

            if (m_scenePath.stringValue != string.Empty)
                oldSceneAsset = AssetDatabase.LoadAssetAtPath<SceneAsset>(m_scenePath.stringValue);

            SceneAsset newSceneAsset = (SceneAsset)EditorGUI.ObjectField(drawArea, "Scene", oldSceneAsset, typeof(SceneAsset), false);

            if (oldSceneAsset != newSceneAsset)
            {
                m_sceneName.stringValue = newSceneAsset.name;
                m_scenePath.stringValue = AssetDatabase.GetAssetPath(newSceneAsset);
            }
        }

        private void DrawBootstrapProperty(Rect position)
        {
            EditorGUIUtility.labelWidth = 60;
            Rect drawArea = new Rect(position.xMin + position.width / 2,
            position.yMin + m_singleLineHeight,
            position.size.x / 2,
            m_singleLineHeight);

            EditorGUI.PropertyField(drawArea, m_bootstrap, new GUIContent("Bootstrap"));
        }
    }
}