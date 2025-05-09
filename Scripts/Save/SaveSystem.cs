using System.IO;
using UnityEngine;

public class SaveSystem : MonoBehaviour {
    public static string saveFilePath = "";

    void Awake() {
        if (saveFilePath != "") return;

        CheckFile();
    }

    void Start() {
        if (saveFilePath != "") return;

        CheckFile();
    }

    private static void CheckFile() {
        string path = Application.persistentDataPath + "/save.json";

        if (!File.Exists(path)) {
            string createText = "";
            File.WriteAllText(path, createText);
        }

        saveFilePath = path;
    }

    public static void Save() {
        CheckFile();

        SavedData savedData = new SavedData();
        savedData.money = Money.GetMoney();

        string save = JsonUtility.ToJson(savedData);

        File.WriteAllText(saveFilePath, save);
    }

    public static void Load() {
        CheckFile();

        string savedData = File.ReadAllText(saveFilePath);

        if (savedData.Length < 3) return;

        SavedData loadedData = JsonUtility.FromJson<SavedData>(savedData);

        Money.SetMoney(loadedData.money);
        Money.UpdateAllMoneyUI();
    }
}
