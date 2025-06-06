using System.Collections.Generic;
using Unity.Collections;
using UnityEngine.Serialization;

namespace NPG.Codebase.Game.Gameplay.UI.Windows.Equipment.Components
{
    using UnityEngine;
    using System;

    [Serializable]
    public class Matrix2D<T> : Matrix2D
    {
        
        [SerializeField]
        private T[] data;

        public T this[int row, int col]
        {
            get => data[row * cols + col];
            set => data[row * cols + col] = value;
        }
        
        public void Resize(int newRows, int newCols)
        {
            T[] newData = new T[newRows * newCols];
            for (int r = 0; r < Mathf.Min(rows, newRows); r++)
            {
                for (int c = 0; c < Mathf.Min(cols, newCols); c++)
                {
                    newData[r * newCols + c] = this[r, c];
                }
            }
            rows = newRows;
            cols = newCols;
            data = newData;
        }
        
        public void IndexOf(T item, out int row, out int col)
        {
            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < cols; c++)
                {
                    if (EqualityComparer<T>.Default.Equals(this[r, c], item))
                    {
                        row = r;
                        col = c;
                        return;
                    }
                }
            }
            row = -1;
            col = -1;
        }
    }
    
    [Serializable]
    public class Matrix2D
    {
        public int rows;
        
        public int cols;
        
        public Matrix2D(int rows = 1, int cols = 1)
        {
            this.rows = rows;
            this.cols = cols;
        }
    }
}