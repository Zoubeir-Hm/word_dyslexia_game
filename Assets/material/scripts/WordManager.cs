using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public class WordManager : MonoBehaviour
{
    public static WordManager Instance { get; private set;}
    [SerializeField] DragScript letterPrefab;
    [SerializeField] Transform initialSlot, finalSlot;
    [SerializeField] string[] wordList;

    //------------------
    [SerializeField] private TMPro.TextMeshProUGUI scoreText;
    [SerializeField] GameObject next;
    [SerializeField] GameObject canvas;
    //------------------

    private int wordPoints, points, total_points;
    private int wordIndex = 0;
    
    void Start()
    {
        Instance = this;
        next.SetActive(false);
        wordIndex = 0; // Initialiser l'indice du mot à 0
        InitializeWord(wordList[wordIndex]); // Initialiser le premier mot
    }

    /*void InitializeWord(string word)
    {
        char[] wordLetters = word.ToCharArray();
        char[] shuffledLetters = new char[wordLetters.Length];

        List<char> wordLettersCopy = new List<char>();
        wordLettersCopy = wordLetters.ToList();

        for (int i = 0; i < shuffledLetters.Length; i++)
        {
            int randomIndex = Random.Range(0, wordLettersCopy.Count);
            shuffledLetters[i] = wordLettersCopy[randomIndex];
            wordLettersCopy.RemoveAt(randomIndex);

            DragScript temp = Instantiate(letterPrefab, initialSlot);
            temp.Initialize(initialSlot, shuffledLetters[i].ToString(), false);
        }

        for (int i = 0; i < wordLetters.Length; i++)
        {
            DragScript temp = Instantiate(letterPrefab, finalSlot);
            temp.Initialize(finalSlot, wordLetters[i].ToString(), true);
        }

        wordPoints = wordLetters.Length;
    }*/

    void InitializeWord(string word)
{
    // Empty initialSlot and finalSlot
    foreach (Transform child in initialSlot)
    {
        Destroy(child.gameObject);
    }
    foreach (Transform child in finalSlot)
    {
        Destroy(child.gameObject);
    }

    char[] wordLetters = word.ToCharArray();
    char[] shuffledLetters = new char[wordLetters.Length];
    List<char> wordLettersCopy = wordLetters.ToList();

    // Initialiser les lettres dans le slot initial
    for (int i = 0; i < shuffledLetters.Length; i++)
    {
        int randomIndex = Random.Range(0, wordLettersCopy.Count);

        shuffledLetters[i] = wordLettersCopy[randomIndex];
        wordLettersCopy.RemoveAt(randomIndex);

        DragScript temp = Instantiate(letterPrefab, initialSlot);
        temp.Initialize(initialSlot, shuffledLetters[i].ToString(), false);
    }

    // Initialiser les lettres dans le slot final avec une chance aléatoire de modifier l'alpha
    float alphaChangeProbability = 0.5f; // 50% de chance de changer l'alpha pour chaque lettre
    for (int i = 0; i < wordLetters.Length; i++)
    {
        DragScript temp = Instantiate(letterPrefab, finalSlot);
        temp.Initialize(finalSlot, wordLetters[i].ToString(), true);

        // Décider aléatoirement si on modifie l'alpha
        if (Random.value < alphaChangeProbability) // Random.value renvoie un nombre entre 0.0 et 1.0
        {
            temp.SetAlpha(0.05f); // Mettre un alpha faible
        }
    }

    wordPoints = wordLetters.Length;
}



    public void LoadNextWord()
    {
        // Vérifier s'il y a un mot suivant dans la liste
        if (wordIndex + 1 < wordList.Length)
        {
            
            wordIndex++; // Incrémenter l'indice du mot
            InitializeWord(wordList[wordIndex]); // Initialiser le nouveau mot
        }
        else
        {
            Debug.Log("Fin de la liste de mots."); // Afficher un message si tous les mots ont été complétés
        }
    }
    public void AddPoint()
    {
        points++;
        if (points == wordPoints)
        {
            
            next.SetActive(true);
            canvas.SetActive(false);
            total_points += points;
            points = 0;
            scoreText.SetText("Score: " + total_points);
            Debug.Log("hello" + points);

            // Vérifier si le joueur a terminé tous les mots
            if (wordIndex + 1 < wordList.Length)
            {
                // Charger le mot suivant
                WordManager.Instance.LoadNextWord();
            }
            else
            {
                Debug.Log("Fin de la liste de mots.");
            }
        }
    }

    public void nextWord()
    {
        
        wordIndex++;
        next.SetActive(false);
        canvas.SetActive(true);
    }
    
    
    
}
