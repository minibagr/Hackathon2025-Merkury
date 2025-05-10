using UnityEngine;

public class Depo : MonoBehaviour {

    public void Deposit() {
        MissionSelector missionSelector = Player.player.transform.parent.GetComponent<MissionSelector>();

        missionSelector.sound = gameObject.GetComponent<Sound>();

        missionSelector.InsertItem();
    }
}
