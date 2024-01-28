using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionManager : MonoBehaviour
{
    public Transform vrSword;
    public Transform mouseSword;
    Transform recordedVRSword;
    GameObject vrSwordClone;
    Transform recordedMouseSword;
    GameObject mouseSwordClone;
    public Vector3 vrSwordPosition;
    public Vector3 mouseSwordPosition;  
    public Quaternion vrSwordRotation;
    public Quaternion mouseSwordRotation;
    public GameObject vrSwordPrefab;
    public GameObject mouseSwordPrefab;
    public GameObject planePrefab;
    Vector3 planePosition;
    Quaternion planeRotation;
    bool swordsAreBeyondTangentPlane = false;
    bool hasCollidedOnce = false;
    bool isColliding = false;
    // Checks if the swords are colliding
    public void SwordCollisionEnter() 
    {
        isColliding = true;
        swordsAreBeyondTangentPlane = true;
        // Get the transforms of the swords
        recordedVRSword = vrSword;
        recordedMouseSword = mouseSword;
        // Get the positions of the swords
        vrSwordPosition = vrSword.position;
        mouseSwordPosition = mouseSword.position;
        // Get the rotations of the swords
        vrSwordRotation = vrSword.rotation;
        mouseSwordRotation = mouseSword.rotation;
        // Calculate the tangent plane
        CalculateTangentPlane();
    }
    
    public void SwordCollisionStay() 
    {

    }
    // destroys the plane and sword clones upon collision exit
    public void SwordCollisionExit() 
    {
        Destroy(vrSwordClone);
        Destroy(mouseSwordClone);
        hasCollidedOnce = false;
        isColliding = false;

        Debug.Log("Collision Exit");
    }
    // Calculates the tangent plane between two transforms' y axis upon collision
    public void CalculateTangentPlane() 
    {
        // Get the normal vector of the plane
        Vector3 normal = Vector3.Cross(vrSword.up, mouseSword.up);
        // Get the tangent vector of the plane
        Vector3 tangent = Vector3.Cross(normal, vrSword.up);
        // Get the position of the plane
        planePosition = vrSword.position;
        // Get the rotation of the plane
        planeRotation = Quaternion.LookRotation(tangent, normal);
        // Spawn the plane and swords
        if (!hasCollidedOnce) 
        {
            SpawnPlane();
            SpawnSwords();
        }
    }
    void SpawnSwords() 
    {
        // Instantiate the swords
        vrSwordClone = Instantiate(vrSwordPrefab, vrSwordPosition, vrSwordRotation);
        mouseSwordClone = Instantiate(mouseSwordPrefab, mouseSwordPosition, mouseSwordRotation);
    }
    // Creates a plane at the point of collision
    void SpawnPlane() 
    {
        // Instantiate the plane
        GameObject plane = Instantiate(planePrefab, planePosition, planeRotation);
        hasCollidedOnce = true;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }
    // Checks if the swords are beyond the tangent plane
    void CheckSwordsBeyondTangentPlane() 
    {
        vrSwordPosition = vrSword.position;
        mouseSwordPosition = mouseSword.position;
        vrSwordRotation = vrSword.rotation;
        mouseSwordRotation = mouseSword.rotation;
        // Check if the swords are beyond the tangent plane
        if (vrSwordPosition.y < mouseSwordPosition.y || mouseSwordPosition.x < vrSwordPosition.x) 
        {
            swordsAreBeyondTangentPlane = true;
        } else 
        {
            swordsAreBeyondTangentPlane = false;
        }
    }
    // Update is called once per frame
    void Update()
    {
        if(isColliding) 
        {
            // Check if the swords are beyond the tangent plane
            CheckSwordsBeyondTangentPlane();
            // If the swords are beyond the tangent plane, destroy the plane and sword clones
            if (swordsAreBeyondTangentPlane) 
            {
                Destroy(vrSwordClone);
                Destroy(mouseSwordClone);
                isColliding = false;
                swordsAreBeyondTangentPlane = false;
            }
        }
    }
}
