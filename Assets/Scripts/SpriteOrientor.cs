using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteOrientor : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _sr;
    [SerializeField] private ClickCollider _cc;
    [SerializeField] private Sprite _up;
    [SerializeField] private Sprite _down;
    [SerializeField] private Sprite _left;
    [SerializeField] private Sprite _right;

    private Vector2 _direction;
    private float[] _angles;
    private Sprite[] _sprites;
    private void Awake()
    {
        _angles = new float[] { -135, -45, 45, 135, 180 };
        _sprites = new Sprite[] { _down, _right, _up, _left, _down };
    }
    public void SetDirection(Vector2 dir)
    {
        _direction = dir;

        float angle = Vector2.SignedAngle(Vector2.up, _direction);
        Sprite sprite = null;
        for(int i = 0; i < _angles.Length; i++)
        {
            if(angle < _angles[i])
            {
                sprite = _sprites[i];
                break;
            }
        }
        _sr.sprite = sprite;
        
        _cc?.AdjustColliderToSprite();
    }
}
