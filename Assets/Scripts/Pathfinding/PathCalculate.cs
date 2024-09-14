using Config;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Pathfinding
{
    
    public class PathCalculate
    {
        private Heap<Node> openList;
        private HashSet<Node> closedList;

        private readonly TileConfig tileConfig;
        private readonly Tilemap tilemap;
        private readonly Dictionary<Vector3Int, Node> nodes;
        
        //List<Node> neighbors;
        private readonly Vector3Int[] directions =
        {
            new Vector3Int(1, 0, 0),
            new Vector3Int(-1, 0, 0),
            new Vector3Int(0, 1, 0),
            new Vector3Int(0, -1, 0),
            new Vector3Int(1, 1, 0),
            new Vector3Int(-1, 1, 0),
            new Vector3Int(1, -1, 0),
            new Vector3Int(-1, -1, 0)
        };
        
        public PathCalculate(Tilemap tilemap)
        {
            this.tilemap = tilemap;
            tileConfig = Resources.Load<TileConfig>("TileConfig");
            nodes = new Dictionary<Vector3Int, Node>();
            closedList = new HashSet<Node>();
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
            var startNode = GetNode(startPos);
            var targetNode = GetNode(targetPos);

            if (startNode == null || targetNode == null)
            {
                return null; // If the start or target position is not valid
            }

            openList = new Heap<Node>(GetSearchSize(startPos, targetPos));
            closedList.Clear();
            openList.Add(startNode);

            while (openList.Count > 0)
            {
                var currentNode = openList.RemoveFirst();
                closedList.Add(currentNode);

                if (currentNode == targetNode)
                {
                    return RetracePath(startNode, targetNode);
                }

                for (var i = 0; i < directions.Length; i++)
                {
                    var neighbor = GetNode(currentNode.position + directions[i]);
                    if (neighbor == null || !neighbor.walkable || closedList.Contains(neighbor)) continue;

                    var newCostToNeighbor = currentNode.gCost + GetDistance(currentNode, neighbor);
                    if (newCostToNeighbor >= neighbor.gCost && openList.Contains(neighbor)) 
                        continue;
                    
                    neighbor.gCost = newCostToNeighbor;
                    neighbor.hCost = GetDistance(neighbor, targetNode);
                    neighbor.parent = currentNode;

                    if (!openList.Contains(neighbor))
                        openList.Add(neighbor);
                    else
                        openList.UpdateItem(neighbor);
                }
            }

            return null;
        }
        
        private int GetSearchSize(Vector3Int startPos, Vector3Int targetPos)
        {
            var minX = Mathf.Min(startPos.x, targetPos.x);
            var minY = Mathf.Min(startPos.y, targetPos.y);
            var maxX = Mathf.Max(startPos.x, targetPos.x);
            var maxY = Mathf.Max(startPos.y, targetPos.y);

            var width = maxX - minX + 1;
            var height = maxY - minY + 1;

            return width * height; // Total number of cells in the chunk
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
