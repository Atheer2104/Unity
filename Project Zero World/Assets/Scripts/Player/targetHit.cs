using UnityEngine;

public class targetHit : MonoBehaviour {

    public float health = 100f;

    public void takeDamage(float damage) {
        
        health -= damage;
        if (health <= 0f) {
            Die();
        }

    }

    void Die() {
        Destroy(gameObject);
    }

}

