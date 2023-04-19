using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public LayerMask TargetMask;

    [SerializeField]
    float speed = 1;

    int damage = 1;

    [SerializeField]
    GameObject hitFx;


    void Update()
    {

        transform.Translate(transform.forward * speed * Time.deltaTime, Space.World);

        Destroy(this.gameObject, 2f);
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

