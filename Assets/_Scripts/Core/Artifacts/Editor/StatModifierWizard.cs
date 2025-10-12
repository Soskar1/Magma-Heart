using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace MagmaHeart.Core.Artifacts
{
    public class StatModifierWizard : EditorWindow
    {
        private Action<StatModifierModel> m_onModifierCreated;
        private readonly List<Type> m_modifierTypes = new();
        private Vector2 m_scroll;

        public static void Show(Action<StatModifierModel> onCreated)
        {
            StatModifierWizard window = CreateInstance<StatModifierWizard>();
            window.titleContent = new GUIContent("Stat Modifier Wizard");
            window.m_onModifierCreated = onCreated;
            window.position = new Rect(Screen.width / 2f, Screen.height / 2f, 300f, 350f);
            window.InitializeTypes();
            window.ShowUtility();
        }

        private void InitializeTypes()
        {
            Type interfaceType = typeof(IStatModifier);
            m_modifierTypes.Clear();

            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                Type[] types;
                try
                {
                    types = assembly.GetTypes();
                }
                catch
                {
                    continue;
                }

                foreach (Type type in types)
                    if (interfaceType.IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract)
                        m_modifierTypes.Add(type);
            }

            m_modifierTypes.Sort((a, b) => a.Name.CompareTo(b.Name));
        }

        private void OnGUI()
        {
            EditorGUILayout.LabelField("Available Stat Modifiers", EditorStyles.boldLabel);
            EditorGUILayout.Space();

            m_scroll = EditorGUILayout.BeginScrollView(m_scroll);

            foreach (Type type in m_modifierTypes)
            {
                if (GUILayout.Button(ObjectNames.NicifyVariableName(type.Name), GUILayout.Height(24)))
                {
                    IStatModifier instance = (IStatModifier)Activator.CreateInstance(type);
                    m_onModifierCreated?.Invoke(instance.ToModel());
                    Close();
                }
            }

            EditorGUILayout.EndScrollView();

            GUILayout.FlexibleSpace();
            EditorGUILayout.BeginHorizontal(EditorStyles.helpBox);
            GUILayout.FlexibleSpace();

            if (GUILayout.Button("Cancel", GUILayout.Width(100)))
                Close();

            EditorGUILayout.EndHorizontal();
        }
    }
}