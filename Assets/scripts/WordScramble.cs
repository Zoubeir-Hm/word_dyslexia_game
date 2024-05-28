using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[System.Serializable]
public class Result
{
    public int totalScore = 0;



    [Header("REF UI")]

    public Text textTime;
    public Text textTotalScore;

    [Header("REF RESULT SCREEN")]
    public GameObject resultCanvas;
    public Image[] stars;
    public Text textResultScore;

    [Space(10)]
    public Color starOn;
    public Color starOFF;


    public void ShowResult() {
        //textResultScore.text = totalScore.ToString();
        
        textResultScore.text = Mathf.RoundToInt(WordScramble.main.totalScore).ToString();

        int allTimeLimit = WordScramble.main.GetAllTimeLimit();

        for (int i = 0; i < stars.Length; i++)
        {
            if (totalScore >= allTimeLimit / (3-i))
            {
                Debug.Log("im in enabled");
                stars[i].enabled = true;
            } else
            {
                Debug.Log("Not in enabled");
                stars[i].enabled = false;
            }
        }

        resultCanvas.SetActive(true);
    }
}


[System.Serializable]
public class Word
{
    public string word;
    [Header("leave empty if you want randomized")]
    public string desiredRandom;

    [Space(10)]
    public float timeLimit;

    public string GetString() 
    {
        if(!string.IsNullOrEmpty(desiredRandom))
        {
            return desiredRandom;
        }

        string result = word;
        while(result == word) {
            result = "";
            List<char> characters = new List<char>(word.ToCharArray());

            while(characters.Count > 0)
            {
                int indexChar = Random.Range(0, characters.Count - 1);
                result += characters[indexChar];

                characters.RemoveAt(indexChar);
            }

        } 
        return result;
    
        
    }

}

public class WordScramble : MonoBehaviour
{
    public Word[] words;
    public float totalScore;
    [Space(10)]
    public Result result;


    [Header(("UI REFERENCE"))]
    public GameObject wordCanvas;
    public CharObject prefab;
    public Transform container;
    public float space;
    public float LerpSpeed;
    List<CharObject> charObjects = new List<CharObject>();
    CharObject firstSelected;

    public int currentWord;

    public static WordScramble main;

    void Awake() {
        main = this;    
    }
    
    // Start is called before the first frame update
    void Start()
    {
      ShowScramble(currentWord);
      result.textTotalScore.text = "score : "+result.totalScore.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        RepositionObject();
        totalScore = Mathf.Lerp(totalScore, result.totalScore, Time.deltaTime * 5);
        result.textTotalScore.text = "score : "+Mathf.RoundToInt(totalScore).ToString();
    }

    public int GetAllTimeLimit()
    {
        float result = 0;
        foreach(Word w in words)
        {
            result += w.timeLimit / 2 ;
        }

        return Mathf.RoundToInt(result);
    }

    void RepositionObject()
    {
        if(charObjects.Count == 0)
        {
            return;
        }

        float center = (charObjects.Count - 1) / 2;

        for (int i = 0; i < charObjects.Count; i++)
        {
            charObjects[i].rectTransform.anchoredPosition = Vector2.Lerp(charObjects[i].rectTransform.anchoredPosition, new Vector2((i - center) * space, 0), LerpSpeed * Time.deltaTime);
            charObjects[i].index = i;
        }
    }

    /// <summary>
    /// Show a random word to the screen
    /// </summary>

    public void ShowScramble ()
    {
        ShowScramble(Random.Range(0, words.Length - 1));
    }

    /// <summary>
    /// Show word from collection with desired index
    /// </summary>
    /// <param name="index"> index of the element </param>

    public void ShowScramble (int index) 
    {
        charObjects.Clear();
        foreach(Transform child in container)
        {
            Destroy(child.gameObject);
        }

        if (index > words.Length - 1)
        {
            result.ShowResult();
            wordCanvas.SetActive(false);
            return;
        }

        char[] chars = words[index].GetString().ToCharArray();
      
        foreach(char c in chars)
        {
            CharObject clone = Instantiate(prefab.gameObject).GetComponent<CharObject>();
            clone.transform.SetParent(container);
            charObjects.Add(clone.Init(c));
            
        }

        currentWord = index;
        StartCoroutine(TimeLimit());
    }

    public void Swap(int indexA, int indexB)
    {
        
        //Debug.Log(indexA);
        CharObject tmpA = charObjects[indexA];
        charObjects[indexA] = charObjects[indexB];
        charObjects[indexB] = tmpA;

        charObjects[indexA].transform.SetAsLastSibling();
        charObjects[indexB].transform.SetAsLastSibling();
        CheckWord();
    }

    public void Select (CharObject charObject)
    {
        if (firstSelected)
        {
            Debug.Log("this is the first word index" + (firstSelected.index).ToString());
            Debug.Log("this is the second word index" + (charObject.index).ToString());
            Swap(firstSelected.index, charObject.index);
            //unselected
            firstSelected.Select();
            charObject.Select();
        } else 
        {
            Debug.Log("im here");
            firstSelected = charObject;
        }
    }

    public void UnSelect()
    {
        firstSelected = null;

    }

    public void CheckWord()
    {
        StartCoroutine(CoCheckWord());
    }

    IEnumerator CoCheckWord() 
    {
        yield return new WaitForSeconds(1f);
        string word = "";
        foreach (CharObject charObject in charObjects)
        {
            word += charObject.character;
        }

        if (timeLimit <=0) {
            currentWord++;
            ShowScramble(currentWord);
            yield break;
        }

        if (word == words[currentWord].word) 
        {
            currentWord++;
            result.totalScore += Mathf.RoundToInt(timeLimit);

            StopCoroutine(TimeLimit());
            ShowScramble(currentWord);
            
        } 
    }
    float timeLimit;
    IEnumerator TimeLimit()
    {
        timeLimit = words[currentWord].timeLimit;
        result.textTime.text = Mathf.RoundToInt(timeLimit).ToString();
        
        int myword = currentWord;
        yield return new WaitForSeconds(1);
        while(timeLimit > 0)
        {
            if(myword != currentWord){
                yield break;
            }

            timeLimit -= Time.deltaTime;
            result.textTime.text = Mathf.RoundToInt(timeLimit).ToString();
            yield return null;
        }

        
        CheckWord();
    }
}

