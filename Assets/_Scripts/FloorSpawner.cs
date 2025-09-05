using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

public class FloorSpawner : MonoBehaviour
{
    // Public
    [Header("Movement Values")]
    public float bottom;
    public float top, rndDelayMin, rndDelayMax;
    [Tooltip("This value should be set to half the height of the player. It is used to determine the floor position for the mesh movement. The value should be negative to represent a downward offset.")]
    public float offset; 
    public float smoothness;
    [Header("Prefab & Size")]
    public List<GameObject> columnPrefab = new();
    public float width;
    public float length;
    public Vector3 checkRadius;
    [Header("Gizmo Settings")]
    public Color gizmoColor;

    // Private
    private Vector2 floorExtents;
    private bool playerNear;
    private GameObject[,] floorTiles = new GameObject[101,101];
    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        BoxCollider tempRef = this.AddComponent<BoxCollider>();
        tempRef.size = checkRadius;
        tempRef.isTrigger = true;
        floorExtents.x = width;
        floorExtents.y = length;
        
        // Debug.Log("X: " + floorExtents.x + " Y: " +  floorExtents.y);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(gizmoColor.r, gizmoColor.g, gizmoColor.b);
        Gizmos.DrawWireCube(new Vector3(transform.position.x, transform.position.y, transform.position.z), new Vector3(width, 1, length));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!player && other.tag == "Player")
        {
            player = other.gameObject;
        }

        if (other.tag == "Player")
        {
            playerNear = true;
            StopCoroutine(DespawnLoop());
            StartCoroutine(SpawnLoop());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            playerNear = false;
            StopCoroutine(SpawnLoop());
            StartCoroutine(DespawnLoop());
        }
    }

    private IEnumerator SpawnLoop()
    {
        for (int i = 0; i < floorExtents.x; i++)
        {
            if (!playerNear) break;
            for (int j = 0; j < floorExtents.y; j++)
            {
                if (!playerNear) break;
                StartCoroutine(SpawnFloorMover(i, j));
                yield return new WaitForFixedUpdate();
            }
        }
    }

    private IEnumerator DespawnLoop()
    {
        for (int i = 0; i < floorExtents.x; i++)
        {
            if (playerNear) break;
            for (int j = 0; j < floorExtents.y; j++)
            {
                if (playerNear) break;
                if (floorTiles[i, j])
                {
                    floorTiles[i,j].GetComponent<RiseFall>().StopMovement();
                    Destroy(floorTiles[i, j]);
                }
                yield return new WaitForFixedUpdate();
            }
        }
    }

    private IEnumerator SpawnFloorMover(int i, int j)
    {
        if (columnPrefab == null) yield return null;
        // 0.5f is coming from the half-size of the floor tile itself.
        // This should instead derive the value from the mesh.
        // float posX = ((float)i - floorExtents.x / 2.0f - 0.5f);
        float posX = ((float)i - floorExtents.x / 2.0f + 0.5f);
        // float posY = ((float)j - floorExtents.y / 2.0f - 0.5f);
        float posY = ((float)j - floorExtents.y / 2.0f + 0.5f);

        if (floorTiles[i, j] == null)
        {
            int rnd = Random.Range(0, columnPrefab.Count);
            floorTiles[i,j] = Instantiate(columnPrefab[rnd], new Vector3(posX + transform.position.x, transform.position.y, posY + transform.position.z), Quaternion.identity);
        }

        RiseFall floorMover;
        floorMover = floorTiles[i,j].GetComponentInChildren<RiseFall>();

        if (floorMover)
        {
            floorMover.SetPlayerRef(player);
            floorMover.SetDelay(Random.Range(rndDelayMin, rndDelayMax));
            floorMover.SetHeightOffset(offset);
            floorMover.SetHeightMin(bottom);
            floorMover.SetHeightMax(top);
            floorMover.SetCheckRadius((width+length)/2.0f);
            floorMover.SetSmoothingFactor(smoothness);
            floorMover.StartMovement();
        }
        yield return null;
    }
}
