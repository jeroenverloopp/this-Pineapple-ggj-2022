using UnityEngine;
using UnityEngine.InputSystem;

public class CreatureMovementScript : MonoBehaviour
{
    public PlayerInput playerInput;
    private BaseCreature baseCreature;
    // Start is called before the first frame update
    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        baseCreature = GetComponent<BaseCreature>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        var input = playerInput.actions["Movement"].ReadValue<Vector2>();

        transform.position += new Vector3(input.x, input.y, 0) * baseCreature.moveSpeed * Time.deltaTime;
    }
}
