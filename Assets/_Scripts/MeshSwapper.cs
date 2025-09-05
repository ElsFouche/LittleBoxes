using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(MeshFilter))]

public class MeshSwapper : MonoBehaviour
{

    [Header("Swap Mesh")]
    [SerializeField] private List<Mesh> newMesh = new();
    // [Header("Starting Mesh")]
    private MeshFilter baseMesh;

    private void Awake()
    {
        baseMesh = GetComponent<MeshFilter>();
    }

    public void SwapMeshes(int meshIndex)
    {
        if (newMesh.Count == 0)
        {
            Debug.Log("No meshes to swap to."); return;
        }
        else {
            baseMesh.mesh = newMesh[meshIndex];
        }
    }
}
