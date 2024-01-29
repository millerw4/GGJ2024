using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Locomotion : MonoBehaviour
{
    public Transform origin;
    public Sword sword;
    public float speed = 1.0f;
    public float positiveThreshold = 0.1f;
    bool isMoving = true;


    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DelayCanMove());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Advance()
    {
        if (!isMoving)
        {
            StartCoroutine(Move(origin.position, origin.position + new Vector3(speed, 0.0f, 0.0f), speed));
        }
    }  
    public void Retreat()
    {
        if (!isMoving)
        {
            StartCoroutine(Move(origin.position, origin.position - new Vector3(speed, 0.0f, 0.0f), speed));
        }
    }
    IEnumerator DelayCanMove()
    {
        yield return new WaitForSeconds(1.5f);
        isMoving = false;
    }
    IEnumerator Move(Vector3 start, Vector3 destination, float speed)
    {
        isMoving = true;
        float startTime = Time.time;
        float distance = Vector3.Distance(start, destination);
        float duration = distance / 3.0f;
        while (Time.time - startTime < duration)
        {
            origin.position = Vector3.Lerp(start, destination, (Time.time - startTime) / duration);
            yield return null;
        }
        origin.position = destination;
        isMoving = false;
    }
}
