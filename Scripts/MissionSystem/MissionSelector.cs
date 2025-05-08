using UnityEngine;

public class MissionSelector : MonoBehaviour {
    private static int remainingTime = 3;

    [SerializeField] private Item[] items;
    [SerializeField] private Item[] itemPool;
    [SerializeField] private int minItems;
    [SerializeField] private Vector2 rewardRange;
    [SerializeField] private float reward;

    public enum dificulties {
        easy = 1,
        normal = 2,
        hard = 3
    }

    public void RandomMission(dificulties difficulty) {
        Item[] itemList = new Item[(int)(minItems + difficulty)];

        for (int i = 0; i < itemList.Length; i++) itemList[i] = itemPool[Random.Range(0,itemPool.Length)];

        items = itemList;

        float range = (rewardRange.y - rewardRange.x) / 3;

        reward = Random.Range(rewardRange.x * (float)difficulty, range * (float)difficulty);
    }

    public void CustomMission(Mission mission) {
        items = mission.items;
        reward = mission.reward;
    }
}
