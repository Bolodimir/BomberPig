using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClickCollider : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _sr;
    [SerializeField] private BoxCollider2D _col;
    [SerializeField] private Player _player;
    private void OnMouseDown()
    {
        _player.DropBomb();
    }
    public void AdjustColliderToSprite()
    {
        Destroy(_col);
        _col = _sr.gameObject.AddComponent<BoxCollider2D>();
    }
}
