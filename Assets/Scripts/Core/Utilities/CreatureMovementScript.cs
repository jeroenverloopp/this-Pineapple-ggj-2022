using UnityEngine;
using UnityEngine.InputSystem;

public class CreatureMovementScript : MonoBehaviour
{
    public PlayerInput playerInput;
    private BaseCreature baseCreature;

    private Vector2 _input;
    // Start is called before the first frame update
    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        baseCreature = GetComponent<BaseCreature>();
    }

    void Update()
    {
        _input = playerInput.actions["Movement"].ReadValue<Vector2>();
        transform.position += new Vector3(_input.x, _input.y, 0) * baseCreature.MoveSpeed * Time.deltaTime;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
        
    }
}
