using UnityEngine;

class Whale: MonoBehaviour {
    public enum WhaleState { PLAYFUL, HUNGRY };
    public WhaleState state = WhaleState.HUNGRY;
    float currentSpeed;
    public float baseSpeed;
    public float knockback;
    void Start() {
        currentSpeed = baseSpeed;
    }
    void Update() {
        if (state == WhaleState.HUNGRY)
        {
            GameObject[] powerups = GameObject.FindGameObjectsWithTag("PowerUp");
            GameObject target = ClosestGameObject(powerups);
            if (target != null) {
                Utility.MoveTowardsTarget(gameObject, target, currentSpeed, 10);
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
    void OnTriggerEnter2D(Collider2D collider) {
        GameObject col = collider.gameObject;
        if (col.TryGetComponent<Knockbackable>(out var knockbackable)) {
            Vector2 dir = GetComponent<Rigidbody2D>().velocity;//.normalized;
            knockbackable.ApplyKnockback(new(dir, 35));
        }
    }
}