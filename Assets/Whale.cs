using UnityEngine;

class Whale: MonoBehaviour {
    public enum WhaleState { PLAYFUL, HUNGRY };
    public WhaleState state = WhaleState.HUNGRY;
    float currentSpeed;
    public float baseSpeed;
    void Start() {
        currentSpeed = baseSpeed;
    }
    void Update() {
        if (state == WhaleState.HUNGRY)
        {
            GameObject[] powerups = GameObject.FindGameObjectsWithTag("PowerUp");
            GameObject target = ClosestGameObject(powerups);
            if (target != null) {
                MoveTowardsTarget(target, currentSpeed);
            }
        }
    }

    private GameObject ClosestGameObject(GameObject[] powerups)
    {
        GameObject target = null;
        float lowestLength = Mathf.Infinity;
        foreach (var other in powerups)
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

    float MoveTowardsTarget(GameObject target, float speed)
    {
        Vector3 dif = target.transform.position - transform.position;
        Vector3 direction = dif.normalized;
        Rigidbody2D rigidbody = GetComponent<Rigidbody2D>();
        rigidbody.velocity = direction * speed;
        return dif.magnitude;
    }
}