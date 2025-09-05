using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderVisualizer : MonoBehaviour
{
    [Header("Gizmo Settings")]
    public Color color;
    public BoxCollider boxCollision;
    public MeshCollider meshCollision;

    void OnDrawGizmos()
    {
        Gizmos.color = new Color(color.r, color.g, color.b);
        if (boxCollision) Gizmos.DrawWireCube(transform.position, boxCollision.size);
        if (meshCollision) Gizmos.DrawWireCube(meshCollision.transform.position, meshCollision.bounds.size);
    }
}
