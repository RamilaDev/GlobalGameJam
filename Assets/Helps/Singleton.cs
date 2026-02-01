using System.Diagnostics;
using UnityEngine;

namespace Elven
{

    /// <summary>
    /// Clase genérica que permite declarar cualquier clase como singleton, forzar existencia única, exponer su instancia etc
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {

        private static bool IsQuitting;
        private void OnApplicationQuit()
        {
            IsQuitting = true;
        }


        private static T _instance;

        private static readonly object _lock = new object();

        // Permite definir si la instancia debe persistir entre escenas.
        [SerializeField] private bool dontDestroyOnLoad = false;


        public static T Instance
        {
            get
            {
                lock (_lock)
                {

                    if (_instance == null)
                    {

                        if (IsQuitting)
                        {
                            StackFrame stackFrame = new();
                            UnityEngine.Debug.LogWarning("Some object tried to create an instance of a singleton while the application was quitting \n" + stackFrame);
                            return null;
                        }


                        _instance = FindFirstObjectByType<T>();




                    }
                    return _instance;
                }
            }
        }

        protected virtual void Awake()
        {

            if (_instance != null && _instance != this)
            {
                return;
            }


            _instance = this as T;

            if (dontDestroyOnLoad)
            {
                //DontDestroyOnLoad(gameObject);
            }
        }
    }
}