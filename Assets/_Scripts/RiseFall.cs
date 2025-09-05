using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Gets the first transform in the children on the parent. 
/// </summary>

public class RiseFall : MonoBehaviour
{
    // Public
    // Protected
    // Private
    private GameObject player;
    private Transform mesh;
    private float heightMin = -20.0f, heightMax = 0.0f, rndDelay = 0.0f, checkRadius, smoothingFactor;
    private float heightOffset = -1.65f;
    private float distance;
    private bool playerNear;

    void Start()
    {
        foreach (Transform t in transform.GetComponentsInChildren<Transform>())
        {
            if (t != transform)
            {
                mesh = t;
                break;
            }
        }

        if (mesh == null)
        {
            Debug.Log("No child transform found: self-destructing.");
            Destroy(gameObject);
        }

        mesh.localPosition = new Vector3(0, heightMin, 0);

        // checkRadius = GetComponent<SphereCollider>().radius;

        if (smoothingFactor < 0.00001f && smoothingFactor > -0.0001f) smoothingFactor = 1.0f;
    }

    public void SetHeightMax(float newMax){
        heightMax = newMax;
    }

    public void SetHeightMin(float newMin){
        heightMin = newMin;
    }
    
    public void SetDelay(float newDelay)
    {
        rndDelay = newDelay;
    }

    public void SetCheckRadius(float newRadius)
    {
        checkRadius = newRadius;
    }

    public void SetHeightOffset(float newOffset)
    {
        heightOffset = newOffset;
    }

    public void SetPlayerRef(GameObject newPlayer)
    {
        player = newPlayer;
    }

    public void SetSmoothingFactor(float newSmoothingFactor)
    {
        smoothingFactor = newSmoothingFactor;
    }
/*
    private void OnTriggerEnter(Collider other)
    {
        if (player == null && other.tag == "Player")
        {
            player = other.gameObject;
        }

        if (other.tag == "Player") 
        {
            // Debug.Log("Player entered trigger zone.");
            playerNear = true;
            StartCoroutine(DistToPlayer());
            StartCoroutine(MoveBetween());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            // Debug.Log("Player exited trigger zone.");
            playerNear = false;
            StopCoroutine(MoveBetween());
            StopCoroutine(DistToPlayer());
        }
    }
*/  

    public void StartMovement()
    {
        playerNear = true;
        StartCoroutine(DistToPlayer());
        StartCoroutine(MoveBetween());
    }

    public void StopMovement()
    {
        playerNear = false;
        StopCoroutine(MoveBetween());
        StopCoroutine(DistToPlayer());
    }

    private IEnumerator MoveBetween()
    {
        yield return new WaitForSeconds(rndDelay);
        float dist;
        float recover;
        while (playerNear)
        {
            dist = distance / (smoothingFactor * checkRadius);
            if (Mathf.Abs(heightMax-mesh.localPosition.y) < 0.05f)
            {
                yield return new WaitForSeconds(rndDelay + 1.0f);
                recover = dist;
                do
                {
                    dist = Mathf.Clamp(distance / (smoothingFactor * checkRadius), 0.0f, 1.0f);
                    mesh.localPosition = new Vector3(0, Mathf.SmoothStep((heightMax), heightMin, recover), 0);
                    recover += Mathf.Clamp((dist - recover) * 0.05f, 0.0f, 1.0f);
                    yield return new WaitForFixedUpdate();
                } while (recover - dist < 0.0f);
            } else
            {
                mesh.localPosition = new Vector3(0, Mathf.SmoothStep((heightMax), heightMin, dist), 0);
                yield return new WaitForFixedUpdate();
            }
        }
    }

    private IEnumerator DistToPlayer()
    {
        Vector3 playerPos;
        // Vector3 objectPos;
        while (playerNear) 
        {
            playerPos = new Vector3(player.transform.position.x, player.transform.position.y + heightOffset, player.transform.position.z);
            // objectPos = new Vector3(transform.position.x, transform.position.y + mesh.GetComponent<Renderer>().bounds.extents.y / 2, transform.position.z);
            distance = Vector3.Distance(playerPos, transform.position); // of note is that this distance calculation is using the parent transform, not the mesh.
            // distance = Vector3.Distance(playerPos, transform.position*smoothingFactor);
            // spiky formula
            // distance = Mathf.Sqrt(Vector3.Magnitude((transform.position-playerPos)*smoothingFactor));
            yield return new WaitForFixedUpdate();
        }
    }
}
