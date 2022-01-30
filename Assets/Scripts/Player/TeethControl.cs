using System.Collections.Generic;
using Audio;
using Core.Triggers;
using Creatures;
using Icons;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static UnityEngine.InputSystem.InputAction;

public class TeethControl : MonoBehaviour
{
    // Hunger + Eating
    private float _hunger = 50;
    [SerializeField] private float maxHunger;
    private float _eatTimer;
    public float timeToEat = 3f;

    //Input
    public PlayerInput playerInput;
    public float moveSpeed = 3f;

    //Sensoring
    private List<BaseCreature> fluffsInRange;
    private CircleTrigger _eatFluffTrigger;

    // Animation
    [SerializeField] private SpriteAnimationComponent _animation;

    
    private enum TeethState { Run, Idle, Eat}
    [SerializeField]
    private TeethState teethState;

    [SerializeField]
    private SpriteAnimation runAnimation, idleAnimation, eatAnimation;

    public float Hunger
    {
        get
        {
            return _hunger;
        }
        set
        {
            _hunger = Mathf.Clamp(value, 0, maxHunger);
        }
    }

    public float HungerIncrease;

    // Start is called before the first frame update
    void Start()
    {
        fluffsInRange = new List<BaseCreature>();
        _eatFluffTrigger = CircleTrigger.Create(1, "eatFluffTrigger", transform);
        _eatFluffTrigger.OnTriggerEnter += OnReachTriggerEnter;
        _eatFluffTrigger.OnTriggerExit += OnReachTriggerExit;
    }

    private void OnDestroy()
    {
        _eatFluffTrigger.OnTriggerEnter -= OnReachTriggerEnter;
        _eatFluffTrigger.OnTriggerExit -= OnReachTriggerExit;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (teethState != TeethState.Eat)
        {
            Vector2 playerInputMovement = playerInput.actions["Movement"].ReadValue<Vector2>();
            if (playerInputMovement.x != 0 || playerInputMovement.y != 0)
            {
                transform.position += new Vector3(playerInputMovement.x, playerInputMovement.y, 0) * moveSpeed * Time.deltaTime;
                SetState(TeethState.Run);

                DetermineRotation(playerInputMovement);
            }
            else
                SetState(TeethState.Idle);
        }
        else
        {
            _eatTimer -= Time.deltaTime;

            if (teethState == TeethState.Eat && _eatTimer <= 0)
            {
                FinishEating();
            }
        }

        Hunger -= HungerIncrease * Time.deltaTime;

        if (Hunger <= 0)
        {
            Debug.Log($"Game Over!");
        }
    }

    public void StartEating(CallbackContext context)
    {
        if (context.performed && teethState != TeethState.Eat && fluffsInRange.Count > 0)
        {
            SetState(TeethState.Eat);

            while (fluffsInRange.Count > 0)
            {
                _eatTimer = timeToEat;
                var fluff = fluffsInRange[0];
                fluffsInRange.RemoveAt(0);
                Hunger += fluff.creatureData.Nutrition;
                Destroy(fluff.gameObject);
                AudioManager.Instance.Play("TeethEat");
                IconManager.Instance.Create("Food", fluff.transform.position + Vector3.up * .2f);
            }
        }    
    }

    public void FinishEating()
    {
        SetState(TeethState.Idle);
    }

    public void OnReachTriggerEnter(Collider2D collider)
    {
        if (!collider.isTrigger)
        {
            if (collider.gameObject.tag == "Fluff")
            {
                fluffsInRange.Add(collider.gameObject.GetComponent<BaseCreature>());
                Debug.Log($"Added fluff");
            }
        }
    }

    public void OnReachTriggerExit(Collider2D collider)
    {
        if (!collider.isTrigger)
        {
            if (collider.gameObject.tag == "Fluff")
            {
                fluffsInRange.Remove(collider.gameObject.GetComponent<BaseCreature>());
                Debug.Log($"Removed fluff");
            }
        }
    }

    private void SetState(TeethState state)
    {
        if (teethState != state)
            SetAnimationChange(state);
        teethState = state;
    }

    private void SetAnimationChange(TeethState state)
    {
        switch (state)
        {
            case TeethState.Run:
                {
                _animation.SetAnimation(runAnimation);
                break; 
                }
            case TeethState.Idle:
                {
                    _animation.SetAnimation(idleAnimation);
                    break;
                }
               
            case TeethState.Eat:
                {
                    _animation.SetAnimation(eatAnimation);
                    break;
                }
        }
    }

    private void DetermineRotation(Vector2 movement)
    {
        var angle = Mathf.Atan2(movement.y, movement.x) * Mathf.Rad2Deg;
        angle += 90;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
}