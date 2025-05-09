using UnityEngine;

public class Sound : MonoBehaviour {
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private float pitch = 1.0f;
    [SerializeField] private Vector2 range;

    public void PlaySound() {
        if (audioSource != null && !audioSource.isPlaying) {
            audioSource.pitch = pitch + Random.Range(range.x, range.y);
            audioSource.Play();
        }
    }
}
