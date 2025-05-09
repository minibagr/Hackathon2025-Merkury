using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;
using static UnityEngine.UI.Image;

public class Enemy : MonoBehaviour {
    [SerializeField] private float health = 100;
    [SerializeField] private bool canBeKilled = true;

    [SerializeField] private NavMeshAgent agent;

    [SerializeField] private float attackDamage;
    [SerializeField] private float attackRange;

    [SerializeField] private float detectDistance;
    [SerializeField] private float seeDistance;
    [SerializeField] private float fov = 60;
    [SerializeField] private float wanderRadius;
    [SerializeField] private float destinationThreshold;

    [SerializeField] private bool wait = true;
    [SerializeField] private float time;
    [SerializeField] private float defaultTime;
    [SerializeField] private Vector2 modifier;


    private Vector3? lastSeenPosition = null;

    private void Update() {

        if (agent.destination != null && agent.remainingDistance < destinationThreshold) lastSeenPosition = null; 
      
        // TODO 1: add sneak modifiers

        // sense should be for small distances
        if (Vector3.Distance(Player.player.position, transform.position) <= detectDistance) {
            lastSeenPosition = Player.player.position;
            SetDestination((Vector3)lastSeenPosition);
        } else {
            RaycastHit hit;

            Vector3 direction = Player.player.position - transform.position;

            float signedAngle = Mathf.Abs(Vector3.SignedAngle(transform.forward, direction, Vector3.up));

            if (signedAngle > fov / 2) {
                if (lastSeenPosition != null) {
                    SetDestination((Vector3)lastSeenPosition);
                } else Wander();
                return;
            }

            if (Physics.Raycast(transform.position, direction, out hit, seeDistance)) {
                if (hit.transform.tag == "Player") {
                    lastSeenPosition = hit.point;
                    SetDestination((Vector3)lastSeenPosition);
                } else if (lastSeenPosition != null) {
                    SetDestination((Vector3)lastSeenPosition);
                } else Wander();
            } else if (lastSeenPosition != null) {
                SetDestination((Vector3)lastSeenPosition);
            } else Wander();
        }
    }

    private void SetDestination(Vector3 position) {
        if (IsDestinationReachable(position)) agent.destination = position;
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

    private void Wander() {
        // TODO 1: wander
        if (agent.remainingDistance > destinationThreshold) return;

        if (wait && 0 < time) { 
            time -= Time.deltaTime;
            return;
        } else if (wait) time = defaultTime + Random.Range(modifier.x, modifier.y);

        Vector3 randomDirection = Random.insideUnitSphere * wanderRadius;
        randomDirection += transform.position;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDirection, out hit, wanderRadius, NavMesh.AllAreas)) {
            GetComponent<NavMeshAgent>().SetDestination(hit.position);
        }
    }

    bool IsDestinationReachable(Vector3 targetPosition) {
        NavMeshPath path = new NavMeshPath();
        NavMeshAgent agent = GetComponent<NavMeshAgent>();

        if (agent.CalculatePath(targetPosition, path)) {
            return path.status == NavMeshPathStatus.PathComplete;
        }

        return false;
    }
}
