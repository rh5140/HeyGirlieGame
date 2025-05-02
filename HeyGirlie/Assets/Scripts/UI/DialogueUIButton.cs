using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections;

public class DialogueUIButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private GameObject button;
    [SerializeField] private UnityEvent buttonAction;
    private RectTransform transform;
    private bool selected = false;
    [SerializeField] private bool stayOpen = false;
    void Awake(){
        transform = button.GetComponent<RectTransform>(); 
    }

    public void OnPointerEnter(PointerEventData eventData){
        if(!selected) StartCoroutine(AnimateButton(true));
    }

    public void OnPointerExit(PointerEventData eventData){
        if(!selected) StartCoroutine(AnimateButton(false));
    }

    public void Selected(){
        if(stayOpen || !selected) buttonAction.Invoke();
        // if(!selected) buttonAction.Invoke();

        if(stayOpen) selected = !selected;
        else StartCoroutine(AnimateButton(false));
    }

    IEnumerator AnimateButton(bool open){
        float time = 0, lerpTime = 0.25f;
        float start = 65f, end = 0f;

        while(time < lerpTime){
            transform.anchoredPosition = new Vector2(0, (open) ? Mathf.Lerp(start, end, time / lerpTime) : Mathf.Lerp(end, start, time / lerpTime));

            time += Time.deltaTime;
            yield return null;
        }
    }
}
