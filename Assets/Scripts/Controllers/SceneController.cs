using UnityEngine.SceneManagement;

public class SceneController
{
    public static void GoToGameScene()
    {
        SceneManager.LoadScene(Constants.SCENE_GAME, LoadSceneMode.Single);
    }
    public static void GoToMenuScene()
    {
        SceneManager.LoadScene(Constants.SCENE_MENU, LoadSceneMode.Single);
    }
}