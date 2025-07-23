using System.Collections.Generic;
using UnityEngine;

public class PathfindingGrid : MonoBehaviour
{
   private Dictionary<Vector3, PathfindingNode> nodes = new Dictionary<Vector3, PathfindingNode>();

   public void CreateNode(Vector3 position)
   {
       if (!nodes.ContainsKey(position))
       {
           nodes[position] = new PathfindingNode();
       }
   }

   public PathfindingNode GetNode(Vector3 position)
   {
       nodes.TryGetValue(position, out var node);
       return node;
   }
}
