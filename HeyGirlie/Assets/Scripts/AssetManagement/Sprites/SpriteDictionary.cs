using UnityEngine;
using System.Collections.Generic;

[System.Serializable] public class SpriteDictionary : MonoBehaviour
{
    [SerializeField] public SpriteDictionaryItem[] spriteArray;
    public Dictionary<string, Sprite> spriteDict;

    public void Awake()
    {
        spriteDict = new Dictionary<string, Sprite>();
        foreach (SpriteDictionaryItem s in spriteArray)
        {
            spriteDict.Add(s.spriteName, s.sprite);
        }
    }
}
