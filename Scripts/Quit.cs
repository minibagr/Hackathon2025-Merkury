using UnityEngine;

public class Quit : MonoBehaviour {
    public static void QuitGame() {
        Debug.Log("Quit");
        Application.Quit();
    }
}
