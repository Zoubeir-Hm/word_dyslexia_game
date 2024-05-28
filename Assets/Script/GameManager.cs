using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{

    [Header("Menu Scene Settings")]
    public string menu_scene;
    public string first_quiz_scene;
    public string result_scene;


    [Header("Quiz Scene Settings")]
    public string correctAnswer;
    public int correctScore;
    public string nextScene;
    public AudioSource sound;
    public AudioClip correctSound;
    public AudioClip wrongSound;
    public AudioClip hintSound;


    [Header("Result Scene Settings")]
    public Text text_score;
    public GameObject[] stars;
    public int _1_star_limit;
    public int _2_star_limit;
    public int _3_star_limit;

    [Header("For Debugging Only (Ignore Me)")]
    public int currentScore;
    Scene activeScene;
    // Start is called before the first frame update
    void Start()
    {
        currentScore = PlayerPrefs.GetInt("score");
        activeScene = SceneManager.GetActiveScene();
        
        if(activeScene.name == menu_scene)
        {
            PlayerPrefs.SetString("result_scene", result_scene);
            PlayerPrefs.SetString("quiz_scene", first_quiz_scene);

        }
        else if(activeScene.name == PlayerPrefs.GetString("quiz_scene"))
        {
            PlayerPrefs.DeleteKey("score");
            Debug.Log("Ok" + PlayerPrefs.GetInt("score"));
            currentScore = PlayerPrefs.GetInt("score");

        }
        else if (activeScene.name == PlayerPrefs.GetString("result_scene"))
        {
            if(currentScore <= _1_star_limit)
            {
                stars[0].SetActive(true);
                stars[1].SetActive(false);
                stars[2].SetActive(false);
            }
            else if(currentScore <= _2_star_limit)
            {
                stars[0].SetActive(true);
                stars[1].SetActive(true);
                stars[2].SetActive(false);
            }
            else if(currentScore <= _3_star_limit)
            {
                stars[0].SetActive(true);
                stars[1].SetActive(true);
                stars[2].SetActive(true);
            }
            text_score.text = "" + currentScore;
            
        }
    }

    public void OpenScenes(string halaman)
    {
        SceneManager.LoadScene(halaman);
    }

    public void OpenPopup(GameObject gameobject)
    {
        gameobject.SetActive(true);
    }

    public void ClosePopup(GameObject gameObject)
    {
        gameObject.SetActive(false);
    }

    public void UserAnswer(string answer)
    {
        StartCoroutine(AnwerCheck(answer));
    }

    IEnumerator AnwerCheck(string answer)
    {
        if (correctAnswer == answer)
        {
            currentScore = currentScore + correctScore;
            PlayerPrefs.SetInt("score", currentScore);
            sound.clip = correctSound;
            sound.Play();
        }
        else
        {
            sound.clip = wrongSound;
            sound.Play();
        }
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(nextScene);
    }

    public void PlaySound()
    {
        sound.clip = hintSound;
        sound.Play();
    }

    public void CloseGame()
    {
        Application.Quit();
    }
}
