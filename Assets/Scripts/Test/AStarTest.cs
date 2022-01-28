using Level;
using Test;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;


public class AStarTest : MonoBehaviour
{

    [SerializeField] private LayerMask _wallsMask;
    [SerializeField] private Unit _testUnitPrefab;
    [SerializeField] private Transform _target;
    private LevelAStarGrid _grid;

    private void Awake()
    {
        _grid = new LevelAStarGrid(new Vector2(-50,-50), new Vector2(100, 100), new Vector2Int(100, 100));
        _grid.SetWalkableByCollision(_wallsMask, 2);
        _grid.SetNeighbours();
    }

    private void CreateUnit()
    {
        if (_grid == null)
        {
            Debug.LogError("Define Grid before Creating a search request");
        }

        Vector3 startPosition = new Vector3(Random.Range(-50, 50), Random.Range(-50, 50));
        if(Physics2D.OverlapCircle(startPosition, 2, _wallsMask))
        {
            return;
        }
        
        Unit unit = Instantiate(_testUnitPrefab, startPosition , Quaternion.identity);
        unit.SetTarget(_grid, _target);
    }

    public void CreateSUnit(CallbackContext context)
    {
        if(context.performed)
        {
            for (int i = 0; i < 1; i++)
            {
                CreateUnit();
            }
        }
    }

    public void CreateAUnit(CallbackContext context)
    {
        if (context.performed)
        {
            for (int i = 0; i < 10; i++)
            {
                CreateUnit();
            }
        }
    }
}
