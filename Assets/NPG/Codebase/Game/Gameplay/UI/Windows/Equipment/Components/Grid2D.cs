using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace NPG.Codebase.Game.Gameplay.UI.Windows.Equipment.Components
{
    public class Grid2D<T> : IEnumerable<T> where T : class
    {
        public int Columns { get; private set; }
        public int Rows { get; private set; }
        public int CellSize { get; }

        private List<List<T>> _gridList;
        
        public T this[int row, int column]
        {
            get => _gridList[row][column];
            set => _gridList[row][column] = value;
        }

        public Grid2D(int columns, int rows, int cellSize)
        {
            Columns = columns;
            Rows = rows;
            CellSize = cellSize;
            _gridList = new List<List<T>>(rows);
            for (int i = 0; i < rows; i++)
            {
                var row = new List<T>(columns);
                for (int j = 0; j < columns; j++)
                    row.Add(null);
                _gridList.Add(row);
            }
        }

        /// <summary>
        /// Initializes the grid with the provided values.
        /// </summary>
        /// <param name="values"></param>
        public void InitializeGrid(T[] values)
        {
            if(values.Length > Columns * Rows)
            {
                Debug.LogError("Values array exceeds grid size.");
                return;
            }
            
            int count = 0;
            for (int i = 0; i < _gridList.Count; i++)
            {
                for(int j = 0; j < _gridList[0].Count; j++)
                {
                    if (count < values.Length)
                    {
                        _gridList[i][j] = values[count++];
                    }
                    else
                    {
                        _gridList[i][j] = null;
                    }
                }
            }
        }

        /// <summary>
        /// Initializes the grid with the specified value and fills it with a certain amount of that value.
        /// </summary>
        /// <param name="valueFactory"></param>
        /// <param name="onValueSet"></param>
        public void InitializeGrid(Func<T> valueFactory, Action<T> onValueSet = null)
        {
            if (valueFactory == null)
            {
                Debug.LogError("Cannot initialize grid: valueFactory is null.");
                return;
            }

            _gridList.Clear();

            for (int i = 0; i < Rows; i++)
            {
                var row = new List<T>(Columns);
                for (int j = 0; j < Columns; j++)
                {
                    T newValue = valueFactory();
                    row.Add(newValue);
                    onValueSet?.Invoke(newValue);
                }
                _gridList.Add(row);
            }
        }
        /// <summary>
        /// Sets the value at the specified column and row in the grid.
        /// </summary>
        /// <param name="column"></param>
        /// <param name="row"></param>
        /// <param name="value"></param>
        public void SetValue(int row, int column, T value)
        {
            if (column >= 0 || column < Columns || row >= 0 || row < Rows)
            {
                _gridList[row][column] = value;
            }
            Debug.LogError("Invalid grid position: " + row + ", " + column);
        }

        /// <summary>
        /// Expands the grid with new row and column amounts, preserving existing values.
        /// Will not shrink the grid if smaller dimensions are provided.
        /// </summary>
        /// <param name="newRowAmount">New number of rows (vertical size).</param>
        /// <param name="newColumnAmount">New number of columns (horizontal size).</param>
        /// <param name="defaultValue"> Value to populate new cells </param>
        /// <param name="populateNewCells">If true, fills new cells with null or a default value.</param>
        public void ExpandGrid(int newRowAmount, int newColumnAmount, T defaultValue = null, bool populateNewCells = true, Action<T> onValueSet = null)
        {
            int targetRows = Mathf.Max(Rows, newRowAmount);
            int targetColumns = Mathf.Max(Columns, newColumnAmount);
            
            for (int row = _gridList.Count; row < targetRows; row++)
            {
                var newRow = new List<T>(targetColumns);
                for (int col = 0; col < targetColumns; col++)
                {
                    newRow.Add(populateNewCells ? defaultValue : default);
                    onValueSet?.Invoke(newRow[col]);
                }
                _gridList.Add(newRow);
            }
            
            for (int row = 0; row < _gridList.Count; row++)
            {
                List<T> currentRow = _gridList[row];
                for (int col = currentRow.Count; col < targetColumns; col++)
                {
                    currentRow.Add(populateNewCells ? defaultValue : default);
                    onValueSet?.Invoke(currentRow[col]);
                }
            }
            
            Rows = targetRows;
            Columns = targetColumns;
        }
        
        /// <summary>
        /// Returns the index of the first occurrence of the specified value in the grid as a Vector2Int (row, column).
        /// </summary>
        /// <param name="value">Value from the grid</param>
        /// <returns></returns>
        public Vector2Int GetIndexOf(T value)
        {
            for (int i = 0; i < _gridList.Count; i++)
            {
                for (int j = 0; j < _gridList[i].Count; j++)
                {
                    if (_gridList[i][j] == value)
                    {
                        return new Vector2Int(i, j); // Return row, column as Vector2Int
                    }
                }
            }
            return new Vector2Int(-1, -1); // Not found
        }
        
        public T GetValue(T value)
        {
            for (int i = 0; i < _gridList.Count; i++)
            {
                for (int j = 0; j < _gridList[i].Count; j++)
                {
                    if (_gridList[i][j] == value)
                    {
                        return _gridList[i][j];
                    }
                }
            }
            return null;
        }

        public IEnumerator<T> GetEnumerator()
        {
            foreach (var row in _gridList)
            {
                foreach (var item in row)
                {
                    yield return item;
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}