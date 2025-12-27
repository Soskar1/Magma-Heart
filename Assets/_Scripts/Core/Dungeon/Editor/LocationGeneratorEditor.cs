using UnityEditor;
using UnityEngine;

namespace MagmaHeart.Core.Dungeon
{
    [CustomEditor(typeof(DungeonGenerator))]
    public class LocationGeneratorEditor : Editor
    {
        private SerializedProperty m_generatorConfigFileName;
        private SerializedProperty m_useSeed;
        private SerializedProperty m_seed;

        public void OnEnable()
        {
            m_generatorConfigFileName = serializedObject.FindProperty("m_generatorConfigFileName");
            m_useSeed = serializedObject.FindProperty("m_useSeed");
            m_seed = serializedObject.FindProperty("m_seed");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.UpdateIfRequiredOrScript();

            DrawConfigFile();
            DrawSeed();

            serializedObject.ApplyModifiedProperties();
        }

        private void DrawConfigFile()
        {
            TextAsset oldConfigFile = null;

            if (m_generatorConfigFileName.stringValue != string.Empty)
                oldConfigFile = ExternalResources.LoadTextAsset(m_generatorConfigFileName.stringValue);

            TextAsset newConfigFile = (TextAsset)EditorGUILayout.ObjectField(new GUIContent("Config File"), oldConfigFile, typeof(TextAsset), false);

            if (oldConfigFile != newConfigFile)
                m_generatorConfigFileName.stringValue = newConfigFile.name;
        }

        private void DrawSeed()
        {
            EditorGUILayout.PropertyField(m_useSeed, new GUIContent("Use custom seed?"));

            ++EditorGUI.indentLevel;

            if (Application.isPlaying || !m_useSeed.boolValue)
                GUI.enabled = false;

            EditorGUILayout.PropertyField(m_seed, new GUIContent("Seed"));
            GUI.enabled = true;

            --EditorGUI.indentLevel;
        }
    }
}
