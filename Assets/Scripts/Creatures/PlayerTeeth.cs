using System;
using System.Collections.Generic;
using Audio;
using Core.Triggers;
using Creatures.Behaviour;
using Icons;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Creatures
{
    public class PlayerTeeth : BaseCreature
    {

        [SerializeField] private PlayerInput playerInput;
        
        private CircleTrigger _eatTrigger;
        private float _eatTimer;
        private List<FluffCreature> _fluffsInRange;
        protected override void Awake()
        {
            //base.Awake();
            _eatTrigger = CircleTrigger.Create(1,"EatTrigger", transform);
            _eatTrigger.OnTriggerEnter += OnEatTriggerEnter;
            _eatTrigger.OnTriggerExit += OnEatTriggerExit;

            MoveSpeed = creatureData.MoveSpeed;
            Hunger = creatureData.StartHunger;
            _fluffsInRange = new List<FluffCreature>();
            SetState(BehaviourState.Idle);
        }

        private void OnDestroy()
        {
            _eatTrigger.OnTriggerEnter -= OnEatTriggerEnter;
            _eatTrigger.OnTriggerExit -= OnEatTriggerExit;
        }

        protected override void Start()
        {
            //base.Start();
        }

        protected override void Update()
        {
            base.Update();
        }
        
        public void StartEating(InputAction.CallbackContext context)
        {
            if (context.performed && State != BehaviourState.Eating && _fluffsInRange.Count > 0)
            {
                SetState(BehaviourState.Eating);

                while (_fluffsInRange.Count > 0)
                {
                    var fluff = _fluffsInRange[0];
                    _eatTimer = Mathf.Max(_eatTimer , fluff.creatureData.TimeToGetEaten);
                    
                    Hunger = Mathf.Max(Hunger - fluff.creatureData.Nutrition , 0);
                    
                    AudioManager.Instance.Play("TeethEat");
                    IconManager.Instance.Create("Food", fluff.transform.position + Vector3.up * .2f);
                    
                    _fluffsInRange.RemoveAt(0);
                    Destroy(fluff.gameObject);
                }
            }    
        }
        
        void FixedUpdate()
        {
            if (State != BehaviourState.Eating)
            {
                Vector2 playerInputMovement = playerInput.actions["Movement"].ReadValue<Vector2>();
                if (playerInputMovement.x != 0 || playerInputMovement.y != 0)
                {
                    transform.position += new Vector3(playerInputMovement.x, playerInputMovement.y, 0) * MoveSpeed * Time.deltaTime;
                    SetState(BehaviourState.Roaming);

                    DetermineRotation(playerInputMovement);
                }
                else
                    SetState(BehaviourState.Idle);
            }
            else
            {
                
                _eatTimer -= Time.deltaTime;

                if (State == BehaviourState.Eating && _eatTimer <= 0)
                {
                    FinishEating();
                }
            }

            Hunger += creatureData.HungerGained * Time.deltaTime;
            //hungerText.text = $"Hunger: {Hunger}";

            if (Hunger >= creatureData.MaxHunger)
            {
                Debug.Log($"Game Over!");
            }
        }
        
        private void FinishEating()
        {
            SetState(BehaviourState.Idle);
        }
        
        private void DetermineRotation(Vector2 movement)
        {
            var angle = Mathf.Atan2(movement.y, movement.x) * Mathf.Rad2Deg;
            angle += 90;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
        
        private void OnEatTriggerEnter(Collider2D collider)
        {
            if (!collider.isTrigger)
            {
                FluffCreature fc = collider.gameObject.GetComponent<FluffCreature>();
                if (fc != null)
                {
                    _fluffsInRange.Add(fc);
                }
            }
        }

        private void OnEatTriggerExit(Collider2D collider)
        {
            if (!collider.isTrigger)
            {
                FluffCreature fc = collider.gameObject.GetComponent<FluffCreature>();
                if (fc != null)
                {
                    _fluffsInRange.Remove(fc);
                }
            }
        }
    }
}