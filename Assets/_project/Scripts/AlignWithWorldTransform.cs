using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AlignWithWorldTransform : MonoBehaviour
{
    Vector3 initialPosition;
    Vector3 oldPosition;
    Vector3 newPosition;
    Vector3 velocity;
    float magnitude;
    public TextMeshProUGUI text;
    public float positiveThreshold = 0.1f;
    public ParticleSystem particles;
    public Transform origin;
    public float speed = 1.0f;
    bool isMoving = false;
    // private Sword sword;

    // Start is called before the first frame update
    void Start()
    {
        // sword = GetComponentInParent<Sword>();
        initialPosition = transform.position;
        oldPosition = initialPosition;
        newPosition = initialPosition;
        velocity = Vector3.zero;
        magnitude = 0.0f;
    }
    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.identity; 

    }

    void FixedUpdate()
    {
        oldPosition = newPosition;
        newPosition = transform.position;
        velocity = newPosition - oldPosition;
        magnitude = velocity.magnitude;
        float xVelocity = velocity.x;
        text.text = xVelocity.ToString().Substring(0, 4);
        if (xVelocity > positiveThreshold)
        {
            particles.Play();
            // sword.Lunge();
            
        } else if (xVelocity < -positiveThreshold)
        {
            particles.Play();
            // sword.Pull();
        }
    }
    // IEnumerator Move(Vector3 start, Vector3 destination, float speed)
    // {
    //     isMoving = true;
    //     float startTime = Time.time;
    //     float distance = Vector3.Distance(start, destination);
    //     float duration = distance / 3.0f;
    //     while (Time.time - startTime < duration)
    //     {
    //         origin.position = Vector3.Lerp(start, destination, (Time.time - startTime) / duration);
    //         yield return null;
    //     }
    //     origin.position = destination;
    //     isMoving = false;
    // }
}
