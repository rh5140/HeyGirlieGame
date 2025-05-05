using UnityEngine;
using UnityEngine.EventSystems;

public class EventSystemManager : MonoBehaviour
{
    void Awake()
    {
        CursorManager.Instance.EventSystem = gameObject;
    }
}
