using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
    [SerializeField] float movement_speed = 1.0f;
    [SerializeField] float angleStep = 15.0f;
    [SerializeField] float max_health = 100;
    float powerUpDeaccelerationLeft = 0;
    [SerializeField] float powerUpAcceleration = 1000;
    [SerializeField] float deacclerationSpeed = 30;
    bool taking_knockback = false;
    Vector3 current_knockback;
    [SerializeField] float knockbackDeacceleration = 0.1f;
    float health;
    void Start()
    {
        health = max_health;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateTransform();
    }

    private void UpdateTransform()
    {
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
        float powerUpDeacceleration = Math.Min(deacclerationSpeed, powerUpDeaccelerationLeft);
        movement_speed -= powerUpDeacceleration;
        powerUpDeaccelerationLeft -= powerUpDeacceleration;
        if (taking_knockback) {
            float newKnockback = current_knockback.magnitude - knockbackDeacceleration;
            current_knockback = current_knockback.normalized * newKnockback;
            if (current_knockback.sqrMagnitude <= 0) {
                taking_knockback = false;
            }
            Rigidbody2D rigidbody = GetComponent<Rigidbody2D>();
            rigidbody.velocity = current_knockback;
        }
        else {
            GetComponent<Rigidbody2D>().velocity = movement_speed * transform.up;
        }
    }
    // Player collided with powerup
    void OnTriggerEnter2D(Collider2D collider) {
        movement_speed += powerUpAcceleration;
        powerUpDeaccelerationLeft = powerUpAcceleration;
        Destroy(collider.gameObject);
    }
    void OnCollisionEnter2D(Collision2D collider) {
        if(collider.gameObject.TryGetComponent<Shark>(out var shark)) {
            health -= shark.damage;
            taking_knockback = true;
            current_knockback = collider.gameObject.GetComponent<Rigidbody2D>().velocity.normalized * shark.knockback;
        }
    }
}