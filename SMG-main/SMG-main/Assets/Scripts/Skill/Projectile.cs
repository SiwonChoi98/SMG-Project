using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour // UpperSlash는 16.5 속도 , 0.7 제거
{
    public LayerMask TargetMask;

    [SerializeField]
    float speed = 1f;

    int damage = 1;

    [SerializeField]
    float DestroyTime = 1f;

    [SerializeField]
    GameObject hitFx;


    void Update()
    {

        transform.Translate(transform.forward * speed * Time.deltaTime, Space.World);

        Destroy(this.gameObject, DestroyTime);
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (1 << collision.transform.gameObject.layer == TargetMask)
        {
            //Debug.Log("collision.gameObject.layer : " + collision.gameObject.layer);
            collision.gameObject.GetComponent<IDamageable>()?.TakeDamage(damage, hitFx);
            
        }


    }

    public void SetDamage(int i)
    {
        damage = i;
    }

    public void SetTarget(LayerMask layerMask) // 
    {
        TargetMask = layerMask;
    }
}

