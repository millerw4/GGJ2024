using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

// creates a clone of the swords at the point of collision and disables the renderer of the original swords
// until the swords are back on their original sides of the tangent plane
// at which point the original swords are enabled and the clones are destroyed
// 1) create a clone of the swords at the point of collision
// 2) disable the renderer of the original swords
// 3) check if the swords are back on their respective sides of the tangent plane
// 4) if the swords are back on their respective sides of the tangent plane, enable the renderer of the original swords
// 5) destroy the clones
// 6) destroy the plane
// 7) if the swords are not back on their respective sides of the tangent plane, repeat steps 3-7

public class CollisionManager : MonoBehaviour
{
    public Transform vrSword;
    public Transform mouseSword;
    public GameObject vrSwordPrefab;
    public GameObject mouseSwordPrefab;
    private GameObject vrSwordClone;
    private GameObject mouseSwordClone;
    private Vector3 planeNormal;
    private Vector3 planePosition;
    private Quaternion vrSwordInitialRotation;
    private Quaternion mouseSwordInitialRotation;
    private bool isColliding = false;

    public GameObject planePrefab;
    private GameObject planeClone;

    // Called when the swords start colliding
    public void SwordCollisionStart() 
    {
        Debug.Log("Collision Start");

        if (isColliding) 
        {
            Debug.Log("Already colliding, returning");
            return;
        }

        isColliding = true;

        CalculateTangentPlane();

        // Create clones of the swords at the point of collision using the prefabs
        vrSwordClone = Instantiate(vrSwordPrefab, vrSword.position, vrSword.rotation);
        mouseSwordClone = Instantiate(mouseSwordPrefab, mouseSword.position, mouseSword.rotation);

        // Create a clone of the plane at the point of collision using the prefab
        Quaternion planeRotation = Quaternion.FromToRotation(Vector3.up, planeNormal);
        planeClone = Instantiate(planePrefab, planePosition, planeRotation);

        // Disable the renderers of the original swords
        foreach (Renderer renderer in vrSword.GetComponentsInChildren<Renderer>())
        {
            renderer.enabled = false;
        }
        foreach (Renderer renderer in mouseSword.GetComponentsInChildren<Renderer>())
        {
            renderer.enabled = false;
        }

        // Start the coroutine
        StartCoroutine(CheckSwordsPosition());
    }

    // Called when the swords stop colliding
    public void SwordCollisionExit() 
    {
        Debug.Log("Collision Exit");
        if (AreSwordsOnRespectiveSides())
        {
            Debug.Log("Swords are on respective sides, enabling renderers");
            // Enable the renderers of the original swords
            foreach (Renderer renderer in vrSword.GetComponentsInChildren<Renderer>())
            {
                renderer.enabled = true;
            }
            foreach (Renderer renderer in mouseSword.GetComponentsInChildren<Renderer>())
            {
                renderer.enabled = true;
            }

            Debug.Log("Destroying clones");
            // Destroy the clones
            Destroy(vrSwordClone);
            Destroy(mouseSwordClone);

            // Destroy the plane
            Destroy(planeClone);

            isColliding = false;
        }
    }

    // Calculates the tangent plane between two transforms' y axis upon collision
    //1) Convert the z vectors from the local space to the world space
    //2) Get the normal vector of the plane
    //3) Get the position of the plane
    //4) Store the initial rotations of the swords
    public void CalculateTangentPlane() 
    {
        // Convert the z vectors from the local space to the world space
        Vector3 vrSwordZ = vrSword.TransformDirection(Vector3.forward);
        Vector3 mouseSwordZ = mouseSword.TransformDirection(Vector3.forward);

        // Get the normal vector of the plane
        planeNormal = Vector3.Cross(vrSwordZ, mouseSwordZ);

        // Get the position of the plane at the midpoint between the swords
        planePosition = (vrSword.position + mouseSword.position) / 2;

        // Draw a debug line to show the normal vector of the plane
        Debug.DrawRay(planePosition, planeNormal, Color.red, 10);

        // Store the initial rotations of the swords
        vrSwordInitialRotation = vrSword.rotation;
        mouseSwordInitialRotation = mouseSword.rotation;
    }

