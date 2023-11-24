using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Gamelogic.Grids;
using UnityEngine;
namespace Cihan
{
    public class M_Control : MonoBehaviour
    {
        RectGrid<SpriteCell> grid;
        IMap3D<RectPoint> map;

        private void OnEnable()
        {
            M_Observer.OnFingerDown += FingerDown;
            M_Observer.OnCreatedGrid += CreatedGrid;

        }
        private void OnDisable()
        {
            M_Observer.OnFingerDown -= FingerDown;
            M_Observer.OnCreatedGrid -= CreatedGrid;
        }

        private void CreatedGrid()
        {
            grid = M_Grid.I.Grid;
            map = M_Grid.I.Map;
        }

        private void FingerDown(Vector2 vector)
        {
            Vector3 _worldPosition = CihanUtility.GetWorldPosDirectionZ(vector);

            CellClick(_worldPosition);
        }

        private void CellClick(Vector3 worldPosition)
        {
            RectPoint point = map[worldPosition];


            if (grid.Contains(point))
            {
                SpriteCell cell = grid[point];
                if (cell.PartItem != null)
                {
                    PartItem _partItem = cell.PartItem;
                    Part _part = _partItem.CurrentPart;
                    for (int i = 0; i < _part.PartItems.Count; i++)
                    {
                        PartItem _p = _part.PartItems[i];
                        if (_partItem != _p)
                        {
                            _p.Cell.PartItem = null;
                            _p.transform.DOLocalMove(_partItem.transform.localPosition, 0.125f).SetEase(Ease.OutExpo);
                            Destroy(_p.gameObject, 0.125f);
                        }
                    }

                }
            }
        }
    }

}
