using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shark : MonoBehaviour
{
    enum MentalState { HUNGRY, ANGRY, TIRED };
    public float damage;
    public float knockback;
    public float health;
    public float baseSpeed;
    [SerializeField] GameObject player;
    MentalState state = MentalState.HUNGRY;
    GameObject target;
    float currentSpeed;
    void Start()
    {
        target = player;
        currentSpeed = baseSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        if ((state == MentalState.HUNGRY || state == MentalState.ANGRY)
        && target == player) {
            Vector3 direction = (player.transform.position - transform.position).normalized;
            Rigidbody2D rigidbody = GetComponent<Rigidbody2D>();
            rigidbody.velocity = direction * currentSpeed;
        }
        else if (state == MentalState.TIRED) {
            // Find plant to sleep in
        }
    }
}
