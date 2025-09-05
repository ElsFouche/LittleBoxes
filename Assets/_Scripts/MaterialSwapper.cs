using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialSwapper : MonoBehaviour
{
    [Header("Materials")]
    [SerializeField] private List<Material> newMaterial = new();

    private Material baseMaterial;

    private void Awake()
    {
        baseMaterial = GetComponent<Material>();
        if (baseMaterial == null)
        {
            Debug.Log("No base material: self-destructing.");
            Destroy(this);
        }
    }

    public void SwapMaterial(int materialSelection = 0)
    {
        if (newMaterial.Count == 0)
        {
            Debug.Log("No new materials found.");
            return;
        } else {
            baseMaterial = new Material(newMaterial[materialSelection]);
        }
    }
}