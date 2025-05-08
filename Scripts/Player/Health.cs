using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour {
    [SerializeField] private float health = 100;
    [SerializeField] private GameObject visualsCanvas;
    [SerializeField] private Image[] visuals;
    [SerializeField] private Vector2[] range;

    public void Update() {
        if (!ObjectLock.active) visualsCanvas.SetActive(false);
        else visualsCanvas.SetActive(true);
    }

    public void UpdateHealth(float amount) {
        health += amount;

        if (health > 100) health = 100;
        else if (health <= 0) {
            health = 0;
            Debug.Log("Player died");
        }

        UpdateHealthUI();
    }

    private void UpdateHealthUI() {
        for (int i = 0; i < visuals.Length; i++) {
            Color updatedColor = visuals[i].color;
            if (health <= range[i].y && health >= range[i].x) {
                float normalized = Mathf.InverseLerp(range[i].x / 100, range[i].y / 100, health / 100);
                normalized = 1 - normalized;

                updatedColor.a = normalized;
            } else if (health <= range[i].x) updatedColor.a = 1;
            else updatedColor.a = 0;

            visuals[i].color = updatedColor;
        }
    }
}
