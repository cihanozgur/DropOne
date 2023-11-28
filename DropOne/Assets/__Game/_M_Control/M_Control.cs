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
                        CombinePartItem(cell);

                        VerticalMove();

                        RowDelete();

                        DeterminePartClusters();

                        yield return new WaitForSeconds(2.25f);

                        VerticalMove();


                    }
                    yield return new WaitForSeconds(0.25f);
                }
            }
        }

        private void DeterminePartClusters()
        {
            // bu part itemlerin komşuluk ilişkileri bozulduysa bunları ayırıp yeni partlar oluştur
            Part[] _allParts_00 = FindObjectsOfType<Part>().OrderBy(x => x.transform.position.magnitude).ToArray();
            for (int i = 0; i < _allParts_00.Length; i++)
            {
                Part _part1 = _allParts_00[i];
                if (_part1 != null && _part1.PartItems.Count > 1)
                {
                    List<List<PartItem>> _partItems = FindPhrases(_part1);
                    if (_partItems.Count > 1)
                    {
                        print(_partItems.Count);
                        for (int j = 0; j < _partItems.Count; j++)
                        {
                            Part _part2 = Instantiate(M_Parts.I.PartEmptyPrefab, _part1.transform.parent);
                            _part2.ColorIndex = _part1.ColorIndex;
                            _part2.transform.position = _partItems[j][0].transform.position;
                            _part2.PartItems = _partItems[j];
                            for (int k = 0; k < _part2.PartItems.Count; k++)
                            {
                                _part2.PartItems[k].CurrentPart = _part2;
                                _part2.PartItems[k].transform.SetParent(_part2.transform);
                                _part2.PartItems[k].OffsetI = _part2.PartItems[k].OffsetI - _partItems[j][0].OffsetI;
                                _part2.PartItems[k].OffsetJ = _part2.PartItems[k].OffsetJ - _partItems[j][0].OffsetJ;
                            }
                        }
                        Destroy(_part1.gameObject);
                    }
                }
            }
        }

        List<List<PartItem>> FindPhrases(Part part)
        {
            //part içindeki part itemlerin offsetI offsetJ değişkenlerini grid index gibi düşün. bunları kullanaak öbekleri bul
            //öbeklerin içindeki part itemlerin komşuluk ilişkileri bozulduysa bunları ayırıp yeni partlar oluştur
            List<List<PartItem>> _phrases = new List<List<PartItem>>();
            List<PartItem> _partItems = part.PartItems.ToList();
            while (_partItems.Count > 0)
            {
                List<PartItem> _phrase = new List<PartItem>();
                _phrase.Add(_partItems[0]);
                _partItems.RemoveAt(0);
                for (int i = 0; i < _phrase.Count; i++)
                {
                    PartItem _partItem1 = _phrase[i];
                    for (int j = 0; j < _partItems.Count; j++)
                    {
                        PartItem _partItem2 = _partItems[j];
                        if (Mathf.Abs(_partItem1.OffsetI - _partItem2.OffsetI) + Mathf.Abs(_partItem1.OffsetJ - _partItem2.OffsetJ) == 1)
                        {
                            _phrase.Add(_partItem2);
                            _partItems.RemoveAt(j);
                            j--;
                        }
                    }
                }
                _phrases.Add(_phrase);
            }
            return _phrases;



        }

        private void RowDelete()
        {
            ///////////////////////////////////
            //hücreleri en alttan ukarıya doğru tarayarak dolu satırları sil
            for (int j = 0; j < grid.Height; j++)
            {
                bool _isFull = true;
                for (int i = 0; i < grid.Width; i++)
                {
                    RectPoint _point = new RectPoint(i, j);
                    if (grid.Contains(_point))
                    {
                        SpriteCell _cell2 = grid[_point];
                        if (_cell2.PartItem == null)
                        {
                            _isFull = false;
                            break;
                        }
                    }
                    else
                    {
                        _isFull = false;
                        break;
                    }
                }
                if (_isFull)
                {
                    for (int i = 0; i < grid.Width; i++)
                    {
                        RectPoint _point = new RectPoint(i, j);
                        if (grid.Contains(_point))
                        {
                            SpriteCell _cell2 = grid[_point];
                            if (_cell2.PartItem != null)
                            {
                                _cell2.PartItem.CurrentPart.PartItems.Remove(_cell2.PartItem);
                                Destroy(_cell2.PartItem.gameObject);
                            }
                        }
                    }
                }
            }
            Part[] _allParts_01 = FindObjectsOfType<Part>().OrderBy(x => x.transform.position.magnitude).ToArray();
            for (int i = 0; i < _allParts_01.Length; i++)
            {
                Part _part2 = _allParts_01[i];
                _part2.PartItems = _part2.PartItems.Where(qq => qq != null).ToList();
                if (_part2.PartItems.Count == 0)
                {
                    Destroy(_part2.gameObject);
                }
            }
        }

        private void VerticalMove()
        {
            Part[] _allParts_00 = FindObjectsOfType<Part>().OrderBy(x => x.transform.position.magnitude).ToArray();

            for (int i = 0; i < _allParts_00.Length; i++)
            {
                Part _part1 = _allParts_00[i];
                if (_part1 != null)
                {

                    for (int j = 0; j < _part1.PartItems.Count; j++)
                    {
                        _part1.PartItems[j].Cell.PartItem = null;
                        _part1.PartItems[j].Cell = null;
                    }
                    Vector3 _firstPosition = _part1.transform.position;
                    int _verticalSearchCount = 1;

                    int breakCount = 0;
                    while (true && breakCount < 1000)
                    {
                        breakCount++;
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

        private void CombinePartItem(SpriteCell cell)
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
        }
    }
}
