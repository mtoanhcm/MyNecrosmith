using Config;
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

        private TileConfig tileConfig;
        private Tilemap tilemap;

        public PathCalculate(Tilemap tilemap)
        {
            this.tilemap = tilemap;
            tileConfig = Resources.Load<TileConfig>("TileConfig");
            nodes = new Dictionary<Vector3Int, Node>();
        }

        private Node GetNode(Vector3Int position)
        {
            if (!nodes.ContainsKey(position))
            {
                if (tilemap.HasTile(position))
                {
                    var tileType = tileConfig.GetTileType(tilemap.GetTile(position));
                    bool walkable = tileType != Tile.TileType.Blocker && tileType != Tile.TileType.None;
                    nodes[position] = new Node(position, walkable);
                }
                else
                {
                    return null; // No tile at this position
                }
            }
            return nodes[position];
        }

        public List<Vector3Int> FindPath(Vector3Int startPos, Vector3Int targetPos)
        {
            Node startNode = GetNode(startPos);
            Node targetNode = GetNode(targetPos);

            if (startNode == null || targetNode == null)
            {
                return null; // If the start or target position is not valid
            }

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
                    if (neighbor == null || !neighbor.walkable || closedList.Contains(neighbor)) continue;

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
                Node neighbor = GetNode(node.position + direction);
                if (neighbor != null)
                {
                    neighbors.Add(neighbor);
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