    // Checks if the swords are back on their respective sides of the tangent plane
    //1) Calculate the positions of the swords as if they had their initial rotations
    //2) Calculate the distance between the swords
    //3) Calculate the dot product of the vectors from the plane position to the sword positions and the plane normal
    //4) If the dot products have different signs, the swords are on their respective sides of the plane
    private bool AreSwordsOnRespectiveSides()
    {
        // Calculate the distance between the swords
        float distance = Vector3.Distance(vrSword.position, mouseSword.position);

        // If the distance is less than the threshold, return false
        float threshold = 0.5f; // Set the threshold to the desired value
        if (distance < threshold)
        {
            Debug.Log("Distance between swords is less than threshold, returning");
            return false;
        }

        // Calculate the positions of the swords as if they had their initial rotations
        Vector3 vrSwordPosition = planePosition + vrSwordInitialRotation * Vector3.forward * distance / 2;
        Vector3 mouseSwordPosition = planePosition + mouseSwordInitialRotation * Vector3.forward * distance / 2;

        // Calculate the dot product of the vectors from the plane position to the sword positions and the plane normal
        Vector3 vrSwordVector = vrSwordPosition - planePosition;
        Vector3 mouseSwordVector = mouseSwordPosition - planePosition;
        float vrSwordDot = Vector3.Dot(vrSwordVector, planeNormal);
        float mouseSwordDot = Vector3.Dot(mouseSwordVector, planeNormal);
        Debug.Log("vrSwordDot: " + vrSwordDot);
        Debug.Log("mouseSwordDot: " + mouseSwordDot);
        Debug.Log("vrSwordDot * mouseSwordDot: " + vrSwordDot * mouseSwordDot);
        Debug.Log("(Exits on TRUE) vrSwordDot * mouseSwordDot < 0: " + (vrSwordDot * mouseSwordDot < 0));

        // If the dot products have different signs, the swords are on their respective sides of the plane
        return vrSwordDot * mouseSwordDot < 0;
    }

    private IEnumerator CheckSwordsPosition()
    {
        Debug.Log("Checking swords position every 0.1 seconds");
        // Keep checking until the swords are back on their respective sides of the tangent plane
        while (!AreSwordsOnRespectiveSides())
        {
            yield return new WaitForSeconds(0.1f); // Wait for 0.1 seconds before the next check
        }

        Debug.Log("Swords are on respective sides, exiting collision");
        // Call SwordCollisionExit when the swords are back on their respective sides of the tangent plane
        SwordCollisionExit();
    }
    // public Transform vrSword;
    // public Transform mouseSword;
    // Transform recordedVRSword;
    // GameObject vrSwordClone;
    // Transform recordedMouseSword;
    // GameObject mouseSwordClone;
    // public Vector3 vrSwordPosition;
    // public Vector3 mouseSwordPosition;  
    // public Quaternion vrSwordRotation;
    // public Quaternion mouseSwordRotation;
    // public GameObject vrSwordPrefab;
    // public GameObject mouseSwordPrefab;
    // public GameObject planePrefab;
    // Vector3 planePosition;
    // Quaternion planeRotation;
    // bool swordsAreBeyondTangentPlane = false;
    // bool hasCollidedOnce = false;
    // bool isColliding = false;
    // bool vrSwordxIsGreater = false;
    // bool vrSwordyIsGreater = false;
    // // Checks if the swords are colliding
    // public void SwordCollisionEnter() 
    // {
    //     isColliding = true;
    //     swordsAreBeyondTangentPlane = true;
    //     // Get the transforms of the swords
    //     recordedVRSword = vrSword;
    //     recordedMouseSword = mouseSword;
    //     // Get the positions of the swords
    //     vrSwordPosition = vrSword.position;
    //     mouseSwordPosition = mouseSword.position;
    //     // Get the rotations of the swords
    //     vrSwordRotation = vrSword.rotation;
    //     mouseSwordRotation = mouseSword.rotation;
    //     if (vrSwordPosition.x > mouseSwordPosition.x) 
    //     {
    //         vrSwordxIsGreater = true;
    //     } else 
    //     {
    //         vrSwordxIsGreater = false;
    //     }
    //     if (vrSwordPosition.y > mouseSwordPosition.y) 
    //     {
    //         vrSwordyIsGreater = true;
    //     } else 
    //     {
    //         vrSwordyIsGreater = false;
    //     }
    //     // Calculate the tangent plane
    //     CalculateTangentPlane();
    //     SpawnSwords();
    // }
    
