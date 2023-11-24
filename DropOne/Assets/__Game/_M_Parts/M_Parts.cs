using System.Collections;
using System.Collections.Generic;
using Gamelogic.Grids;
using UnityEngine;
namespace Cihan
{
    public class M_Parts : MonoBehaviour
    {
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
            for (int a = 0; a < 5; a++)
            {
                Part part = Instantiate(PartPrefabs[Random.Range(0, PartPrefabs.Length)], transform);
                for (int j = 0; j < SizeY; j++)
                {
                    for (int i = 0; i < SizeX; i++)
                    {
                        SpriteCell cell = M_Grid.I.Grid[new RectPoint(i, j)];
                        part.transform.position = cell.transform.position;
                        for (int x = 0; x < part.PartItems.Length; x++)
                        {

                        }
                    }
                }
            }
        }
    }

}
