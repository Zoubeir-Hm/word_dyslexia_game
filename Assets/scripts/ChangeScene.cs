using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    public void NextScene(int sceneId)
    {
        SceneManager.LoadScene(sceneId);
    }

    public void HomeScene(){
        SceneManager.LoadScene(7);   
    }
}
