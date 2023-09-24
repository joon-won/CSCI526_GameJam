using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CSCI526GameJam {

    [Serializable]
    public class Path {

        #region Fields
        [SerializeField] private List<Spot> spots = new();
        #endregion

        #region Publics
        public List<Spot> Spots => spots;

        public Path(Spot start, Spot end) {
            AStar(start, end);
        }
        #endregion

        #region Internals
        // TODO: Modify + optimize.
        private void AStar(Spot start, Spot end) {
            foreach (var spot in MapManager.Instance.Spots) {
                spot.ComputeDistanceToBase();
            }

            var spotToPre = new Dictionary<Spot, Spot>();

            var opens = new PriorityQueue<Spot>();
            var closeds = new HashSet<Spot>();

            opens.Enqueue(start, start.DistanceToBase);
            while (opens.Count > 0) {
                var spot = opens.Dequeue();
                closeds.Add(spot);
                for (int i = 0; i < 4; i++) {
                    var adjacent = spot.GetAdjacent((Direction)i);

                    if (!adjacent || closeds.Contains(adjacent)
                        || adjacent.Tower && adjacent.Tower is not PlayerBase) continue;

                    if (adjacent.Index == end.Index) {
                        spotToPre[adjacent] = spot;
                        var curr = adjacent;
                        while (curr) {
                            spots.Insert(0, curr);
                            if (!spotToPre.ContainsKey(curr)) break;

                            curr = spotToPre[curr];
                        }
                        return;
                    }
                    else if (!opens.Contains(adjacent)) {
                        opens.Enqueue(adjacent, adjacent.DistanceToBase);
                        spotToPre[adjacent] = spot;
                    }
                    else {
                        if (spot.DistanceToBase < adjacent.DistanceToBase) {
                            spotToPre[adjacent] = spot;
                            adjacent.DistanceToBase = spot.DistanceToBase;
                        }
                    }
                }
            }
        }
        #endregion

        #region Unity Methods
        #endregion

        public class PriorityQueue<T> {
            private List<Tuple<T, int>> items = new List<Tuple<T, int>>();

            public int Count => items.Count;

            public void Enqueue(T item, int priority) {
                items.Add(new Tuple<T, int>(item, priority));
                items.Sort((x, y) => x.Item2.CompareTo(y.Item2));
            }

            public T Dequeue() {
                if (items.Count == 0) {
                    throw new InvalidOperationException("Queue is empty.");
                }

                T item = items[0].Item1;
                items.RemoveAt(0);
                return item;
            }
            public bool Contains(T item) {
                foreach (var tuple in items) {
                    if (EqualityComparer<T>.Default.Equals(tuple.Item1, item))
                        return true;
                }
                return false;
            }
        }
    }
}
