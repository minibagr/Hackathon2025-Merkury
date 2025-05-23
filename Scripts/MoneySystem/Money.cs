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

    private void Start() {
        money = 100;
        UpdateAllMoneyUI();
    }

    public static state UpdateMoney(float amount) {
        if (amount < 0 && money < Mathf.Abs(amount)) return state.Fail;

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

    public static float GetMoney() {
        return money;
    }

    public static void SetMoney(float amount) {
        money = amount;
    }
}
