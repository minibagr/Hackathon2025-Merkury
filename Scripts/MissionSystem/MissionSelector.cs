using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MissionSelector : MonoBehaviour {
    private static int remainingTime = 3;
    private static dificulties difficulty = dificulties.easy;

    [SerializeField] private List<Item> items;
    [SerializeField] private List<int> itemsCount;
    [SerializeField] private List<Item> itemPool;
    [SerializeField] private List<Mission> customMissions;
    [SerializeField] private int minItems;
    [SerializeField] private Vector2 rewardRange;
    [SerializeField] private Vector2Int amountRange;
    [SerializeField] private int reward = 0;
    [SerializeField] private AudioClip defaultSound;
    [SerializeField] private AudioClip completeSound;
    public Sound sound;

    public enum dificulties {
        easy = 1,
        normal = 2,
        hard = 3
    }

    private void Awake() {
        CheckIfCompleted();
    }

    public void InsertItem() {
        if (sound.audioSource.isPlaying) return;

        Inventory inventory = Player.inventory;

        if (inventory.selectedSlot == -1) return;

        Slot selectedSlot = inventory.inventory[inventory.selectedSlot];

        for (int i = 0; i < items.Count; i++) {
            if (selectedSlot.itemInSlot == items[i]) {
                int state = itemsCount[i] - selectedSlot.amount;

                if (state > 0) {
                    inventory.RemoveItem(selectedSlot, selectedSlot.amount);
                    itemsCount[i] = state;

                    sound.audioSource.clip = defaultSound;
                    CheckIfCompleted();

                    sound.PlaySound();
                } else {
                    inventory.RemoveItem(selectedSlot, selectedSlot.amount + state);
                    items.RemoveAt(i);
                    itemsCount.RemoveAt(i);

                    sound.audioSource.clip = defaultSound;
                    CheckIfCompleted();

                    sound.PlaySound();
                }

                break;
            }
        }
    }

    public void CheckIfCompleted() {
        if (items.Count > 0) return;

        Money.UpdateMoney(reward);
        sound.audioSource.clip = completeSound;
        NextMission();
    }

    private void NextMission() {
        if (customMissions.Count != 0) {
            Mission mission = customMissions.First();
            customMissions.Remove(mission);
            CustomMission(mission.Duplicate());
        } else RandomMission(difficulty);
    }

    private void RandomMission(dificulties difficulty) {
        List<Item> itemList = new List<Item>();
        List<int> amountList = new List<int>();

        for (int i = 0; i < (int)(minItems + difficulty); i++) itemList.Add(itemPool[Random.Range(0,itemPool.Count)]);
        for (int i = 0; i < (int)(minItems + difficulty); i++) amountList.Add(Random.Range(amountRange.x, amountRange.y));

        items = itemList;
        itemsCount = amountList;

        float range = (rewardRange.y - rewardRange.x) / 3;

        reward = (int)Random.Range(rewardRange.x * (float)difficulty, range * (float)difficulty);
    }

    private void CustomMission(Mission mission) {
        items = mission.items;
        reward = mission.reward;
        itemsCount = mission.amount;
    }
}
