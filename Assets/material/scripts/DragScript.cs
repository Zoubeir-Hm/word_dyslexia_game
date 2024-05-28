using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragScript : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler 
{
    public static DragScript currentLetterDrag;
    [SerializeField] TMPro.TextMeshProUGUI letterDisplay;

    private bool isHint, isFilled;
    private Vector3 initialPosition;
    private Transform initialParent;

    public string Letter {get; private set;}

    public void Initialize(Transform parent, string letter, bool isHint)
    {
        Letter = letter;
        Debug.Log(Letter);
        transform.SetParent(parent);
        if (letterDisplay != null) {
            letterDisplay.SetText(Letter);
        } else {
            Debug.LogError("TextMeshPro component is not assigned to letterDisplay");
        }
        this.isHint = isHint;
        GetComponent<CanvasGroup>().alpha = isHint ? 0.5f : 1f;
    }

    public void Match(Transform parent)
    {
        transform.SetParent(parent);
        transform.localPosition = Vector3.zero;
        isHint = true;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (isHint)
            return;
        initialPosition = transform.position;
        initialParent = transform.parent;
        currentLetterDrag = this;
        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isHint)
            return;
        transform.position = Input.mousePosition;
    }

    public void OnDrop(PointerEventData eventData)
    {
        if(isHint && !isFilled)
        {
            if(currentLetterDrag.Letter == Letter)
            {
                WordManager.Instance.AddPoint();
                currentLetterDrag.Match(transform);
                isFilled = true;
                GetComponent<CanvasGroup>().alpha = 1f;
            }
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (isHint)
            return;
        currentLetterDrag = null;

        if (transform.parent == initialParent)
        {
            transform.position = initialPosition;
        }
        GetComponent<CanvasGroup>().blocksRaycasts = true;
    }

    public void SetAlpha(float alpha)
    {
        var canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup != null)
        {
            canvasGroup.alpha = alpha;
        }
        else
        {
            Debug.LogError("CanvasGroup component is missing!");
        }
    }

}
