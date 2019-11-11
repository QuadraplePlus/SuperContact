using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteManager : MonoBehaviour
{
    //스프라이트를 파일이름을 키로 묶어 저장함
    private static Dictionary<string, Sprite> cachedSprites
        = new Dictionary<string, Sprite>();
    
    //스프라이트 로드
    public static Sprite[] Load()
    {
        Sprite[] sprites = Resources.LoadAll<Sprite>(Constant.kPhotoFolderName);

        foreach (Sprite sprite in sprites)
        {
            if (!cachedSprites.ContainsKey(sprite.name))
            {
                cachedSprites.Add(sprite.name , sprite);
            }
        }
        return sprites;
    }
    //스프라이트가 캐쉬 된 상태라면 로드하지않고 캐쉬된 스프라이트를 가져옴
    public static Sprite GetSprite(string name)
    {
        if (!cachedSprites.ContainsKey(name))
        {
            Sprite sprite = Resources.Load<Sprite>("photo/" + name);
            if (sprite) cachedSprites.Add(sprite.name, sprite);

            return sprite;
        }
        else
        {
            return cachedSprites[name];
        }
    }
}
