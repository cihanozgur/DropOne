using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cihan
{
    public class M_Singleton<T> : MonoBehaviour where T : Component
    {
        [SerializeField] private bool isDontDestroyOnLoad = false;

        public virtual void Awake()
        {
            // create the instance
            if (II == null)
            {
                II = this as T;
                if (isDontDestroyOnLoad)
                {
                    DontDestroyOnLoad(this.gameObject);
                }
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public static T II;

        public static T I
        {
            get
            {
                if (II == null)
                {
                    GameObject _g = GameObject.Find(typeof(T).Name);
                    if (_g != null)
                    {
                        II = _g.GetComponent<T>();
                    }
                }

                return II;
            }
        }
    }
}