using Map;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Pathfinding {
    public class PathCalculate
    {
        private Dictionary<Vector3Int, Node> nodes;
        private Heap<Node> openList;
        private HashSet<Node> closedList;

        public PathCalculate(Tilemap tilemap)
        {
            InitializeNodes(tilemap);
        }

        private void InitializeNodes(Tilemap tilemap)
        {
            nodes = new Dictionary<Vector3Int, Node>();
            foreach (Vector3Int position in tilemap.cellBounds.allPositionsWithin)
            {
                if (tilemap.HasTile(position))
                {
                    bool walkable = tilemap.GetTile(position).name != EnviromentType.Obstacle.ToString();
                    Debug.Log($"Node {position} is walkable: {walkable}");
                    nodes[position] = new Node(position, walkable);
                }
            }
        }

        public List<Vector3Int> FindPath(Vector3Int startPos, Vector3Int targetPos)
        {
            if (!nodes.ContainsKey(startPos) || !nodes.ContainsKey(targetPos))
            {
                return null; // If the start or target position is not on the tilemap, return null
            }

            Node startNode = nodes[startPos];
            Node targetNode = nodes[targetPos];

            openList = new Heap<Node>(nodes.Count);
            closedList = new HashSet<Node>();
            openList.Add(startNode);

            while (openList.Count > 0)
            {
                Node currentNode = openList.RemoveFirst();
                closedList.Add(currentNode);

                if (currentNode == targetNode)
                {
                    return RetracePath(startNode, targetNode);
                }

                foreach (Node neighbor in GetNeighbors(currentNode))
                {
                    if (!neighbor.walkable || closedList.Contains(neighbor)) continue;

                    int newCostToNeighbor = currentNode.gCost + GetDistance(currentNode, neighbor);
                    if (newCostToNeighbor < neighbor.gCost || !openList.Contains(neighbor))
                    {
                        neighbor.gCost = newCostToNeighbor;
                        neighbor.hCost = GetDistance(neighbor, targetNode);
                        neighbor.parent = currentNode;

                        if (!openList.Contains(neighbor))
                            openList.Add(neighbor);
                        else
                            openList.UpdateItem(neighbor);
                    }
                }
            }
            return null;
        }

        private List<Node> GetNeighbors(Node node)
        {
            List<Node> neighbors = new List<Node>();
            Vector3Int[] directions = {
            new Vector3Int(1, 0, 0),
            new Vector3Int(-1, 0, 0),
            new Vector3Int(0, 1, 0),
            new Vector3Int(0, -1, 0),
            new Vector3Int(1, 1, 0),
            new Vector3Int(-1, 1, 0),
            new Vector3Int(1, -1, 0),
            new Vector3Int(-1, -1, 0)
        };
            foreach (Vector3Int direction in directions)
            {
                Vector3Int neighborPos = node.position + direction;
                if (nodes.ContainsKey(neighborPos))
                {
                    neighbors.Add(nodes[neighborPos]);
                }
            }
            return neighbors;
        }

        private int GetDistance(Node a, Node b)
        {
            int dstX = Mathf.Abs(a.position.x - b.position.x);
            int dstY = Mathf.Abs(a.position.y - b.position.y);

            if (dstX > dstY)
                return 14 * dstY + 10 * (dstX - dstY);
            return 14 * dstX + 10 * (dstY - dstX);
        }

        private List<Vector3Int> RetracePath(Node startNode, Node endNode)
        {
            List<Vector3Int> path = new List<Vector3Int>();
            Node currentNode = endNode;

            while (currentNode != startNode)
            {
                path.Add(currentNode.position);
                currentNode = currentNode.parent;
            }
            path.Reverse();
            return path;
        }
    }
}