    // public void SwordCollisionStay() 
    // {

    // }
    // // destroys the plane and sword clones upon collision exit
    // public void SwordCollisionExit() 
    // {
    //     Destroy(vrSwordClone);
    //     Destroy(mouseSwordClone);
    //     hasCollidedOnce = false;
    //     isColliding = false;

    //     Debug.Log("Collision Exit");
    // }
    // // Calculates the tangent plane between two transforms' y axis upon collision
    // public void CalculateTangentPlane() 
    // {
    //     // Get the normal vector of the plane
    //     Vector3 normal = Vector3.Cross(vrSword.up, mouseSword.up);
    //     // Get the tangent vector of the plane
    //     Vector3 tangent = Vector3.Cross(normal, vrSword.up);
    //     // Get the position of the plane
    //     planePosition = vrSword.position;
    //     // Get the rotation of the plane
    //     planeRotation = Quaternion.LookRotation(tangent, normal);
    //     // Spawn the plane and swords
    //     if (!hasCollidedOnce) 
    //     {
    //         SpawnPlane();
    //     }
    // }
    // void SpawnSwords() 
    // {
    //     // Instantiate the swords
    //     vrSwordClone = Instantiate(vrSwordPrefab, vrSwordPosition, vrSwordRotation);
    //     mouseSwordClone = Instantiate(mouseSwordPrefab, mouseSwordPosition, mouseSwordRotation);
    // }
    // // Creates a plane at the point of collision
    // void SpawnPlane() 
    // {
    //     // Instantiate the plane
    //     GameObject plane = Instantiate(planePrefab, planePosition, planeRotation);
    //     hasCollidedOnce = true;
    // }
    // // Start is called before the first frame update
    // void Start()
    // {
        
    // }
    // // Checks if the swords are beyond the tangent plane
    // void CheckSwordsBeyondTangentPlane() 
    // {
    //     vrSwordPosition = vrSword.position;
    //     mouseSwordPosition = mouseSword.position;
    //     vrSwordRotation = vrSword.rotation;
    //     mouseSwordRotation = mouseSword.rotation;
    //     // Check if the swords are beyond the tangent plane
    //     if (vrSwordxIsGreater && vrSwordyIsGreater) 
    //     {
    //         if (vrSwordPosition.x < mouseSwordPosition.x && vrSwordPosition.y < mouseSwordPosition.y) 
    //         {
    //             swordsAreBeyondTangentPlane = false;
    //         }
    //     } else if (!vrSwordxIsGreater && vrSwordyIsGreater) 
    //     {
    //         if (vrSwordPosition.x > mouseSwordPosition.x && vrSwordPosition.y < mouseSwordPosition.y) 
    //         {
    //             swordsAreBeyondTangentPlane = false;
    //         }
    //     } else if (vrSwordxIsGreater && !vrSwordyIsGreater) 
    //     {
    //         if (vrSwordPosition.x < mouseSwordPosition.x && vrSwordPosition.y > mouseSwordPosition.y) 
    //         {
    //             swordsAreBeyondTangentPlane = false;
    //         }
    //     } else if (!vrSwordxIsGreater && !vrSwordyIsGreater) 
    //     {
    //         if (vrSwordPosition.x > mouseSwordPosition.x && vrSwordPosition.y > mouseSwordPosition.y) 
    //         {
    //             swordsAreBeyondTangentPlane = false;
    //         }
    //     }
    // }
    // // Update is called once per frame
    // void Update()
    // {
    //     if(isColliding) 
    //     {
    //         // Check if the swords are beyond the tangent plane
    //         CheckSwordsBeyondTangentPlane();
    //         // If the swords are beyond the tangent plane, destroy the plane and sword clones
    //         if (!swordsAreBeyondTangentPlane) 
    //         {
    //             Destroy(vrSwordClone);
    //             Destroy(mouseSwordClone);
    //             isColliding = false;
    //             swordsAreBeyondTangentPlane = false;
    //         }
    //     }
    // }
}
