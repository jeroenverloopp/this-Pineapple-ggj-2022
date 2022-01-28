using Level;
using Test;
using UnityEngine;


public class AStarTest : MonoBehaviour
{

    [SerializeField] private LayerMask _wallsMask;
    [SerializeField] private Unit _testUnitPrefab;
    [SerializeField] private Transform _target;
    private LevelAStarGrid _aStarGrid;

    private void Awake()
    {
        _aStarGrid = new LevelAStarGrid(new Vector2(-50,-50), new Vector2(100, 100), new Vector2Int(100, 100));
        _aStarGrid.SetWalkableByCollision(_wallsMask, 2);
        _aStarGrid.SetNeighbours();
    }

    private void CreateUnit()
    {
        if (_aStarGrid == null)
        {
            Debug.LogError("Define Grid before Creating a search request");
        }

        Vector3 startPosition = new Vector2(Random.Range(-50, 50), Random.Range(-50, 50));
        if (Physics2D.OverlapCircle(startPosition, 2.2f, _wallsMask))
        {
            return;
        }
        
        Unit unit = Instantiate(_testUnitPrefab, startPosition , Quaternion.identity);
        unit.SetTarget(_aStarGrid, _target);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            for (int i = 0; i < 1; i++)
            {
                CreateUnit();
            }
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            for (int i = 0; i < 10; i++)
            {
                CreateUnit();
            }
        }
    }
}
