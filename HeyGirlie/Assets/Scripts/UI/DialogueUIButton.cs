using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections;

public class DialogueUIButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private GameObject button;
    [SerializeField] private UnityEvent buttonAction;
    [SerializeField] private KeyCode buttonKey;
    private RectTransform transform;
    private EventTrigger eventTrigger;
    private bool selected = false;
    [SerializeField] private bool stayOpen = false;
    void Awake(){
        transform = button.GetComponent<RectTransform>(); 
        eventTrigger = gameObject.GetComponent<EventTrigger>();
    }

    void Update(){
        if(Input.GetKeyUp(buttonKey)) {
            if(stayOpen) {
                selected = !selected;
                StartCoroutine(AnimateButton(selected));
            }
            else StartCoroutine(AnimateButton(false));

            eventTrigger.OnSubmit(null);
        }
    }

    public void OnPointerEnter(PointerEventData eventData){
        eventTrigger.OnSelect(null);
        if(!selected) StartCoroutine(AnimateButton(true));
    }

    public void OnPointerExit(PointerEventData eventData){
        if(!selected) StartCoroutine(AnimateButton(false));
    }

    public void Selected(){
        if(stayOpen || !selected) buttonAction.Invoke();

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
