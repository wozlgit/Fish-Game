using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] float movement_speed = 1.0f;
    [SerializeField] float angleStep = 15.0f;
    [SerializeField] float max_health = 100;
    float powerUpDeaccelerationLeft = 0;
    [SerializeField] float powerUpAcceleration = 1000;
    [SerializeField] float deacclerationSpeed = 30;
    float health;
    void Start()
    {
        health = max_health;
        Rigidbody2D rigidbody = GetComponent<Rigidbody2D>();
        rigidbody.AddForce(movement_speed * transform.up, ForceMode2D.Force);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateTransform();
    }

    private void UpdateTransform()
    {
        Rigidbody2D rigidbody = GetComponent<Rigidbody2D>();
        rigidbody.AddForce(-movement_speed * transform.up, ForceMode2D.Force);
        if (Input.GetKey(KeyCode.DownArrow))
        {
            transform.Rotate(-angleStep * Time.deltaTime * Vector3.forward);
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            transform.Rotate(angleStep * Time.deltaTime * Vector3.forward);
        }
        if (transform.eulerAngles.z < 225)
        {
            transform.eulerAngles = new Vector3(0, 0, 225);
        }
        else if (transform.eulerAngles.z > 315)
        {
            transform.eulerAngles = new Vector3(0, 0, 315);
        }
        rigidbody.AddForce(movement_speed * transform.up, ForceMode2D.Force);
        float deaccleration = Math.Min(deacclerationSpeed, powerUpDeaccelerationLeft);
        rigidbody.AddForce(deaccleration * transform.up, ForceMode2D.Force);
    }
    void OnTriggerEnter2D(Collider2D collider) {
        movement_speed += powerUpAcceleration;
        Rigidbody2D rigidbody = GetComponent<Rigidbody2D>();
        rigidbody.AddForce(transform.up * powerUpAcceleration, ForceMode2D.Force);
        powerUpDeaccelerationLeft = powerUpAcceleration;
        Destroy(collider.gameObject);
    }
    // Player should always move "forward"
    // But added to that should be other forces (knockback, collision, etc.)s
    void OnCollisionEnter2D(Collision2D collider) {
        Shark shark = gameObject.GetComponent("Shark") as Shark;
        if(shark != null) {
            health -= shark.damage;
            //Vector3 knockback = collider.gameObject.transform.
        }
    }
}