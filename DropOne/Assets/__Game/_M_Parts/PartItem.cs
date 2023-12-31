using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cihan
{
  public class PartItem : MonoBehaviour
  {
    public Part CurrentPart;
    public MeshRenderer MeshRenderer;
    public int OffsetI;
    public int OffsetJ;
    public int colorIndex;

    public SpriteCell Cell;

    internal int ColorIndex
    {
      set
      {
        colorIndex = value;
        MeshRenderer.material.color = M_Color.I.Colors[colorIndex];
      }
      get { return colorIndex; }
    }

    internal List<PartItem> ToList()
    {
      throw new NotImplementedException();
    }
  }

}