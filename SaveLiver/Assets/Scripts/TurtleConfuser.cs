﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TurtleConfuser : TurtleFollow
{
    public override void OnDead(bool getLiver)
    {
        base.OnDead(getLiver);
        if (getLiver == true)
        {
            Player.instance.ConfusePlayer();
        }
    }
}
