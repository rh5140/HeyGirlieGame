using UnityEngine;
using System.Collections.Generic;

[System.Serializable] public class MultiSpriteContainer : MonoBehaviour
{
    [SerializeField] public SpriteDictionary[] spriteDicts;
    public Dictionary<string, Sprite> multiSpriteDict;

    public void Awake()
    {
        multiSpriteDict = new Dictionary<string, Sprite>();
        foreach (SpriteDictionary sd in spriteDicts)
        {
            foreach (var element in sd.spriteDict)
            {
                multiSpriteDict.Add(element.Key, element.Value);
            }
        }
    }
}
