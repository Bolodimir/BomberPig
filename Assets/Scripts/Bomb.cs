using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    [SerializeField] private float _timeToBoom;
    [SerializeField] private SpriteRenderer _sr;
    [SerializeField] private GameObject _effect;
    [SerializeField] private LayerMask _canBeKilled;

    private float _createTime;
    private Pathfinding _pf;
    private NodePositioner _np;
    private void Awake()
    {
        _createTime = Time.time;
    }
    private void Start()
    {
        _pf = FindObjectOfType<Pathfinding>();
        _np = FindObjectOfType<NodePositioner>();
    }
    private void Update()
    {
        if (Time.time > _createTime + _timeToBoom)
        {
            Boom();
        }

        Color tempColor = _sr.color;
        tempColor.g = 1 - ((Time.time - _createTime) / _timeToBoom);
        tempColor.b = 1 - ((Time.time - _createTime) / _timeToBoom);
        _sr.color = tempColor;
    }
    private void Boom()
    {
        Node closest = _np.FindClosestNode(transform.position);
        Node[] neighbours = _pf.FindNeighbours(closest.xGrid, closest.yGrid);
        Vector2[] positions = new Vector2[neighbours.Length];
        for(int i = 0; i < positions.Length; i++)
        {
            positions[i] = new Vector2(neighbours[i].x, neighbours[i].y);
        }

        foreach(Vector2 pos in positions)
        {
            Destroy(Instantiate(_effect, pos, transform.rotation), 2f);
            TryKill(pos);
        }
        Destroy(Instantiate(_effect, transform.position, transform.rotation),2f);
        TryKill(transform.position);
        
        Destroy(gameObject);
    }
    private void TryKill(Vector2 pos)
    {
        foreach(Collider2D col in Physics2D.OverlapCircleAll(pos, 0.35f, _canBeKilled))
        {
            if (col.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                col.GetComponent<Player>().Die();
            }
            if(col.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                col.GetComponent<Enemy>().Die();
            }
        }
    }
}
