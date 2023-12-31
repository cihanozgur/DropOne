using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Gamelogic.Grids;
using Unity.VisualScripting;
using UnityEngine;
namespace Cihan
{
    public class M_Parts : M_Singleton<M_Parts>
    {

        public Part PartEmptyPrefab;
        public Part[] PartPrefabs;
        public int SizeX = 7;
        public int SizeY = 12;
        private void OnEnable()
        {
            M_Observer.OnCreatedGrid += CreatedGrid;
        }
        private void OnDisable()
        {
            M_Observer.OnCreatedGrid -= CreatedGrid;
        }

        private void CreatedGrid()
        {
            SizeX = M_Grid.I.SizeX;
            SizeY = M_Grid.I.SizeY;

            AddBeginParts();
        }

        private void AddBeginParts()
        {
            StartCoroutine(AddBeginParts());
            IEnumerator AddBeginParts()
            {
                for (int a = 0; a < 200; a++)
                {
                    Part part = Instantiate(PartPrefabs[Random.Range(0, PartPrefabs.Length)], transform);
                    part.Setup();
                    bool isOk = true;
                    for (int j = 0; j < SizeY; j++)
                    {
                        int r = Random.Range(0, 2);
                        for (int i = 0; i < SizeX; i++)
                        {
                            int ii = -1;
                            if (r == 0)
                            {
                                ii = i;
                            }
                            else
                            {
                                ii = SizeX - i - 1;
                            }
                            SpriteCell cell = M_Grid.I.Grid[new RectPoint(ii, j)];
                            part.transform.position = cell.transform.position;
                            isOk = true;
                            for (int x = 0; x < part.PartItems.Count; x++)
                            {
                                RectPoint point = new RectPoint(part.PartItems[x].OffsetI + ii, part.PartItems[x].OffsetJ + j);
                                if (M_Grid.I.Grid.Contains(point))
                                {
                                    if (M_Grid.I.Grid[point].PartItem != null)
                                    {
                                        isOk = false;
                                    }
                                }
                                else
                                {
                                    isOk = false;
                                }
                            }
                            if (isOk)
                            {
                                for (int x = 0; x < part.PartItems.Count; x++)
                                {
                                    RectPoint point = new RectPoint(part.PartItems[x].OffsetI + ii, part.PartItems[x].OffsetJ + j);
                                    M_Grid.I.Grid[point].PartItem = part.PartItems[x];
                                    part.PartItems[x].Cell = M_Grid.I.Grid[point];
                                }
                            }
                            if (isOk)
                            {
                                break;
                            }
                        }
                        if (isOk)
                        {
                            break;
                        }

                    }

                }
                yield return null;
            }



        }


    }
}
