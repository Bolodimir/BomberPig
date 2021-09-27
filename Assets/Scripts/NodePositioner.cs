using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodePositioner : MonoBehaviour
{
    [SerializeField] private Transform[] _corners;
    [SerializeField] private Vector2 _mapSizeInNodes;
    [SerializeField] private bool _drawGizmos;

    public Node[,] Nodes;
    public Node FindClosestNode(Vector2 position)
    {
        Node closest = Nodes[0, 0];
        float minDistance = Vector2.Distance(new Vector2(closest.x, closest.y), position);
        foreach (Node node in Nodes)
        {
            float distance = Vector2.Distance(new Vector2(node.x, node.y), position);
            if (distance < minDistance)
            {
                minDistance = distance;
                closest = node;
            }
        }
        return closest;
    }
    public Node GetRandomNode()
    {
        int x = Random.Range(0, Nodes.GetLength(0));
        int y = Random.Range(0, Nodes.GetLength(1));
        while (!Nodes[x, y].Empty)
        {
            x = Random.Range(0, Nodes.GetLength(0));
            y = Random.Range(0, Nodes.GetLength(1));
        }
        return Nodes[x, y];
    }
    private void Awake()
    {
        PlaceNodes();
    }
    private void PlaceNodes()
    {
        Nodes = new Node[(int)_mapSizeInNodes.x, (int)_mapSizeInNodes.y];

        Vector2[] leftBorder = LerpNodesIntoLine(_corners[0].position, _corners[1].position, (int)_mapSizeInNodes.y);
        Vector2[] rightBorder = LerpNodesIntoLine(_corners[3].position, _corners[2].position, (int)_mapSizeInNodes.y);

        for(int i = 0; i < (int)_mapSizeInNodes.y; i++)
        {
            Vector2[] line = LerpNodesIntoLine(leftBorder[i], rightBorder[i], (int)_mapSizeInNodes.x);
            for(int j = 0; j < (int)_mapSizeInNodes.x; j++)
            {
                Nodes[j, i] = new Node();
                Nodes[j, i].x = line[j].x;
                Nodes[j, i].y = line[j].y;
                Nodes[j, i].xGrid = j;
                Nodes[j, i].yGrid = i;
                Nodes[j, i].Empty = true;
                Nodes[j, i].distance = int.MaxValue;
            }
        }
    }
    private Vector2[] LerpNodesIntoLine(Vector2 first, Vector2 second, int nodeNumber) // returns an array of inserted nodes including first and second in a proper order
    {
        if (nodeNumber < 1) return new Vector2[0];

        Vector2 pivot = (second - first) / (nodeNumber - 1);
        Vector2[] result = new Vector2[nodeNumber];

        for(int i = 1; i < nodeNumber - 1; i++)
        {
            result[i] = first + pivot * (i);
        }
        result[0] = first;
        result[nodeNumber - 1] = second;
        return result;
    }
    private void OnDrawGizmos()
    {
        
        if (!Application.isPlaying) return;
        if (!_drawGizmos) return;
        
        foreach(Node node in Nodes)
        {
            if(node.Empty)
                Gizmos.color = Color.green;
            else
                Gizmos.color = Color.red;
            Gizmos.DrawCube(new Vector2(node.x, node.y), Vector3.one/4);
        }        
    }
}
public class Node
{
    public float x;
    public float y;
    public int xGrid;
    public int yGrid;
    public bool Empty;

    public Node parent;
    public bool isChecked;
    public float distance;
}

