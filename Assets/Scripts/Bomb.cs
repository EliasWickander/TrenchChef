using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public float explosionRadius = 5;
    private MeshRenderer meshRenderer;

    private bool exploding = false;
    private float explodeTimer = 0;

    private void Awake()
    {
        meshRenderer = GetComponentInChildren<MeshRenderer>();
    }

    private void Update()
    {
        if (exploding)
        {
            if (explodeTimer < 1)
            {
                explodeTimer += Time.deltaTime;
            }
            else
            {
                explodeTimer = 0;
                exploding = false;
                Destroy(gameObject);
            }
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, explosionRadius, LayerMask.GetMask("Player"));

        exploding = true;
        meshRenderer.enabled = false;
        
        if (hits.Length > 0)
        {
            PlayerController playerController = hits[0].GetComponentInParent<PlayerController>();
            
            playerController.Kill();
        }
    }

    private void OnDrawGizmos()
    {
        if (exploding)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, explosionRadius);
        }
    }
}