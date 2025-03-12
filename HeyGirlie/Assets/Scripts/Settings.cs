using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System;
using System.IO;
using System.Collections.Generic;
using TMPro;

public class Settings : MonoBehaviour
{
    [SerializeField] private Texture2D appleCursor;
    [SerializeField] private Texture2D applebeeCursor;
    [SerializeField] private Texture2D beeCursor;
    private CursorMode cursorMode = CursorMode.Auto;
    private Vector2 hotSpot = Vector2.zero;
    
    [SerializeField] private GameObject settingsMenu;
    [SerializeField] private GameObject saveProfilesMenu;

    public void OpenSaves(){
        Instantiate(saveProfilesMenu);
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

    public void Close(){
        Destroy(settingsMenu);
    }
}
