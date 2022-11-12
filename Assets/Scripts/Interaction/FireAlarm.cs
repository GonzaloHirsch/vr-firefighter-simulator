using UnityEngine;

public class FireAlarm : MonoBehaviour {
   public GameObject instructionText;

   public void OnPointerClick() {
      SceneController.GoToGameScene();
   }
   public void OnPointerEnter() {
      this.instructionText.SetActive(true);
   }
   public void OnPointerExit() {
      this.instructionText.SetActive(false);
   }
}