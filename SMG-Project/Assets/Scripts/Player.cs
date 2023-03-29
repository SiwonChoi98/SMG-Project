using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Camera camera;
    private Animator animator;

    private bool isMove;
    private Vector3 destination;
    private float speed;

    private void Awake()
    {
        camera = Camera.main;
        animator = GetComponentInChildren<Animator>();
        speed = 5.0f;
    }

    void Start()
    {
    }

    void Update()
    {
        CheckDestination();
        Move();
    }

    private void CheckDestination()
    {
        if (Input.GetMouseButton(1))
        {
            RaycastHit hit;
            if (Physics.Raycast(camera.ScreenPointToRay(Input.mousePosition), out hit))
            {
                SetDestination(hit.point);
            }
        }
    }
    private void SetDestination(Vector3 dest)
    {
        destination = dest;
        isMove = true;
        animator.SetBool("IsMove", true);
    }

    private void Move()
    {
        if (isMove)
        {
            var dir = destination - transform.position;
            animator.transform.forward = dir;
            transform.position += dir.normalized * Time.deltaTime * speed;
        }

        if (Vector3.Distance(transform.position, destination) <= 0.1f)
        {
            isMove = false;
            animator.SetBool("IsMove", false);
        }
    }

}
