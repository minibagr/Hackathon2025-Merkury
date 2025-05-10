using UnityEngine;

public class Tool : MonoBehaviour {
    public ToolItem tool;
    public GameObject particle;
    public Animation animation;
    [SerializeField] private float useTime;
    [SerializeField] private float soundTime;
    [SerializeField] private bool used = false;
    [SerializeField] private bool soundPlayed = false;
    [SerializeField] private float correct;
    [SerializeField] private Sound sound;
    [SerializeField] private AnimationState firstState = null;
    [SerializeField] private GameObject hideSound;

    void OnEnable() {
        if (used) {
            firstState.time = correct;
            animation.Play();
        }
    }

    void OnDisable() {
        if (gameObject.scene.isLoaded && Application.isPlaying && Time.timeScale != 0) Instantiate(hideSound, transform.position, Quaternion.identity);

        if (used && firstState) {
            correct = firstState.time;
        }
    }

    private void Update() {
        if (used) {
            if (firstState != null) {
                if (firstState.normalizedTime >= useTime) {
                    used = false;
                    Use();
                } else if (!soundPlayed && firstState.normalizedTime >= soundTime) {
                    sound.PlaySound();
                    soundPlayed = true;
                }
            } else {
                used = false;
                Use();
                sound.PlaySound();
            }
            return;
        }

        if (animation && animation.isPlaying) return;

        if (Input.GetKey(KeyCode.Mouse0)) {
            foreach (AnimationState state in animation) {
                firstState = state;
                break;
            }

            animation.Play();

            used = true;
            soundPlayed = false;
        }
    }

    public void Use() {
        RaycastHit hit;

        Transform cameraPivot = Camera.main.transform.parent;

        if (Physics.Raycast(cameraPivot.position, cameraPivot.TransformDirection(Vector3.forward), out hit, tool.range)) {
            if (hit.transform.tag != "Material") return;
            
            MaterialObj material = hit.transform.GetComponent<MaterialObj>();

            if (material.material.subItemType != tool.toolType) return;

            material.Mine(tool.damage);

            tool.durability -= 1;

            Particle part = particle.GetComponent<Particle>();

            ParticleSystem.MainModule particleSystemMain = part.particleSystem.main;
            particleSystemMain.startColor = material.particleColor;

            part.sound.audioSource.clip = material.sound;

            Instantiate(particle, hit.point, Quaternion.identity);

            if (tool.durability <= 0) Player.inventory.RemoveItem(tool, 1);
        }
    }
}
