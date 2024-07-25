using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding {
    public class Node : IHeapItem<Node>
    {
        public Vector3Int position;
        public bool walkable;
        public Node parent;
        public int gCost;
        public int hCost;
        public int fCost => gCost + hCost;
        public int HeapIndex { get; set; }

        public Node(Vector3Int position, bool walkable)
        {
            this.position = position;
            this.walkable = walkable;
        }

        public int CompareTo(Node other)
        {
            int compare = fCost.CompareTo(other.fCost);
            if (compare == 0)
            {
                compare = hCost.CompareTo(other.hCost);
            }
            return -compare;
        }
    }
}

