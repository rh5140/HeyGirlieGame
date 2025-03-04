using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System;
using System.IO;
using System.Collections.Generic;
using TMPro;

public class Settings : MonoBehaviour
{
    public Texture2D appleCursor;
    public Texture2D applebeeCursor;
    public Texture2D beeCursor;
    public CursorMode cursorMode = CursorMode.Auto;
    public Vector2 hotSpot = Vector2.zero;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenSaves(){
        SceneManager.LoadScene("Save Profiles");
    }

    public void ChangeCursor(float value){
        int cursor = (int)Math.Ceiling(value);
        switch(cursor){
            case 0:
                Cursor.SetCursor(appleCursor, Vector2.zero, cursorMode);
                break;
            case 1:
                Cursor.SetCursor(applebeeCursor, Vector2.zero, cursorMode);
                break;
            case 2:
                Cursor.SetCursor(beeCursor, Vector2.zero, cursorMode);
                break;
            default:
                Cursor.SetCursor(null, Vector2.zero, cursorMode);
                break;
        }
    }
}
