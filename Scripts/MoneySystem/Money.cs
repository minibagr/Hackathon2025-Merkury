using TMPro;
using UnityEngine;

public class Money : MonoBehaviour {
    public enum state {
        Fail,
        Success
    }

    private static float money;

    private void Awake() {
        money = 100;
        UpdateAllMoneyUI();
    }

    public state UpdateMoney(float amount) {
        if (amount < 0 && money < amount) return state.Fail;

        money += amount;

        UpdateAllMoneyUI();

        return state.Success;
    }

    public static void UpdateAllMoneyUI() {
        GameObject[] textObjs = GameObject.FindGameObjectsWithTag("MoneyTextUI");

        foreach (GameObject textObj in textObjs) {
            textObj.GetComponent<TMP_Text>().text = money.ToString();
        }
    }
}
