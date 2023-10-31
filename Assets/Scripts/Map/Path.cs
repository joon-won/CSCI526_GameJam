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

        private HashSet<Spot> extraBlocks = new();
        #endregion

        #region Publics
        public List<Spot> Spots => spots;

        public Path(Spot start, Spot end, HashSet<Spot> extraBlocks = null) {
            if (extraBlocks != null) {
                this.extraBlocks = new(extraBlocks);
            }
            AStar(start, end);
        }

        public bool IsValid() {
            for (int i = 0; i < spots.Count; i++) {
                var spot = spots[i];
                if (IsSpotBlocked(spot)) return false;
            }
            return true;
        }

        public bool IsSpotBlocked(Spot spot) {
            return spot.Tower && spot.Tower is not PlayerBase;
        }
        #endregion

        #region Internals
        private int Manhattan(Spot start, Spot end) {
            return Mathf.Abs(start.Index.x - end.Index.x) + Mathf.Abs(start.Index.y - end.Index.y);
        }

        private void AStar(Spot start, Spot end) {
            var allSpots = MapManager.Instance.Spots;
            var nodes = new Matrix<Node>(allSpots.Width, allSpots.Height);
            foreach (var spot in allSpots) {
                var node = new Node();
                node.spot = spot;
                node.g = 0;
                node.h = Manhattan(spot, end);
                node.f = node.h;
                node.parent = null;

                nodes[spot.Index] = node;
            }

            var opens = new PriorityQueue<Node>();
            var closeds = new HashSet<Node>();

            var startNode = nodes[start.Index];
            opens.Enqueue(startNode, startNode.f);

            var adjacents = new List<Vector2Int>();
            while (opens.Count > 0) {
                var currNode = opens.Dequeue();
                closeds.Add(currNode);

                var x = currNode.spot.Index.x;
                var y = currNode.spot.Index.y;
                adjacents.Clear();
                adjacents.AddRange(new[] {
                    new Vector2Int(x - 1, y),
                    new Vector2Int(x + 1, y),
                    new Vector2Int(x, y - 1),
                    new Vector2Int(x, y + 1)
                });

                for (int i = 0; i < 4; i++) {
                    var index = adjacents[i];
                    if (index.x < 0 || index.x >= nodes.Width
                        || index.y < 0 || index.y >= nodes.Height)
                        continue;

                    var adjacent = nodes[index];
                    if (closeds.Contains(adjacent)
                        || extraBlocks.Contains(adjacent.spot)
                        || IsSpotBlocked(adjacent.spot))
                        continue;

                    var cost = currNode.g + 1;
                    if (index == end.Index) {
                        nodes[end.Index].parent = currNode;
                        var curr = adjacent;
                        while (curr != null) {
                            spots.Insert(0, curr.spot);
                            curr = curr.parent;
                        }
                        return;
                    }

                    if (!opens.Contains(adjacent)) {
                        adjacent.g = cost;
                        adjacent.f = adjacent.h + adjacent.g;
                        opens.Enqueue(adjacent, adjacent.f);
                        adjacent.parent = currNode;
                    }
                    else {
                        if (cost < adjacent.g) {
                            //adjacent.parent = currNode;
                            //adjacent.g = currNode.g;
                            opens.Remove(adjacent);
                        }
                    }
                }
            }
        }
        #endregion

        #region Unity Methods
        #endregion

        private class Node {
            public Spot spot;
            public int g;
            public int h;
            public int f;
            public Node parent;
        }
    }
}
