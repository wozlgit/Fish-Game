using UnityEngine;
using System.Collections.Generic;

public class Knockback {
    public Vector2 knockbackLeft;
    public float timeLeft;
    public Knockback(Vector2 aKnockbackLeft, float aTimeLeft) {
        knockbackLeft = aKnockbackLeft;
        timeLeft = aTimeLeft;
    }
}

public class Knockbackable: MonoBehaviour {
    List<Knockback> knockbacks = new List<Knockback>();
    public void ApplyKnockback(Knockback knockback) { knockbacks.Add(knockback); }
    public List<Knockback> GetKnockbacks() { return knockbacks; }
    public void TakeKnockbacks()
    {
        Rigidbody2D rigidbody = GetComponent<Rigidbody2D>();
        rigidbody.velocity = new(0, 0);
        for (int i = 0; i < knockbacks.Count; i++)
        {
            Vector2 used_knockack = knockbacks[i].knockbackLeft / knockbacks[i].timeLeft;
            rigidbody.velocity += used_knockack;
            knockbacks[i].timeLeft -= 1;
            knockbacks[i].knockbackLeft -= used_knockack;
            if (knockbacks[i].timeLeft <= 0)
            {
                knockbacks.Remove(knockbacks[i]);
            }
        }
    }
    public bool TakingKnockback() { return knockbacks.Count > 0; }
    void Update() {
        if (knockbacks.Count > 0) {
            TakeKnockbacks();
        }
    }
}