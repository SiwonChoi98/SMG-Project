using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class CameraController : MonoBehaviour
{

    // �ϴ��� public���� ó���س���.
    public Transform target;
    public Vector3 Offset;

    void LateUpdate()
    {
        //���� target�� ����� ������ �ȵǾ��ٸ�, Player�� ã�Ƽ� �־��ش�.
        if (target == null)
        {
            GameObject go = GameObject.Find("Player");
            target = go.transform;
        }
        transform.position = target.position + Offset;
  
    }


}
