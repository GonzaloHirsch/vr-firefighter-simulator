using UnityEngine;

public class MenuController : Framework.MonoBehaviorSingleton<MenuController>
{
    void HandleAlarmPull() {
        SceneController.GoToGameScene();
    }
}
