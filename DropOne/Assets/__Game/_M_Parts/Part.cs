using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cihan;
using UnityEngine;
namespace Cihan
{
    public class Part : MonoBehaviour
    {
        public List<PartItem> PartItems;
        public int ColorIndex = 0;

        public void Setup()
        {
            M_Color.I.ColorCounter++;
            ColorIndex = M_Color.I.ColorCounter % M_Color.I.Colors.Length;
            for (int i = 0; i < PartItems.Count; i++)
            {
                PartItems[i].ColorIndex = ColorIndex;
            }
        }

#if UNITY_EDITOR

        [ContextMenu("SetOffsetIJ")]
        public void SetOffsetIJ()
        {
            PartItems = GetComponentsInChildren<PartItem>().ToList();
            for (int i = 0; i < PartItems.Count; i++)
            {
                PartItems[i].OffsetI = (int)PartItems[i].transform.localPosition.x;
                PartItems[i].OffsetJ = (int)PartItems[i].transform.localPosition.y;
                PartItems[i].CurrentPart = this;
            }
            UnityEditor.EditorUtility.SetDirty(gameObject);
        }
#endif
    }

}
