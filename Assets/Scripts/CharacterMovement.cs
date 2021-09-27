using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    [SerializeField] private SpriteOrientor _so;
    [SerializeField] private float _distanceMargin; //how far char. needs to be to be considered at the point
    [SerializeField] private float _movSpeed;

    private Vector2[] _movPoints;
    private int _curIndex;
    private Vector2 _curDirection;
    private bool _isMoving = false;
    private void Update()
    {
        if (!_isMoving) return;

        Vector2 direction = _movPoints[_curIndex] - (Vector2)transform.position;
        direction.Normalize();
        transform.position = transform.position + (Vector3)direction * _movSpeed * Time.deltaTime;
        if (IsAtSpot()) 
            SetNewPoint();
    }
    public void Stop()
    {
        _isMoving = false;
    }
    public void SetPath(Vector2[] path)
    {
        if (path.Length == 0) return;
        _movPoints = path;
        _curIndex = 0;
        _isMoving = true;
        SetNewPoint();
    }
    public bool IsMoving()
    {
        return _isMoving;
    }
    private void SetNewPoint()
    {
        _curIndex++;
        if(_curIndex < _movPoints.Length)
        {
            _curDirection = _movPoints[_curIndex] - (Vector2)transform.position;
            _curDirection.Normalize();
            _so.SetDirection(_curDirection);
        }
        else
        {
            _curDirection = Vector2.zero;
            _isMoving = false;
        }
    }
    private bool IsAtSpot()
    {        
        return Vector2.Distance(transform.position, _movPoints[_curIndex]) < _distanceMargin;
    }
    
}
