using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// Sword tracks the movement of the sword and emits events
// Lunge is a sharp forward movement of the sword
// Pull is a sharp backward movement of the sword
// Strike is a collision with a target trigger
public class Sword : MonoBehaviour
{
    Vector3 oldPosition;
    Vector3 newPosition;
    Vector3 velocity;
    float xVelocity;
    public UnityEvent lunge;
    public UnityEvent pull;
    public UnityEvent block;
    Renderer rend;
    // public UnityEvent strike;
    // public UnityEvent strikeStart;
    // public UnityEvent strikeEnd;
    // public UnityEvent strikeCancel;
    // public UnityEvent strikeComplete;
    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponentInChildren<Renderer>();
        oldPosition = transform.position;
        newPosition = transform.position;
        velocity = Vector3.zero;
        xVelocity = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void FixedUpdate()
    {
        CalculateVelocity();
        if (xVelocity > 0.1f)
        {
            lunge.Invoke();
        } else if (xVelocity < -0.1f)
        {
            pull.Invoke();
        }
    }
    private void CalculateVelocity()
    {
        oldPosition = newPosition;
        newPosition = transform.position;
        velocity = newPosition - oldPosition;
        xVelocity = velocity.x;
    }
    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger detected");
        if (other.gameObject.tag == "Sword")
        {
            block.Invoke();
        }
    }
    public void EndCollision()
    {
        Debug.Log("Collision ended");
    }
}
