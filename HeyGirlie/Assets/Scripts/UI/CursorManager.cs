using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using System;
using System.Collections;

public class CursorManager : MonoBehaviour
{
    private static CursorManager _instance;
    public static CursorManager Instance {get {return _instance;}}

    [SerializeField] private GameObject _eventSystem;
    public GameObject EventSystem {
        get {return _eventSystem;}
        set { _eventSystem = value; }
    }

    private Func<bool> _action;
    public Func<bool> Action {
        get {return _action;}
        set {_action = value;}
    }

    [SerializeField] private Texture2D[] cursors;
    [SerializeField] private Texture2D[] waitCursorFrames;
    [SerializeField] private CursorMode cursorMode = CursorMode.Auto;
    [SerializeField] private Vector2 cursorHotspot = Vector2.one;
    [SerializeField] private float prevCursor = 0;
    [SerializeField] private bool animateLock = false;

    private int tick = 0, frame = 0;
    void Awake(){
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        _instance = this;

        DontDestroyOnLoad(this.gameObject);
    }

    void Update(){
        if(animateLock){
            if(tick >= 45) {
                Cursor.SetCursor(waitCursorFrames[frame++], cursorHotspot, cursorMode);
                if(frame >= 15) frame = 0;
                tick = 0;
            } else tick++;
        }
    }

    public void ChangeCursor(float value){
        prevCursor = value;
        Cursor.SetCursor(cursors[(int)Math.Ceiling(value)], cursorHotspot, cursorMode);
    }

    public void Load(bool animate){
        if(!animate){
            animateLock = false;
            _eventSystem.SetActive(true);
            SettingManager.Instance.ChangeCursor(prevCursor);
        } else {
            animateLock = true;
            prevCursor = SettingManager.Instance.cursor;
            _eventSystem.SetActive(false);
        }
    }

    public void WaitCursor(Func<bool> action){
        _action = action;
        StartCoroutine(AnimateLoadCursor());
    }

    public IEnumerator AnimateLoadCursor(){
        Load(true);
        yield return new WaitForSeconds(0.5f);
        yield return new WaitUntil(_action);
    }
}
