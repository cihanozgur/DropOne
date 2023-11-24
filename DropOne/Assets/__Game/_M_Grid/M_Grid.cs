using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Gamelogic.Grids;
using Gamelogic.Grids.Examples;
using UnityEngine;
namespace Cihan
{
    public class M_Grid : M_Singleton<M_Grid>
    {
        public int SizeX = 7;
        public int SizeY = 12;
        public SpriteCell cellPrefab;
        public RectGrid<SpriteCell> Grid;
        public IMap3D<RectPoint> Map;

        private void OnEnable()
        {
            M_Observer.OnLevelCreateBegin += OnLevelCreateBegin;
        }
        private void OnDisable()
        {
            M_Observer.OnLevelCreateBegin -= OnLevelCreateBegin;
        }

        private void OnLevelCreateBegin(int arg1, bool arg2)
        {
            DestroyChildren();
            CreateGrid();
        }



        private void DestroyChildren()
        {
            for (int i = transform.childCount - 1; i >= 0; i--)
                DestroyImmediate(transform.GetChild(i).gameObject);
        }

        private void CreateGrid()
        {

            Grid = RectGrid<SpriteCell>.Rectangle(SizeX, SizeY);

            Map = new RectMap(cellPrefab.Dimensions)
                .WithWindow(ExampleUtils.ScreenRect)
                // .AlignMiddleCenter(Grid)
                .To3DXY();
            foreach (RectPoint point in Grid)
            {
                SpriteCell cell = Instantiate(cellPrefab);

                Vector3 worldPoint = Map[point];

                cell.transform.parent = transform;
                cell.transform.localScale = Vector3.one;
                cell.transform.localPosition = worldPoint;

                //cell.Color = ExampleUtils.Colors[point.GetColor4() % 4 * 4];
                cell.I = point.X;
                cell.J = point.Y;
                cell.name = point.ToString();
                Grid[point] = cell;
            }

            M_Observer.OnCreatedGrid?.Invoke();
        }

    }

}
