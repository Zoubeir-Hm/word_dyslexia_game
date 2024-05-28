using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CharObject : MonoBehaviour
{
    public Image image;
    public char character;
    public Text text;
    public RectTransform rectTransform;
    public int index;

    [Header("Appearance")]
    public Color normalColor;
    public Color selectedColor;

    bool isSelected = false;

    public CharObject Init(char c)
    {
        character = c;
        text.text = c.ToString();
        
        gameObject.SetActive(true);
        return this;
    }

    public void Select ()
    {
        isSelected = !isSelected;
        
        if(isSelected == true)
        {
            WordScramble.main.Select(this);
        } else {
           
            WordScramble.main.UnSelect();
        }

    }
    
    
}
