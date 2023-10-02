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
    float movement_speed;
    public float baseSpeed = 8;
    public float angleStep = 40.0f;
    float powerUpDeaccelerationLeft = 0;
    [SerializeField] float powerUpAcceleration = 1000;
    [SerializeField] float deacclerationSpeed = 30;
    private GameStage gameStage;
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
        gameStage = GameStage.Get();
        movement_speed = baseSpeed;
    }

    void Update()
    {
        UpdateTransform();
        if (transform.position.y < -GameStage.gameAreaHeight) {
            transform.position = new Vector3(
                transform.position.x,
                -GameStage.gameAreaHeight,
                transform.position.z
            );
        }
        else if (transform.position.y > GameStage.gameAreaHeight) {
            transform.position = new Vector3(
                transform.position.x,
                GameStage.gameAreaHeight,
                transform.position.z
            );
        }
    }
    private void UpdateTransform()
    {
        float prevZ = transform.rotation.eulerAngles.z;
        float step = angleStep * Time.deltaTime;
        if (Input.GetKey(KeyCode.DownArrow))
        {
            transform.Rotate(-step * Vector3.forward);
            if (transform.eulerAngles.z < 180)
            {
                transform.eulerAngles = new Vector3(0, 0, 180);
            }
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            transform.Rotate(step * Vector3.forward);
            if (transform.eulerAngles.z < 180)
            {
                transform.eulerAngles = new Vector3(0, 0, 360);
            }
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