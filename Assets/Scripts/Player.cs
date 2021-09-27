using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public delegate void PlayerDieHandler();
    public event PlayerDieHandler PlayerDie;

    [SerializeField] private LayerMask _enemyMask;
    [SerializeField] private CharacterMovement _cm;
    [SerializeField] private PathDrawer _pd;
    [SerializeField] private GameObject _bomb;
    [SerializeField] private float _bombDropPeriod = 1;
    [SerializeField] private SpriteRenderer _sr;
    [SerializeField] private GameObject _bloodEffect;

    private Pathfinding _pf;
    private NodePositioner _np;
    private bool _dead = false;
    private float _lastDropped;
    public void DropBomb()
    {
        if (Time.time <= _lastDropped + _bombDropPeriod) return;

        GameObject bomb = Instantiate(_bomb);
        Node closest = _np.FindClosestNode(transform.position);
        Vector2 pos = new Vector2(closest.x, closest.y);
        bomb.transform.position = pos;

        _lastDropped = Time.time;
    }
    public void OnPointerDown(Vector2 worldPosition)
    {
        if (_dead) return;
        Vector2[] path = _pf.FindPath(transform.position, worldPosition);
        _pd.DrawPath(path);
    }
    public void OnPointerMove(Vector2 worldPosition)
    {
        if (_dead) return;
        Vector2[] path = _pf.FindPath(transform.position, worldPosition);
        _pd.DrawPath(path);
    }
    public void OnPointerUp(Vector2 worldPosition)
    {
        if (_dead) return;
        Vector2[] path = _pf.FindPath(transform.position, worldPosition);
        _cm.SetPath(path);
        _pd.DeletePath();
    }
    public void Die()
    {
        _pd.DeletePath();
        if (_dead) return;
        PlayerDie?.Invoke();
        _dead = true;
        _cm.Stop();
        Destroy(Instantiate(_bloodEffect, transform.position, transform.rotation), 2f);
        StartCoroutine(DieAnimation());
    }
    private void Start()
    {
        _dead = true;
        FindObjectOfType<ClickListener>().SetPlayer(this);
        _pf = FindObjectOfType<Pathfinding>();
        _np = FindObjectOfType<NodePositioner>(); 
        FindObjectOfType<GameState>().StartGame += Activate;
    }
    private void Activate()
    {
        _dead = false;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(1<<collision.gameObject.layer == _enemyMask)
        {
            Die();
        }
    }
    IEnumerator DieAnimation()
    {
        float FallSpeed = 1;
        float FallTime = 0.5f;

        float JumpSpeed = 1;
        float JumpTime = 0.5f;

        float RotationAngle = -180;
        float RotationSpeed = RotationAngle / JumpTime;

        float curTime = Time.time;
        while (curTime + JumpTime > Time.time)
        {
            transform.position += Vector3.up * JumpSpeed * Time.deltaTime;
            transform.Rotate(Vector3.forward * RotationSpeed * Time.deltaTime);
            yield return null;
        }
        curTime = Time.time;
        while (curTime + FallTime > Time.time)
        {
            transform.position += Vector3.down * FallSpeed * Time.deltaTime;
            yield return null;
        }

        float fadeTime = 3;
        float fadeSpeed = 1 / fadeTime;
        while (_sr.color.a > 0)
        {
            Color tempColor = _sr.color;
            tempColor.a -= fadeSpeed * Time.deltaTime;
            _sr.color = tempColor;
            yield return null;
        }
        Destroy(gameObject, 0.5f);
        yield break;
    }
}
