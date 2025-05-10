using UnityEngine;

public class Sound : MonoBehaviour {
    public AudioSource audioSource;
    [SerializeField] private float pitch = 1.0f;
    [SerializeField] private Vector2 range;
    [SerializeField] private bool playOnAwake;

    private void Awake() {
        if (playOnAwake) PlaySound();
    }

    public void PlaySound() {
        if (audioSource != null && !audioSource.isPlaying) {
            audioSource.pitch = pitch + Random.Range(range.x, range.y);
            audioSource.Play();
        }
    }
}
