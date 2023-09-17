using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CSCI526GameJam {
    
    /// <summary>
    /// A generic matrix. 
    /// </summary>

    [Serializable]
    public class Matrix<T> : IEnumerable<T> {

        #region Fields
        [SerializeField] private T[,] data;
        #endregion

        #region Publics
        public int Width => data.GetLength(0);
        public int Height => data.GetLength(1);
        public int Count => data.Length;

        /// <summary>
        /// Default constructor. 
        /// </summary>
        public Matrix() {
            data = new T[0, 0];
        }

        /// <summary>
        /// Constructor. 
        /// </summary>
        /// <param name="width">Width of the matrix. </param>
        /// <param name="height">Height of the matrix. </param>
        public Matrix(int width, int height) {
            data = new T[width, height];
        }

        public T this[int x, int y] {
            get {
                return data[x, y];
            }
            set {
                data[x, y] = value;
            }
        }

        public T this[Vector2Int index] {
            get {
                return data[index.x, index.y];
            }
            set {
                data[index.x, index.y] = value;
            }
        }

        public IEnumerator<T> GetEnumerator() {
            return new Iterator(data);
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }
        #endregion

        #region Internals
        private class Iterator : IEnumerator<T> {
            private T[,] matrix;
            private Vector2Int index;

            public Iterator(T[,] matrix) {
                Reset();
                this.matrix = matrix;
            }

            public T Current => matrix[index.x, index.y];
            object IEnumerator.Current => Current;

            public bool MoveNext() {
                index.x++;
                if (index.x >= matrix.GetLength(0)) {
                    index.x = 0;
                    index.y++;
                }
                return index.y < matrix.GetLength(1);
            }

            public void Reset() {
                index = new Vector2Int(-1, 0);
            }

            public void Dispose() {
                // not needed
            }
        }
        #endregion
    }
}
