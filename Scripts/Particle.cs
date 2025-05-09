using UnityEngine;

public class Particle : MonoBehaviour {
    [SerializeField] private ParticleSystem particleSystem;

    void Update() {
        if (particleSystem.isPlaying) return;

        Destroy(gameObject);
    }
}
