using System;
using System.Collections;
using System.Collections.Generic;
using Gamelogic.Grids;
using Gamelogic.Grids.Examples;
using UnityEngine;
namespace Cihan
{
    public class M_Grid : M_Singleton<M_Grid>
    {


        public SpriteCell cellPrefab;
        public GameObject root;

        private RectGrid<SpriteCell> grid;
        private IMap3D<RectPoint> map;

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
            CreateGrid();
        }

        private void CreateGrid()
        {
            grid = RectGrid<SpriteCell>.Rectangle(5, 10);

            map = new RectMap(cellPrefab.Dimensions)
                .WithWindow(ExampleUtils.ScreenRect)
                .AlignMiddleCenter(grid)
                .To3DXY();
            foreach (RectPoint point in grid)
            {
                SpriteCell cell = Instantiate(cellPrefab);

                Vector3 worldPoint = map[point];

                cell.transform.parent = root.transform;
                cell.transform.localScale = Vector3.one;
                cell.transform.localPosition = worldPoint;

                cell.Color = ExampleUtils.Colors[point.GetColor4() % 4 * 4];

                cell.name = point.ToString();
                grid[point] = cell;
            }
        }

    }

}
