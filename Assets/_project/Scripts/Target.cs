using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This script is attached to the target object, with a collider set to trigger in its child object and a particle system in the other child object
// It is used to detect when the target is hit by a sword
// When the target's trigger is entered, its particle system is played
// When the target's trigger is exited, its particle system is stopped
public class Target : MonoBehaviour
{
    // The particle system attached to the target
    public ParticleSystem particleSystem;

    // Start is called before the first frame update
    void Start()
    {
        // Stop the particle system
        particleSystem.Stop();
    }

    // When the target's trigger is entered
    private void OnTriggerEnter(Collider other)
    {
        // If the object that entered the trigger is a sword
        if (other.gameObject.CompareTag("Sword"))
        {
            // Play the particle system
            particleSystem.Play();
        }
    }

    // When the target's trigger is exited
    private void OnTriggerExit(Collider other)
    {
        // If the object that exited the trigger is a sword
        if (other.gameObject.CompareTag("Sword"))
        {
            // Stop the particle system
            particleSystem.Stop();
        }
    }
}
