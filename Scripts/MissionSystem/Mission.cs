using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(menuName = "Missions/Mission")]
public class Mission : ScriptableObject {
    public List<Item> items;
    public List<int> amount;
    public int reward;

    public Mission Duplicate() {
        Mission newMission = CreateInstance<Mission>();
        newMission.items = new List<Item>(items);
        newMission.amount = new List<int>(amount);
        newMission.reward = reward;

        return newMission;
    }
}
