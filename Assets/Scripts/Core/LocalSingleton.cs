using UnityEngine;

namespace Core
{
    public class LocalSingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        public static T Instance => instance;
        private static T instance;
        
        protected virtual void Awake()
        {
            if (instance == null)
                instance = this as T;
            else
                Destroy(gameObject);
        }
    }
}