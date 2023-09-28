using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
    private const float knockbackTime = 35;
    [SerializeField] float movement_speed = 1.0f;
    [SerializeField] float angleStep = 15.0f;
    float powerUpDeaccelerationLeft = 0;
    [SerializeField] float powerUpAcceleration = 1000;
    [SerializeField] float deacclerationSpeed = 30;
    public float maxHealth = 200;
    private float _health;
    public float health {
        get => _health;
        set {
            _health = value;
            if (_health <= 0) {
                // Game over
                Time.timeScale = 0;
                GameObject text = new("GameOver");
                text.transform.parent = GameObject.Find("Canvas").transform;
                text.transform.position = GameObject.Find("Player").transform.position;
                TextMeshPro t = text.AddComponent<TextMeshPro>();
                t.text = "Game over";
            }
        }
    }
    void Start()
    {
        health = maxHealth;
    }

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
        if (!GetComponent<Knockbackable>().TakingKnockback()) {
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
        else if (collider.gameObject.TryGetComponent(out FakePowerUp fakePowerUp)) {
            Vector2 k = -1 * GetComponent<Rigidbody2D>().velocity.normalized * fakePowerUp.knockback;
            GetComponent<Knockbackable>().ApplyKnockback(new (k, knockbackTime));
            health -= fakePowerUp.damage;
            Destroy(collider.gameObject);
        }
    }
    void OnCollisionEnter2D(Collision2D collider) {
        if(collider.gameObject.TryGetComponent<Shark>(out var shark)) {
            health -= shark.damage;
            Vector2 k = collider.gameObject.GetComponent<Rigidbody2D>().velocity.normalized * shark.knockback;
            GetComponent<Knockbackable>().ApplyKnockback(new (k, knockbackTime));
        }
    }
}