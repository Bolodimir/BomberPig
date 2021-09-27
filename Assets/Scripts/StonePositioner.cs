using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StonePositioner : MonoBehaviour
{
    [SerializeField] private NodePositioner _np;
    private void Start()
    {
        foreach(Transform stone in transform)
        {
            Node closestNode = _np.FindClosestNode(stone.position);
            stone.position = new Vector2(closestNode.x, closestNode.y);
            closestNode.Empty = false;
        }
    }
}
