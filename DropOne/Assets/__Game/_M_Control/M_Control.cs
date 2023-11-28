using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
            StartCoroutine(CellClick());
            IEnumerator CellClick()
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
                                _p.transform.SetParent(_partItem.transform);
                                _p.transform.DOLocalMove(Vector3.zero, 0.125f).SetEase(Ease.OutExpo);
                                Destroy(_p.gameObject, 0.125f);
                            }
                        }
                        _part.PartItems.Clear();
                        _part.PartItems.Add(_partItem);
                        _partItem.transform.SetParent(null);
                        _part.transform.position = _partItem.transform.position;
                        _partItem.transform.SetParent(_part.transform);
                        _partItem.OffsetI = 0;
                        _partItem.OffsetJ = 0;
                        SpriteCell _cell = _partItem.Cell;
                        //bu partItem'i düşeydeki boşluklara kaydır
                        for (int j = _cell.J - 1; j >= 0; j--)
                        {
                            RectPoint _point = new RectPoint(_cell.I, j);
                            if (grid.Contains(_point))
                            {
                                SpriteCell _c = grid[_point];
                                if (_c.PartItem == null)
                                {
                                    _partItem.OffsetJ = 0;
                                    _partItem.transform.SetParent(null);
                                    _part.transform.position = _c.transform.position;
                                    _partItem.transform.SetParent(_part.transform);
                                    _c.PartItem = _partItem;
                                    _cell.PartItem = null;
                                    _cell = _c;
                                    _partItem.Cell = _cell;
                                    _partItem.transform.DOLocalMove(Vector3.zero, 0.125f).SetEase(Ease.OutExpo);
                                }
                                else
                                {
                                    break;
                                }
                            }
                            else
                            {
                                break;
                            }

                        }

                        Part[] _allParts = FindObjectsOfType<Part>().OrderBy(x => x.transform.position.magnitude).ToArray();

                        for (int i = 0; i < _allParts.Length; i++)
                        {
                            Part _part1 = _allParts[i];
                            if (_part1 != null)
                            {

                                for (int j = 0; j < _part1.PartItems.Count; j++)
                                {
                                    _part1.PartItems[j].Cell.PartItem = null;
                                    _part1.PartItems[j].Cell = null;
                                }
                                Vector3 _firstPosition = _part1.transform.position;
                                int _verticalSearchCount = 1;

                                while (true)
                                {
                                    _part1.transform.position = _firstPosition + new Vector3(0, -_verticalSearchCount, 0);
                                    _verticalSearchCount++;

                                    bool _isOk = true;
                                    for (int j = 0; j < _part1.PartItems.Count; j++)
                                    {
                                        PartItem _partItem1 = _part1.PartItems[j];
                                        RectPoint _point = new RectPoint(_partItem1.OffsetI, _partItem1.OffsetJ);
                                        _point = _point + map[_part1.transform.position];
                                        if (grid.Contains(_point))
                                        {
                                            SpriteCell _cell1 = grid[_point];
                                            if (_cell1.PartItem != null)
                                            {
                                                _isOk = false;
                                                break;
                                            }
                                        }
                                        else
                                        {
                                            _isOk = false;
                                            break;
                                        }
                                    }
                                    if (_isOk == false)
                                    {
                                        _verticalSearchCount--;
                                        _verticalSearchCount--;
                                        _part1.transform.position = _firstPosition + new Vector3(0, -_verticalSearchCount, 0);
                                        for (int j = 0; j < _part1.PartItems.Count; j++)
                                        {
                                            PartItem _partItem1 = _part1.PartItems[j];
                                            RectPoint _point = new RectPoint(_partItem1.OffsetI, _partItem1.OffsetJ);
                                            _point = _point + map[_part1.transform.position];
                                            SpriteCell _cell1 = grid[_point];
                                            _cell1.PartItem = _partItem1;
                                            _partItem1.Cell = _cell1;
                                        }
                                        break;
                                    }

                                }





                            }
                        }
                    }
                }
                yield return new WaitForSeconds(1f);
            }
        }
    }
}

