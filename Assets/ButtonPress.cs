using UnityEditor;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;



public class ButtonPress : MonoBehaviour
{
  
    public void startGame(SceneAsset sa){
        SceneManager.LoadSceneAsync(sa.name);
    }

    public void quitGame(){
        Application.Quit();


    }
}
