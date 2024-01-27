using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorChangeTrigger : MonoBehaviour
{
    public Material material1;
    public Material material2;
    public MeshRenderer meshRenderer;
    // Start is called before the first frame update
    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    private void OnTriggerEnter(Collider other) {
        meshRenderer.material = material2;
    }
    private void OnTriggerExit(Collider other) {
        meshRenderer.material = material1;
    }
}
