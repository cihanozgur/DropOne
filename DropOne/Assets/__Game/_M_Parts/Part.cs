using System.Collections;
using System.Collections.Generic;
using Cihan;
using UnityEngine;
namespace Cihan
{
    public class Part : MonoBehaviour
    {
        public PartItem[] PartItems;
        public int ColorIndex = 0;

        private void Start()
        {
            M_Color.I.ColorCounter++;
            ColorIndex = M_Color.I.ColorCounter % M_Color.I.Colors.Length;
            for (int i = 0; i < PartItems.Length; i++)
            {
                PartItems[i].ColorIndex = ColorIndex;
            }
        }

#if UNITY_EDITOR

        [ContextMenu("SetOffsetIJ")]
        public void SetOffsetIJ()
        {
            for (int i = 0; i < PartItems.Length; i++)
            {
                PartItems[i].OffsetI = (int)PartItems[i].transform.localPosition.x;
                PartItems[i].OffsetJ = (int)PartItems[i].transform.localPosition.y;
            }
            UnityEditor.EditorUtility.SetDirty(gameObject);
        }
#endif
    }

}
