using UnityEngine;

public class Particle : MonoBehaviour {
    public ParticleSystem particleSystem;
    public Sound sound;

    void Update() {
        if ((particleSystem && particleSystem.isPlaying) || (sound && sound.audioSource.isPlaying)) return;

        Destroy(gameObject);
    }
}
