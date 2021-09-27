using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerChaser : Enemy
{
    [SerializeField] private float _checkPeriod;
    [SerializeField] private CharacterMovement _cm;
    [SerializeField] private GameObject _bloodEffect;
    [SerializeField] private SpriteRenderer _sr;

    private Pathfinding _pf;
    private Transform _player;
    private float _lastChecked;
    private bool _dead;
    private GameState _gs;
    private void Start()
    {
        _dead = true;
        _player = FindObjectOfType<Player>().transform;
        _pf = FindObjectOfType<Pathfinding>();
        _lastChecked = -_checkPeriod;
        _gs = FindObjectOfType<GameState>();
        _gs.StartGame += Activate;
        _gs.RegisterEnemy();
    }
    private void Activate()
    {
        _dead = false;
    }
    private void Update()
    {
        if (_dead) return;
        if (_lastChecked + _checkPeriod < Time.time)
        {
            SetPathToPlayer();
        }
    }
    private void SetPathToPlayer()
    {
        if (_player == null) return;
        Vector2[] path = _pf.FindPath(transform.position, _player.position);
        _cm.SetPath(path);
    }
    public override void Die()
    {
        if (!_dead)
        {
            _gs.EnemyKilled();
            Destroy(Instantiate(_bloodEffect, transform.position, transform.rotation), 2f);
            StartCoroutine(DieAnimation());
            _dead = true;
            _cm.Stop();
            Destroy(GetComponentInChildren<Collider2D>());
        }
    }
    IEnumerator DieAnimation()
    {
        float JumpSpeed = 1;
        float JumpTime = 0.5f;

        float FallSpeed = 1;
        float FallTime = 0.5f;

        float RotationAngle = -90;
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
