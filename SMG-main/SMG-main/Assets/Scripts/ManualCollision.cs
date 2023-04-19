using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManualCollision : MonoBehaviour 
{
    [SerializeField]
    private Vector3 boxSize = new Vector3(1.5f, 2.5f, 2.5f);

    public Collider[] CheckOverlapBox(LayerMask layerMask)
    {
        return Physics.OverlapBox(transform.position, boxSize * 0.5f, transform.rotation, layerMask);
    }
    private void OnDrawGizmos()
    {
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(Vector3.zero, boxSize);
    }
}

