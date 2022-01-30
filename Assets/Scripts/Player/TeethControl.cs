using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static UnityEngine.InputSystem.InputAction;

public class TeethControl : MonoBehaviour
{
    public Text hungerText;

    public PlayerInput playerInput;

    public float moveSpeed = 3f;

    private List<BaseCreature> fluffsInRange;

    public float Hunger = 50;

    public float HungerIncrease;

    private 

    // Start is called before the first frame update
    void Start()
    {
        fluffsInRange = new List<BaseCreature>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector2 _input = playerInput.actions["Movement"].ReadValue<Vector2>();
        transform.position += new Vector3(_input.x, _input.y, 0) * moveSpeed * Time.deltaTime;

        Hunger -= HungerIncrease * Time.deltaTime;

        hungerText.text = $"Hunger: {Hunger}";

        if (Hunger <= 0)
        {
            Debug.Log($"Game Over!");
        }
    }

    public void Eat(CallbackContext context)
    {
        if (context.performed)
        {
            while (fluffsInRange.Count > 0)
            {
                var fluff = fluffsInRange[0];
                fluffsInRange.RemoveAt(0);
                Hunger += fluff.creatureData.Nutrition;
                Destroy(fluff.gameObject);
            }
        }    
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



}
