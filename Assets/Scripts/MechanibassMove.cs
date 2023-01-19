using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MechanibassMove : MonoBehaviour
{

    private float dirX;
    private float distance;
    private Rigidbody2D rb;
    private bool facingRight = false;
    private Vector3 localScale;
    private Transform point1;
    private Transform point2;
    private Transform point3;
    private Transform target;
    [SerializeField] private float moveSpeedValue;

    void Start()
    {
        localScale = transform.localScale;
        rb = GetComponent<Rigidbody2D>();
        point1 = transform.parent.transform.Find("Point1").transform;
        point2 = transform.parent.transform.Find("Point2").transform;
        point3 = transform.parent.transform.Find("Point3").transform;
        target = point1;
        dirX = -1;
    }

    void FixedUpdate()
    {

        if (transform.position.Equals(point1.position) && target.position.Equals(point1.position))
        {
            if (dirX <0) target = point2;
            else target = point3;
        }
        else if ((transform.position.Equals(point2.position) && target.position.Equals(point2.position)) || (transform.position.Equals(point3.position) && target.position.Equals(point3.position))) {
            target = point1;
            dirX = -dirX;
        }

        rb.transform.position = Vector2.MoveTowards(rb.transform.position, target.position, moveSpeedValue * Time.deltaTime);

    }

    void LateUpdate()
    {
        CheckWhereToFace();
    }

    void CheckWhereToFace()
    {
        if (dirX > 0)
            facingRight = false;
        else if (dirX < 0)
            facingRight = true;

        if (((facingRight) && (localScale.x < 0)) || ((!facingRight) && (localScale.x > 0)))
            localScale.x *= -1;

        transform.localScale = localScale;
    }

}