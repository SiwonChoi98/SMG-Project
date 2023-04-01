using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManualCollision : MonoBehaviour
{
    [SerializeField]
    private Vector3 boxSize = new Vector3(1.5f, 2.5f, 2.5f);

    [SerializeField]
    private Vector3 boxOffset = new Vector3(0f, 1.2f, 1.1f); // ���� ��Ÿ��� �°� �ణ�� �������� �����ش�.
    
    public Collider[] CheckOverlapBox(LayerMask layerMask)
    {
        return Physics.OverlapBox(transform.position + boxOffset, boxSize);
    }
    private void OnDrawGizmos()
    {
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(Vector3.zero, boxSize);
    }
}
