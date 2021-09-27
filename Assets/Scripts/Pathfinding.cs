using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//[ExecuteInEditMode]
public class Pathfinding : MonoBehaviour
{
    [SerializeField] private NodePositioner _np;

    private Node[,] _nodes;
    
    public Vector2[] FindPath(Vector2 start, Vector2 end)
    {
        Vector2[] result = Dijkstra(_np.FindClosestNode(start), _np.FindClosestNode(end));
        foreach (Node node in _nodes)
        {
            node.distance = int.MaxValue;
            node.isChecked = false;
            node.parent = null;
        }

        return result; ;
    }
    public Node[] FindNeighbours(int x, int y)
    {
        int numberOfNodes = 0;
        Node[] result = new Node[4];
        if (x - 1 >= 0 && _nodes[x - 1, y].Empty)
        {
            result[numberOfNodes] = _nodes[x - 1, y];
            numberOfNodes++;
        }
        if (x + 1 < _nodes.GetLength(0) && _nodes[x + 1, y].Empty)
        {
            result[numberOfNodes] = _nodes[x + 1, y];
            numberOfNodes++;
        }
        if (y - 1 >= 0 && _nodes[x, y - 1].Empty)
        {
            result[numberOfNodes] = _nodes[x, y - 1];
            numberOfNodes++;
        }
        if (y + 1 < _nodes.GetLength(1) && _nodes[x, y + 1].Empty)
        {
            result[numberOfNodes] = _nodes[x, y + 1];
            numberOfNodes++;
        }
        Node[] endResult = new Node[numberOfNodes];
        for (int i = 0; i < numberOfNodes; i++)
        {
            endResult[i] = result[i];
        }
        return endResult;
    }
    private void Start()
    {
        _nodes = _np.Nodes;
    }    
    private Vector2[] Dijkstra(Node start, Node end)
    {
        if (!start.Empty || !end.Empty) return new Vector2[0];
        List<Node> notChecked = new List<Node>();
        start.distance = 0;
        notChecked.Add(start);
        Node current = notChecked[0];
        do
        {            
            Node[] neighbours = FindNeighbours(current.xGrid, current.yGrid);

            foreach (Node node in neighbours)
            {
                if (!notChecked.Contains(node) && !node.isChecked) notChecked.Add(node);
                if (node.distance > current.distance + 1)
                {
                    node.distance = current.distance + 1;
                    node.parent = current;
                }
            }

            current.isChecked = true;
            notChecked.Remove(current);
            if (notChecked.Count == 0) return new Vector2[0];

            current = notChecked[0];
            float minDistance = notChecked[0].distance;
            foreach (Node node in notChecked)
            {
                if (node.distance < minDistance)
                {
                    minDistance = node.distance;
                    current = node;
                }
            }
        } while (current != end);

        return GetPathWithParents(current);
    }
    private Vector2[] GetPathWithParents(Node end)
    {
        int numberOfNodes = 1;
        Node current = end;
        while(current.parent != null)
        {
            current = current.parent;
            numberOfNodes++;
        }

        Vector2[] result = new Vector2[numberOfNodes];
        current = end;
        for (int i = numberOfNodes - 1; i >= 0; i--)
        {
            result[i] = new Vector2(current.x, current.y);
            current = current.parent;
        }
        //print(result.Length);
        return result;
    }    
}
