using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cihan
{
  public class PartItem : MonoBehaviour
  {
    public MeshRenderer MeshRenderer;
    public int OffsetI;
    public int OffsetJ;
    int colorIndex;
    internal int ColorIndex
    {
      set
      {
        colorIndex = value;
        MeshRenderer.material.color = M_Color.I.Colors[colorIndex];
      }
      get { return colorIndex; }
    }
  }

}