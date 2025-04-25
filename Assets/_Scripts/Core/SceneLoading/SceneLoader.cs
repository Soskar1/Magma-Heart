using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MagmaHeart.Core.SceneLoading
{
    public class SceneLoader : MonoBehaviour
    {
        [SerializeField] private BootstrapDictionary m_bootstrapData;
        private Dictionary<string, DungeonBootstrap> m_bootstraps;
        private DataTransfer m_data;

        private void Awake()
        {
            m_bootstraps = m_bootstrapData.ToDictionary();
            DontDestroyOnLoad(gameObject);
        }
        private void OnEnable() => SceneManager.sceneLoaded += OnSceneLoaded;
        private void OnDisable() => SceneManager.sceneLoaded -= OnSceneLoaded;

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            DungeonBootstrap prefab = m_bootstraps[scene.name];
            DungeonBootstrap bootstrapInstance = Instantiate(prefab);
            bootstrapInstance.Initialize(this);
            bootstrapInstance.BootScene();
        }

        public void LoadNextScene(DataTransfer data)
        {
            m_data = data;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }

    [Serializable]
    public class BootstrapDictionary
    {
        [SerializeField] private List<SceneBootstrap> m_bootstraps;
        public List<SceneBootstrap> Bootstraps => m_bootstraps;
    }

    public static class BootstrapDictionaryExtensions
    {
        public static Dictionary<string, DungeonBootstrap> ToDictionary(this BootstrapDictionary bootstrapers)
        {
            Dictionary<string, DungeonBootstrap> dict = new Dictionary<string, DungeonBootstrap>();

            foreach (SceneBootstrap bootstrap in bootstrapers.Bootstraps)
                dict.Add(bootstrap.Scene.name, bootstrap.Bootstrap);

            return dict;
        }
    }

    [Serializable]
    public class SceneBootstrap
    {
        [SerializeField] private SceneAsset m_scene;
        [SerializeField] private DungeonBootstrap m_bootstrap;

        public SceneAsset Scene => m_scene;
        public DungeonBootstrap Bootstrap => m_bootstrap;
    }
}