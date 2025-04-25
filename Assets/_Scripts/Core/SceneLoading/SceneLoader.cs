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
        private Dictionary<string, SceneBootstrap> m_bootstraps;
        private DataTransfer m_data;

        private void Awake()
        {
            m_data = new DataTransfer();
            m_bootstraps = m_bootstrapData.ToDictionary();
            DontDestroyOnLoad(gameObject);
        }
        private void OnEnable() => SceneManager.sceneLoaded += OnSceneLoaded;
        private void OnDisable() => SceneManager.sceneLoaded -= OnSceneLoaded;

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            SceneBootstrap prefab = m_bootstraps[scene.name];
            SceneBootstrap bootstrapInstance = Instantiate(prefab);
            bootstrapInstance.Initialize(this, m_data.ObtainedArtifacts);
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
        [SerializeField] private List<SceneBootstrapKeyValue> m_bootstraps;
        public List<SceneBootstrapKeyValue> Bootstraps => m_bootstraps;
    }

    public static class BootstrapDictionaryExtensions
    {
        public static Dictionary<string, SceneBootstrap> ToDictionary(this BootstrapDictionary bootstrapers)
        {
            Dictionary<string, SceneBootstrap> dict = new Dictionary<string, SceneBootstrap>();

            foreach (SceneBootstrapKeyValue bootstrap in bootstrapers.Bootstraps)
                dict.Add(bootstrap.Scene.name, bootstrap.Bootstrap);

            return dict;
        }
    }

    [Serializable]
    public class SceneBootstrapKeyValue
    {
        [SerializeField] private SceneAsset m_scene;
        [SerializeField] private SceneBootstrap m_bootstrap;

        public SceneAsset Scene => m_scene;
        public SceneBootstrap Bootstrap => m_bootstrap;
    }
}