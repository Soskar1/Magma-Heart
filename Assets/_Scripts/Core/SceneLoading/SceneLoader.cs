using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MagmaHeart.Core.SceneLoading
{
    public class SceneLoader : MonoBehaviour
    {
        private Dictionary<string, SceneBootstrap> m_bootstraps;
        [SerializeField] private SceneBootstrapKeyValue[] m_bootstrapsKeyValues;

        private void Awake()
        {
            m_bootstraps = new Dictionary<string, SceneBootstrap>();

            foreach (SceneBootstrapKeyValue bootstrap in m_bootstrapsKeyValues)
                m_bootstraps.Add(bootstrap.Scene, bootstrap.Bootstrap);

            DontDestroyOnLoad(gameObject);
        }

        private void OnEnable() => SceneManager.sceneLoaded += OnSceneLoaded;
        private void OnDisable() => SceneManager.sceneLoaded -= OnSceneLoaded;

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            SceneBootstrap prefab = m_bootstraps[scene.name];
            SceneBootstrap bootstrapInstance = Instantiate(prefab);
            bootstrapInstance.Initialize(this);
            bootstrapInstance.BootScene();
        }
    }

    [Serializable]
    public class SceneBootstrapKeyValue
    {
        [SerializeField] private SceneBootstrap m_bootstrap;
        [SerializeField] private string m_scenePath;
        [SerializeField] private string m_sceneName;

        public string Scene => m_sceneName;
        public SceneBootstrap Bootstrap => m_bootstrap;
    }
}