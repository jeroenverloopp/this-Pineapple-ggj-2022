using System;
using Food;
using Level;
using UnityEngine;
using Random = UnityEngine.Random;

public class FoodSpawner : MonoBehaviour
{

    [SerializeField] private int _berryCount;
    [SerializeField] private BaseFood _foodPrefab;

    private void Start()
    {
        var grid = LevelManager.Instance.Grid;
        for (int i = 0; i < _berryCount; i++)
        {
            
            Vector2 position = GetRandomPositionInLevel();
            Vector2Int gridPosition = grid.WorldToGridPosition(position);
            while (grid.InBounds(gridPosition) == false || grid[gridPosition.x, gridPosition.y].Walkable == false)
            {
                position = GetRandomPositionInLevel();
                gridPosition = grid.WorldToGridPosition(position);
            }

            var food = Instantiate(_foodPrefab);
            food.transform.position = position;
        }
    }


    private Vector2 GetRandomPositionInLevel()
    {
        var grid = LevelManager.Instance.Grid;
        float randX = Random.Range(grid.Position.x + .5f, grid.Position.x + grid.GridSize.x - .5f);
        float randY = Random.Range(grid.Position.y + .5f, grid.Position.y + grid.GridSize.y - .5f);
        return new Vector2(randX, randY);
    }
}
