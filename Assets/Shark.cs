using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shark : MonoBehaviour
{
    public enum MentalState { HUNGRY, ANGRY, TIRED, SLEEPING };

    [SerializeField] float awakenAngerDistance = 3;
    public Color normalColor = new(22, 68, 190, 255);
    public Color angryColor = Color.red;
    public float damage;
    public float knockback;
    public float health;
    public float baseSpeed;
    [SerializeField] GameObject gameStageObj;
    private GameStage gameStage;
    [SerializeField] GameObject player;
    public MentalState state = MentalState.HUNGRY;
    float hungriness = 0;
    public float tirednessLimit;
    public float tiredness = 0;
    float angriness = 0;
    GameObject target;
    float currentSpeed;
    float awakenedTimer;
    [SerializeField] float awakenedFastRotTime;
    [SerializeField] float justAwakenedRotSpeedMult;
    [SerializeField] float angryMovementSpeedMult;
    [SerializeField] float angryRotSpeedMult;
    void Start()
    {
        player = GameObject.Find("Player");
        gameStageObj = GameObject.Find("Stage");
        gameStage = gameStageObj.GetComponent<GameStage>();
        currentSpeed = baseSpeed;
        target = player;
    }
    void Update()
    {
        if (state == MentalState.ANGRY) GetComponent<SpriteRenderer>().color = angryColor;
        else GetComponent<SpriteRenderer>().color = normalColor;

        if (state == MentalState.SLEEPING) {
            tiredness -= 1;
            if (tiredness <= 0) {
                tiredness = 0;
                state = MentalState.HUNGRY;
                awakenedTimer = 0;
                target = player;
            }
            if ((player.transform.position - transform.position).magnitude < awakenAngerDistance) {
                tiredness = 0;
                state = MentalState.ANGRY;
                awakenedTimer = 0;
                target = player;
            }
        }
        else if (state == MentalState.TIRED) {
            // Find sleeping place
            GameObject[] places = GameObject.FindGameObjectsWithTag("SharkSleepingPlace");
            target = null;
            float lowestLength = Mathf.Infinity;
            foreach (var place in places) {
                Plant plant = place.GetComponent<Plant>();
                if (plant.isClaimed && plant.claimedBy != gameObject) {
                    continue;
                }
                float lengthCmp = (place.transform.position - transform.position).sqrMagnitude;
                if (lengthCmp < lowestLength) {
                    target = place;
                    lowestLength = lengthCmp;
                }
            }
            if (target != null) {
                target.GetComponent<Plant>().isClaimed = true;
                target.GetComponent<Plant>().claimedBy = gameObject;
                float distance = Utility.MoveTowardsTarget(gameObject, target, currentSpeed);;
                if (distance < 0.1) {
                    state = MentalState.SLEEPING;
                    GetComponent<Rigidbody2D>().velocity = new(0, 0);
                    GetComponent<Rigidbody2D>().angularVelocity = 0;
                }
            }
        }
        else {
            tiredness += 0.1f;
            if (tiredness >= tirednessLimit) {
                state = MentalState.TIRED;
            }
        }
        if ((state == MentalState.HUNGRY || state == MentalState.ANGRY)
        && target == player)
        {
            float rotSpeed = 35;
            float movementSpeed = currentSpeed;
            if (awakenedTimer <= awakenedFastRotTime) {
                rotSpeed *= justAwakenedRotSpeedMult;
                awakenedTimer += Time.deltaTime;
            }
            if (state == MentalState.ANGRY) {
                movementSpeed *= angryMovementSpeedMult;
                rotSpeed *= angryRotSpeedMult;
            }
            Utility.MoveTowardsTarget(gameObject, target, movementSpeed, rotSpeed);
        }
        else {
            awakenedTimer = float.PositiveInfinity;
        }

        if (transform.position.y < -GameStage.gameAreaHeight
        || transform.position.y > GameStage.gameAreaHeight) {
            Destroy(gameObject);
        }
    }
    void OnCollisionEnter2D(Collision2D col) {
        if (col.collider.gameObject.CompareTag("Obstacle")) {
            //Destroy(col.collider.gameObject);
        }
    }
}