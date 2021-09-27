using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClickListener : MonoBehaviour
{
    private Player _player;

    [SerializeField] private Camera _camera;

    public void SetPlayer(Player value)
    {
        _player = value;
    }
    private void OnMouseDown()
    {
        Vector2 worldPosition = _camera.ScreenToWorldPoint(Input.mousePosition);
        _player.OnPointerDown(worldPosition);
    }
    private void OnMouseDrag()
    {
        Vector2 worldPosition = _camera.ScreenToWorldPoint(Input.mousePosition);
        _player.OnPointerMove(worldPosition);
    }
    private void OnMouseUp()
    {
        Vector2 worldPosition = _camera.ScreenToWorldPoint(Input.mousePosition);
        _player.OnPointerUp(worldPosition);
    }
}
