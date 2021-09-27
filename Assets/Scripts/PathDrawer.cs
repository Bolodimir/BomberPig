using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathDrawer : MonoBehaviour
{
    [SerializeField] private GameObject _smallCircle;
    [SerializeField] private GameObject _bigCircle;

    private GameObject[] _circles;
    public void DrawPath(Vector2[] path)
    {
        if (path.Length == 0) return;
        if (_circles != null)
            DeletePath();

        _circles = new GameObject[path.Length];
        for(int i = 0; i < path.Length - 1; i++)
        {
            _circles[i] = Instantiate(_smallCircle, path[i], Quaternion.identity);
        }
        _circles[path.Length - 1] = Instantiate(_bigCircle, path[path.Length - 1], Quaternion.identity);
    }
    public void DeletePath()
    {
        if (_circles == null) return;
        foreach (GameObject circle in _circles)
        {
            Destroy(circle);
        }
    }
}
