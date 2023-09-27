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
    float hungriness = 0;
    float tiredness = 0;
    float angriness = 0;
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
        if (state == MentalState.TIRED) {
            tiredness -= 0.1f;
            if (tiredness <= 0) {
                tiredness = 0;
                state = MentalState.HUNGRY;
            }
        }
        else {
            tiredness += 0.001f;
            if (tiredness >= 100) {
                state = MentalState.TIRED;
            }
        }
        if ((state == MentalState.HUNGRY || state == MentalState.ANGRY)
        && target == player) {
            Vector3 direction = (player.transform.position - transform.position).normalized;
            Rigidbody2D rigidbody = GetComponent<Rigidbody2D>();
            rigidbody.velocity = direction * currentSpeed;
        }
    }
}
