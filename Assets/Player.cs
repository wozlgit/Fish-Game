using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
    private const float knockbackTime = 35;
    [SerializeField] float movement_speed = 1.0f;
    [SerializeField] float angleStep = 15.0f;
    [SerializeField] float max_health = 100;
    float powerUpDeaccelerationLeft = 0;
    [SerializeField] float powerUpAcceleration = 1000;
    [SerializeField] float deacclerationSpeed = 30;
    public class Knockback {
        public Vector2 knockbackLeft;
        public float timeLeft;
        public Knockback(Vector2 aKnockbackLeft, float aTimeLeft) {
            knockbackLeft = aKnockbackLeft;
            timeLeft = aTimeLeft;
        }
    }
    List<Knockback> knockbacks = new List<Knockback>();
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

        Rigidbody2D rigidbody = GetComponent<Rigidbody2D>();
        if (knockbacks.Count > 0) {
            rigidbody.velocity = new(0, 0);
            for (int i = 0; i < knockbacks.Count; i++) {
                Vector2 used_knockack = knockbacks[i].knockbackLeft / knockbacks[i].timeLeft;
                rigidbody.velocity += used_knockack;
                knockbacks[i].timeLeft -= 1;
                knockbacks[i].knockbackLeft -= used_knockack;
                if (knockbacks[i].timeLeft <= 0) {
                    knockbacks.Remove(knockbacks[i]);
                }
            }
        }
        else {
            rigidbody.velocity = movement_speed * transform.up;
        }
        GameObject.Find("Stage").GetComponent<GameStage>().PlayerMoved(rigidbody.velocity);
    }
    // Player collided with powerup
    void OnTriggerEnter2D(Collider2D collider) {
        if (collider.gameObject.GetComponent<PowerUp>() != null) {
            movement_speed += powerUpAcceleration;
            powerUpDeaccelerationLeft = powerUpAcceleration;
            Destroy(collider.gameObject);
        }
    }
    void OnCollisionEnter2D(Collision2D collider) {
        if(collider.gameObject.TryGetComponent<Shark>(out var shark)) {
            health -= shark.damage;
            Vector2 k = collider.gameObject.GetComponent<Rigidbody2D>().velocity.normalized * shark.knockback;
            knockbacks.Add(new Knockback(k, knockbackTime));
        }
    }
}