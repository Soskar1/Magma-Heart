using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace MagmaHeart.Core.Artifacts
{
    [CustomEditor(typeof(ArtifactData))]
    public class ArtifactDataEditor : Editor
    {
        private ArtifactData m_data;

        private void OnEnable()
        {
            m_data = (ArtifactData)target;
        }

        public override void OnInspectorGUI()
        {
            m_data.Name = EditorGUILayout.TextField("Name", m_data.Name);
            m_data.Rarity = (Rarity)EditorGUILayout.EnumPopup("Rarity", m_data.Rarity);

            m_data.Sprite = (Sprite)EditorGUILayout.ObjectField("Sprite", m_data.Sprite, typeof(Sprite), false);
            EditorGUILayout.Foldout(true, "Description");
            EditorStyles.textField.wordWrap = true;
            m_data.Description = EditorGUILayout.TextArea(m_data.Description, GUILayout.Height(80));

            EditorGUILayout.LabelField("Per level stats / skills", EditorStyles.boldLabel);
            if (m_data.Rarity == Rarity.Common || m_data.Rarity == Rarity.Rare)
                DrawLevels(5);
            else if (m_data.Rarity == Rarity.Epic || m_data.Rarity == Rarity.Legendary)
                DrawLevels(3);
            else
                DrawLevels(2);
        }

        private void DrawLevels(int maxLevel)
        {
            for (int i = 0; i < maxLevel; ++i)
            {
                List<IStatModifier> modifiers = m_data.StatModifiers[i];
                EditorGUILayout.LabelField($"Level {i + 1}", EditorStyles.boldLabel);
                EditorGUI.indentLevel++;

                for (int j = 0; j < modifiers.Count; j++)
                {
                    var modifier = modifiers[j];

                    EditorGUILayout.BeginVertical("box");
                    DrawModifier(modifier);

                    if (GUILayout.Button("Remove", GUILayout.Width(100)))
                    {
                        modifiers.RemoveAt(j);
                        break;
                    }

                    EditorGUILayout.EndVertical();
                }

                if (GUILayout.Button("Add Stat Modifier"))
                {
                    int groupIndex = i;
                    StatModifierWizard.Show(newModifier =>
                    {
                        if (newModifier != null)
                        {
                            modifiers.Add(newModifier);
                            EditorUtility.SetDirty(m_data);
                        }
                    });
                }

                EditorGUI.indentLevel--;
                EditorGUILayout.Space(30);

                if (GUI.changed)
                    EditorUtility.SetDirty(m_data);
            }
        }

        private void DrawModifier(IStatModifier modifier)
        {
            // Since IStatModifier is an interface, we need to check type
            if (modifier is HealthStatModifier health)
            {
                EditorGUILayout.LabelField("HealthStatModifier", EditorStyles.boldLabel);
                health.AdditionalHealth = EditorGUILayout.FloatField("Additional Health", health.AdditionalHealth);
            }
            else
            {
                EditorGUILayout.HelpBox("Unknown modifier type", MessageType.Warning);
            }
        }
    }
}