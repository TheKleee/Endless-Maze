using System;
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
        Debug.Log("QUIT");
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }
}
