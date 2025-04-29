using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections;

public class CursorAnimation : MonoBehaviour
{
    [SerializeField] private Texture2D[] waitCursorFrames;
    [SerializeField] private float prevCursor = 1;
    [SerializeField] private bool animate = false;

    public void Load(){
        if(animate){
            animate = false;
            SettingManager.Instance.ChangeCursor(prevCursor);
        } else {
            animate = true;
            prevCursor = SettingManager.Instance.cursor;
            StartCoroutine(AnimateLoadCursor());
        }
    }

    IEnumerator AnimateLoadCursor(){
        int i = 0;
        while(animate){
            Cursor.SetCursor(waitCursorFrames[i++], Vector2.zero, CursorMode.Auto);
            if(i >= 15) i = 0;

            yield return new WaitForSeconds(Time.deltaTime * 45);
        }
    }
}
