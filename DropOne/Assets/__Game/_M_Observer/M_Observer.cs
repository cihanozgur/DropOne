using System;
using UnityEngine;

namespace Cihan
{
    public class M_Observer : MonoBehaviour
    {
        public static Action OnFirstOpen;

        public static Action<int, bool> OnLevelCreateBegin;
        public static Action OnLevelCreateComplete;
        public static Action OnLevelStartBegin;
        public static Action OnLevelStartComplete;



        public static Action<Vector2> OnFingerDown;

        public static Action OnCreatedGrid;

        private void Start()
        {
            OnFirstOpen?.Invoke();
        }

        private void OnEnable()
        {
            OnFirstOpen += OnFirstOpenHandler;
        }



        private void OnDisable()
        {
            OnFirstOpen -= OnFirstOpenHandler;
        }

        private void OnFirstOpenHandler()
        {
            OnLevelCreateBegin?.Invoke(0, true);
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                OnFingerDown?.Invoke(Input.mousePosition);
            }
        }
    }
}