using UnityEngine;

namespace BlazeTools.BlazeUtils
{
    /// <summary>
    /// An abstract class implementing the singleton pattern witch assign the object to DontDestroyOnLoad scene.
    /// </summary>
    /// <typeparam name="T">The type of the MonoBehaviour derived class that this singleton represents.</typeparam>
    public abstract class MonoBehaviourSingleton<T> : MonoBehaviour where T : Component
    {
        /// <summary>
        /// The singleton instance.
        /// </summary>
        private protected static T instance;

        /// <summary>
        /// Access the existing instance of this singleton or create a new one.
        /// </summary>
        /// <remarks>
        /// Check for HasInstance before accessing this from OnDestroy, OnDisable, or OnApplicationQuit functions.
        /// </remarks>
        public static T Instance
        {
            get
            {
                instance ??= FindExistingInstance() ?? CreateNewInstance();
                return instance;
            }
        }

        /// <summary>
        /// Check if the singleton has its instance.
        /// </summary>
        /// <remarks>
        /// Useful if you want to access the singleton instance from OnDestroy, OnDisable or OnApplicationQuit functions.
        /// </remarks>
        public static bool HasInstance => instance != null;

        /// <summary>
        /// Overridden Unity Awake function, runs before first frame.
        /// </summary>
        protected virtual void Awake()
        {
            AssignObjectAsInstance();
            DontDestroyOnLoad(gameObject);
        }

        /// <summary>
        /// Assign the newly created singleton to the instance or remove multiplied instances if they occur.
        /// </summary>
        private protected void AssignObjectAsInstance()
        {
            var component = GetComponent<T>();
            if (instance == null)
            {
                instance = component;
            }
            else if (component != instance)
            {
                Destroy(component);
                Destroy(gameObject);
            }
        }

        private protected static T FindExistingInstance()
        {
            var existingInstances = FindObjectsOfType<T>();

            if (existingInstances == null || existingInstances.Length == 0)
            {
                return null;
            }

            return existingInstances[0];
        }

        private static T CreateNewInstance()
        {
            var createdObject = new GameObject($"Singleton {typeof(T)}");
            return createdObject.AddComponent<T>();
        }
    }
}
