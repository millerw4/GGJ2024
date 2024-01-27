using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MousePlayer : MonoBehaviour
{
    public float speed = 1f;
    bool isMoving = false;
    Vector2 mousePosition;
    Vector2 mousePositionOld;
    public float maxXResolution;
    public float maxYResolution;
    float mouseNormalizedX;
    float mouseNormalizedY;
    public Transform sword;
    // Start is called before the first frame update
    void Start()
    {
        var mouse = Mouse.current;
        mousePosition = mouse.position.ReadValue();
        mousePositionOld = mousePosition;
        if (maxXResolution == 0)
        {
            maxXResolution = Screen.currentResolution.width;
        }
        if (maxYResolution == 0)
        {
            maxYResolution = Screen.currentResolution.height;
        }
    }

    // Update is called once per frame
    void Update()
    {
        var mouse = Mouse.current;
        if (mouse.leftButton.wasPressedThisFrame)
        {
            OnLeftClick();
        }
        if (mouse.rightButton.wasPressedThisFrame)
        {
            OnRightClick();
        }
        mousePositionOld = mousePosition;
        mousePosition = mouse.position.ReadValue();
        if (mousePosition.x < 0)
        {
            mousePosition.x = 0;
        }
        if (mousePosition.x > maxXResolution)
        {
            mousePosition.x = maxXResolution;
        }
        if (mousePosition.y < 0)
        {
            mousePosition.y = 0;
        }   
        if (mousePosition.y > maxYResolution)
        {
            mousePosition.y = maxYResolution;
        }
        mouseNormalizedX = mousePosition.x / maxXResolution;
        mouseNormalizedY = mousePosition.y / maxYResolution;
        sword.localPosition = new Vector3(mouseNormalizedX - 0.5f, 0, mouseNormalizedY + 0.5f);
    }
    void LogMousePosition()
    {
        if (mousePositionOld != mousePosition)
        {
            Debug.Log(mousePosition);
            Debug.Log(maxXResolution);
            Debug.Log(maxYResolution);
        }
    }
    void OnLeftClick()
    {
        Advance();
    }
    void OnRightClick()
    {
        Retreat();
    }
    void Advance()
    {
        if (isMoving)
        {
            return;
        } else
        {
            isMoving = true;
            StartCoroutine(Move(transform.position + Vector3.left * speed));
        }
    }
    void Retreat()
    {
        if (isMoving)
        {
            return;
        }
        else
        {
            isMoving = true;
            StartCoroutine(Move(transform.position + Vector3.right * speed));
        }
    }
    IEnumerator Move(Vector3 destination)
    {
        while (transform.position != destination)
        {
            transform.position = Vector3.MoveTowards(transform.position, destination, speed * 2 * Time.deltaTime);
            yield return null;
        }
        isMoving = false;
    }
}
