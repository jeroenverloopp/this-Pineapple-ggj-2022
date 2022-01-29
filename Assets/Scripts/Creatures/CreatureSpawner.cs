using System.Collections.Generic;
using Level;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Creatures
{
    public class CreatureSpawner : MonoBehaviour
    {
        [SerializeField] private List<BaseCreature> _spawnableCreatures;
        private int _selectedCreature;
        

        public void SpawnCreature(InputAction.CallbackContext context)
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