using UnityEngine;
using UnityEngine.Events;

public class ColliderProxy : MonoBehaviour
{

    public UnityEvent<Collision2D> OnCollisionEnter2DAction;
    public UnityEvent<Collision2D> OnCollisionStay2DAction;
    public UnityEvent<Collision2D> OnCollisionExit2DAction;

    public UnityEvent<Collider2D> OnTriggerEnter2DAction;
    public UnityEvent<Collider2D> OnTriggerStay2DAction;
    public UnityEvent<Collider2D> OnTriggerExit2DAction;
    // Start is called before the first frame update
    private void OnCollisionEnter2D(Collision2D collision)
    {
        OnCollisionEnter2DAction.Invoke(collision);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        OnCollisionStay2DAction.Invoke(collision);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        OnCollisionExit2DAction.Invoke(collision);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        OnTriggerEnter2DAction.Invoke(collision);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        OnTriggerStay2DAction.Invoke(collision);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        OnTriggerExit2DAction.Invoke(collision);
    }
}
