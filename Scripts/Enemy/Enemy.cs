using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class Enemy : MonoBehaviour {
    [SerializeField] private float health = 100;
    [SerializeField] private bool canBeKilled = true;

    [SerializeField] private NavMeshAgent agent;

    [SerializeField] private float attackDamage;
    [SerializeField] private float attackRange;

    [SerializeField] private float detectDistance;
    [SerializeField] private float seeDistance;

    private Vector3? lastSeenPosition = null;

    private void Update() {
        if (agent.destination != null && agent.destination == lastSeenPosition) lastSeenPosition = null; 
      
        // TODO 1: add sneak modifiers

        // sense should be for small distances
        if (Vector3.Distance(Player.player.position, transform.position) <= detectDistance) Debug.Log("Sense position: " + Player.player.position);
        else {
            RaycastHit hit;

            Vector3 direction = Player.player.position - transform.position;

            Debug.DrawRay(transform.position, direction, Color.blue);
            if (Physics.Raycast(transform.position, direction, out hit, seeDistance)) {
                if (hit.transform.tag == "Player") {
                    lastSeenPosition = hit.point;
                    agent.destination = hit.point;
                } else if (lastSeenPosition != null) {
                    agent.destination = (Vector3)lastSeenPosition;
                } // TODO 1: wander
            } else if (lastSeenPosition != null) {
                agent.destination = (Vector3)lastSeenPosition;
            } // TODO 1: wander
        }
    }

    public void UpdateHealth(float amount) {
        if (!canBeKilled) return;

        health += amount;

        // TODO 5: maybe spawn numbers around the damaged thing?

        if (health > 100) health = 100;
        else if (health <= 0) {
            health = 0;
            Debug.Log("Enemy died");
        }
    }
}
