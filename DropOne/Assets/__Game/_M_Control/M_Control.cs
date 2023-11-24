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
                    //bu partları diketde boşluklara kaydır
                    for (int i = 0; i < _allParts.Length; i++)
                    {
                        Part _p = _allParts[i];
                        //bu partın itemlarını düşeydeki boşluklara kaydır. boşluk demek cell'in partItem'ı null demek. hiç birşeyi sağa sola kaydırmıyoruz.
                        //aşağıda boşluk yoksda sağa sola kaydırma. sadece düşeydeki boşluklara kaydırma var.
                        //boşluk bulamazsan olduğu yerde dursunlar.partitemlerin part içindeki yapıları bozulmasın

                    }
                }





            }
        }
    }
}

