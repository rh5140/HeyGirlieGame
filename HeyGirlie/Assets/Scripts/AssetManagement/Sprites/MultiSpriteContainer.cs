using UnityEngine;
using System.Collections.Generic;

[System.Serializable] public class MultiSpriteContainer : MonoBehaviour
{
    [SerializeField] public SpriteDictionary[] spriteDicts;
    public Dictionary<string, Sprite> multiSpriteDict;

    public void Start()
    {
        multiSpriteDict = new Dictionary<string, Sprite>();
        foreach (SpriteDictionary sd in spriteDicts)
        {
            foreach (var element in sd.spriteDict)
            {
                if (!multiSpriteDict.ContainsKey(element.Key))
                    multiSpriteDict.Add(element.Key, element.Value);
            }
        }
    }
}
