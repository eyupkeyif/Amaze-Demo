using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EventManager
{
    public static LevelEvents levelEvents = new();
    public static PourEvents PourEvents = new();
    
}

public class PourEvents
{
    public Action<float> onPour;
}
public class LevelEvents
{
    public Action OnTilePassed;
    public Action OnLevelPassed;
    public Action OnNewLevelStart;
}
