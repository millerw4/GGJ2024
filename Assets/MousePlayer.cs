using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MousePlayer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
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
    }
    void OnLeftClick()
    {
        Debug.Log("Left Click");
    }
    void OnRightClick()
    {
        Debug.Log("Right Click");
    }
}
