using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MissionSelector : MonoBehaviour {
    private static int remainingTime = 3;
    private static dificulties difficulty = dificulties.easy;

    [SerializeField] private Item[] items;
    [SerializeField] private int[] itemsCount;
    [SerializeField] private Item[] itemPool;
    [SerializeField] private List<Mission> customMissions;
    [SerializeField] private int minItems;
    [SerializeField] private Vector2 rewardRange;
    [SerializeField] private float reward = 0;

    public enum dificulties {
        easy = 1,
        normal = 2,
        hard = 3
    }

    public void InsertItem() {
        Inventory inventory = Player.inventory;

        Slot selectedSlot = inventory.inventory[inventory.selectedSlot];

        for (int i = 0; i < items.Length; i++) {
            if (selectedSlot.itemInSlot == items[i]) {
                int state = itemsCount[i] - selectedSlot.amount;

                if (state > 0) {
                    inventory.RemoveItem(selectedSlot, selectedSlot.amount);
                    itemsCount[i] = state;
                } else  {
                    inventory.RemoveItem(selectedSlot, selectedSlot.amount + state);
                    items = items.Where((_, index) => index != i).ToArray();
                    itemsCount = itemsCount.Where((_, index) => index != i).ToArray();
                }

                CheckIfCompleted();

                break;
            }
        }
    }

    public void CheckIfCompleted() {
        if (items.Length > 0) return;

        Money.UpdateMoney(reward);
        NextMission();
    }

    private void NextMission() {
        if (customMissions.Count != 0) {
            Mission mission = customMissions.First();
            customMissions.Remove(mission);
            CustomMission(mission);
        } else RandomMission(difficulty);
    }

    private void RandomMission(dificulties difficulty) {
        Item[] itemList = new Item[(int)(minItems + difficulty)];

        for (int i = 0; i < itemList.Length; i++) itemList[i] = itemPool[Random.Range(0,itemPool.Length)];

        items = itemList;

        float range = (rewardRange.y - rewardRange.x) / 3;

        reward = Random.Range(rewardRange.x * (float)difficulty, range * (float)difficulty);
    }

    private void CustomMission(Mission mission) {
        items = mission.items;
        reward = mission.reward;
        itemsCount = mission.amount;
    }
}
