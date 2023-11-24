using System.Collections;
using System.Collections.Generic;
using Cihan;
using UnityEngine;

public class Part : MonoBehaviour
{
    public PartItem[] PartItems;

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
