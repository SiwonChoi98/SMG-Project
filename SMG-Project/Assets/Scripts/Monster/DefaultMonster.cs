using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultMonster : Monster
{
    // Start is called before the first frame update

    void Start()
    {
        target = GameObject.Find("Player").transform;
    }

    // Update is called once per frame
    protected override void Update()
    {
        GetComponent<UnityEngine.AI.NavMeshAgent>().SetDestination(target.position);
    }
}
