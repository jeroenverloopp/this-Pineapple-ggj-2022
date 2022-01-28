using System.Collections.Generic;
using PathFinding.AStar;
using Test;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;
using Grid = PathFinding.AStar.Grid;


public class AStarTest : MonoBehaviour
{

    [SerializeField] private LayerMask _wallsMask;
    [SerializeField] private Unit _testUnitPrefab;
    [SerializeField] private Transform _target;
    private Grid _grid;

    private void Awake()
    {
        _grid = new Grid(Vector2.zero, new Vector2(100, 100), new Vector2Int(100, 100));
        _grid.SetWalkableByCollision(_wallsMask, 2);
    }

    private void CreateUnit()
    {
        if (_grid == null)
        {
            Debug.LogError("Define Grid before Creating a search request");
        }

        Vector3 startPosition = new Vector3(UnityEngine.Random.Range(0, 100), 1 , UnityEngine.Random.Range(0, 100));
        if (Physics.CheckSphere(startPosition, 2, _wallsMask))
        {
            return;
        }
        
        Unit unit = Instantiate(_testUnitPrefab, startPosition , Quaternion.identity);
        unit.SetTarget(_grid, _target);
    }

    public void CreateSUnit(CallbackContext context)
    {
        if(context.performed)
            for (int i = 0; i < 1; i++)
            {
                CreateUnit();
            }
    }

    public void CreateAUnit(CallbackContext context)
    {
        if(context.performed)
            for (int i = 0; i < 10; i++)
            {
                CreateUnit();
            }
    }
}
