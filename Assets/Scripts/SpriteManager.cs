using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteManager : MonoBehaviour
{
    private static Dictionary<string, Sprite> cachSprites
        = new Dictionary<string, Sprite>();

    public static Sprite[] Load()
    {
        Sprite[] sprites = Resources.LoadAll<Sprite>(Constant.kPhotoFolderName);

        foreach (Sprite sprite in sprites)
        {
            if (!cachSprites.ContainsKey(sprite.name))
            {
                cachSprites.Add(sprite.name , sprite);
            }
        }
        return sprites;
    }

    public static Sprite GetSprite(string name)
    {
        if (!cachSprites.ContainsKey(name))
        {
            Sprite sprite = Resources.Load<Sprite>(Constant.kPhotoFolderName + name);

            if (sprite)
                cachSprites.Add(sprite.name, sprite);

            return sprite;
        }
        else
        {
            return cachSprites[name];
        }
    }
}
