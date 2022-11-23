using UnityEngine;
using TMPro;

public class MenuController : Framework.MonoBehaviorSingleton<MenuController>
{
    public GameObject resultObj;
    public TMP_Text timeText;
    public TMP_Text approvalText;

    void Start() {
        this.resultObj.SetActive(State.Instance.hasPlayed);
        if (State.Instance.hasPlayed) {
            // Set the text
            int minutes = Mathf.FloorToInt((State.Instance.endTime - State.Instance.startTime) / 60);
            int seconds = Mathf.FloorToInt(((State.Instance.endTime - State.Instance.startTime) - minutes * 60));
            this.timeText.text = $"Fire turned off in {minutes} minutes and {seconds} seconds.";
            this.approvalText.text = $"{(State.Instance.endTime - State.Instance.startTime <= State.Instance.approvalLimit ? "GOOD" : "NOT FAST ENOUGH")}";
        }
    }

    void HandleAlarmPull() {
        SceneController.GoToGameScene();
    }
}
