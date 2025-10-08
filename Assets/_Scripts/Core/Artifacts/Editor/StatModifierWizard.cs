using System;
using UnityEditor;
using UnityEngine;

namespace MagmaHeart.Core.Artifacts
{
    public class StatModifierWizard : EditorWindow
    {
        private Action<IStatModifier> _onModifierCreated;

        private int _selectedTypeIndex;
        private readonly string[] _modifierTypes = new string[]
        {
            "HealthStatModifier"
        };

        private float _healthValue = 0f;

        public static void Show(Action<IStatModifier> onCreated)
        {
            StatModifierWizard window = CreateInstance<StatModifierWizard>();
            window.titleContent = new GUIContent("Add Stat Modifier");
            window._onModifierCreated = onCreated;
            window.position = new Rect(Screen.width / 2f, Screen.height / 2f, 300f, 160f);
            window.ShowUtility();
        }

        private void OnGUI()
        {
            EditorGUILayout.LabelField("Select Stat Modifier Type", EditorStyles.boldLabel);
            _selectedTypeIndex = EditorGUILayout.Popup("Type", _selectedTypeIndex, _modifierTypes);

            EditorGUILayout.Space(10);

            // Show fields dynamically based on selection
            if (_modifierTypes[_selectedTypeIndex] == "HealthStatModifier")
            {
                _healthValue = EditorGUILayout.FloatField("Additional Health", _healthValue);
            }

            EditorGUILayout.Space(10);
            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button("Cancel"))
            {
                Close();
            }

            if (GUILayout.Button("Create"))
            {
                IStatModifier newModifier = null;

                switch (_modifierTypes[_selectedTypeIndex])
                {
                    case "HealthStatModifier":
                        newModifier = new HealthStatModifier(_healthValue);
                        break;
                }

                _onModifierCreated?.Invoke(newModifier);
                Close();
            }

            EditorGUILayout.EndHorizontal();
        }
    }
}