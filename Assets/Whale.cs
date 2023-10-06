using TMPro;
using UnityEngine;

class Whale: MonoBehaviour {
    public enum WhaleState { HUNGRY, TIRED_ROT, TIRED_SURGE, SLEEPING };

    [SerializeField] int hungryRotationSpeed;
    [SerializeField] int tiredRotationSpeed;
    [SerializeField] int tiredSurgeSpeed;
    public WhaleState state = WhaleState.HUNGRY;
    float currentSpeed;
    public float baseSpeed;
    public float knockback;
    GameObject target = null;
    public float timeSlept;
    public float sleepingTime;
    [SerializeField] float nestMinDistanceX;
    void Start() {
        currentSpeed = baseSpeed;
    }
    void Update() {
        if (state == WhaleState.HUNGRY)
        {
            GameObject[] powerups = GameObject.FindGameObjectsWithTag("PowerUp");
            target = ClosestGameObject(powerups);
            if (target != null) {
                Utility.MoveTowardsTarget(gameObject, target, currentSpeed, hungryRotationSpeed);
            }
        }
        else if(state == WhaleState.TIRED_ROT)
        {
            if (target == null)
            {
                target = ClosestNest();
            }
            if (target != null) {
                Utility.MoveTowardsTarget(gameObject, target, 0, tiredRotationSpeed);
                float targetRotation = Utility.GetRotationFromObject1To2(gameObject, target, out Vector3 v);
                if (Mathf.Abs(transform.rotation.eulerAngles.z - targetRotation) < 5) {
                    state = WhaleState.TIRED_SURGE;
                }
            }
        }
        else if(state == WhaleState.TIRED_SURGE)
        {
            if (target == null) {
                target = ClosestNest();
            }
            if (target != null) {
                Utility.MoveTowardsTarget(gameObject, target, tiredSurgeSpeed, 0);
                if (transform.position.x > (target.transform.position.x + 5)) {
                    state = WhaleState.SLEEPING;
                    timeSlept = 0;
                }
                /*
                When to end surge?
                    - When sufficiently close to target whale nest
                    - Since whale nest can't move, whale can't (it actually can but only VERY slightly)
                    move due to external factors, and whale only starts surge with very close to optimal rotation,
                    the surge will almost always pass through very close to the nest at some point
                    - Distance could be represented by a trigger circle collider on the WhaleNest prefab
                 */
            }
        }
        else
        {
            timeSlept += Time.deltaTime;
            if (timeSlept >= sleepingTime) {
                state = WhaleState.HUNGRY;
            }
        }
    }

    private GameObject ClosestNest()
    {
        GameObject[] nests = GameObject.FindGameObjectsWithTag("WhaleNest");
        GameObject[] actualNests = new GameObject[nests.Length];
        int index = 0;
        foreach (var nest in nests)
        {
            if (nest.transform.position.x > (transform.position.x + nestMinDistanceX))
            {
                actualNests[index] = nest;
                index++;
            }
        }
        return ClosestGameObject(actualNests[..index]);
    }

    private GameObject ClosestGameObject(GameObject[] objects)
    {
        GameObject target = null;
        float lowestLength = Mathf.Infinity;
        foreach (var other in objects)
        {
            float lengthCmp = (other.transform.position - transform.position).sqrMagnitude;
            if (lengthCmp < lowestLength)
            {
                target = other;
                lowestLength = lengthCmp;
            }
        }

        return target;
    }
    void OnTriggerEnter2D(Collider2D collider) {
        GameObject col = collider.gameObject;
        if (col.TryGetComponent<Knockbackable>(out var knockbackable)) {
            if (state == WhaleState.TIRED_SURGE) {
                Vector2 dir = GetComponent<Rigidbody2D>().velocity;
                knockbackable.ApplyKnockback(new(dir, 35));
            }
        }
        else if (col.TryGetComponent<PowerUp>(out var powerUp)) {
            if (state == WhaleState.HUNGRY)
                state = WhaleState.TIRED_ROT;
            Destroy(col);
        }
        else if (col.CompareTag("WhaleNest")) {
            state = WhaleState.SLEEPING;
            timeSlept = 0;
        }
    }
    void OnCollisionEnter2D(Collision2D collision) {
        GameObject col = collision.gameObject;
        if (col.CompareTag("Obstacle")) {
            Destroy(col);
        }
    }
}