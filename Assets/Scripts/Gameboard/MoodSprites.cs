using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoodSprites : MonoBehaviour
{
    public Sprite[] ActionSpriteList;
    public Sprite[] MoodSpriteList;
    public Sprite VillagerActionSprites(int val)
    {
        // 0 - idle
        // 1 - towncenter(homeless)
        // 2 - home
        // 3 - workshop

        return ActionSpriteList[val];
    }


    public Sprite VillagerMoodSprites(int val)
    {
        return MoodSpriteList[val];
    }
}
