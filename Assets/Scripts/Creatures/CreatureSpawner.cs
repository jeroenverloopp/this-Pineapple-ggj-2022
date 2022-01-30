using System;
using System.Collections.Generic;
using Level;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

namespace Creatures
{
    public class CreatureSpawner : MonoBehaviour
    {
        private List<BaseCreature> _spawnableCreatures;
        private int _selectedCreature;

        [SerializeField] private BaseCreature _creaturePrefab;
        [SerializeField] private int _spawnCount = 10;

        private void Start()
        {

            for (int i = 0; i < _spawnCount; i++)
            {
                SpawnCreatureOnRandomPosition(_creaturePrefab);
            }
        }


        private void SpawnCreatureOnRandomPosition(BaseCreature spawnedCreature)
        {
            var grid = LevelManager.Instance.Grid;
            float xRandom = Random.Range(grid.Position.x, grid.Position.x + grid.GridSize.x);
            float yRandom = Random.Range(grid.Position.y, grid.Position.y + grid.GridSize.y);
            Vector2Int gridPos = LevelManager.Instance.Grid.WorldToGridPosition(xRandom,yRandom);
            
            while (LevelManager.Instance.Grid.InBounds(gridPos) == false ||
                   LevelManager.Instance.Grid[gridPos.x, gridPos.y].Walkable == false)
            {
                xRandom = Random.Range(grid.Position.x, grid.Position.x + grid.GridSize.x);
                yRandom = Random.Range(grid.Position.y, grid.Position.y + grid.GridSize.y);
                gridPos = LevelManager.Instance.Grid.WorldToGridPosition(xRandom,yRandom);
            }
            
            BaseCreature creature = Instantiate(spawnedCreature);
            creature.transform.position = new Vector2(xRandom,yRandom);
            
        }
        
        public void OnSpawnCreature(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                
                Vector2 position = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
                Vector2Int gridPos = LevelManager.Instance.Grid.WorldToGridPosition(position);
                if (LevelManager.Instance.Grid.InBounds(gridPos) && LevelManager.Instance.Grid[gridPos.x,gridPos.y].Walkable)
                {
                    BaseCreature creature = Instantiate(_spawnableCreatures[_selectedCreature]);
                    creature.transform.position = position;
                }
            }
        }


        public void PreviousCreature(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                _selectedCreature--;
                if (_selectedCreature < 0)
                {
                    _selectedCreature = _spawnableCreatures.Count - 1;
                }

                Debug.Log($"Spawn creature = {_spawnableCreatures[_selectedCreature]}");
            }
        }
        
        public void NextCreature(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                _selectedCreature++;
                if (_selectedCreature >= _spawnableCreatures.Count)
                {
                    _selectedCreature = 0;
                }

                Debug.Log($"Spawn creature = {_spawnableCreatures[_selectedCreature]}");
            }
        }
        
    }
}