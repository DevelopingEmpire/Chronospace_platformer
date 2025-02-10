using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyConditionalSwitchRecaller : StageMechanicsController
{
    [SerializeField] LobbyConditonalSwitchController csc;

    public override int Idx { get; set; }

    public override void Exit()
    {
        Debug.Log("This object does not support Exit method.");
    }

    public override void SetInitialColor(Material targetColor, Material targetColorGlow)
    {
        Debug.Log("This object does not support SetInitialColor method.");
    }

    public override void Trigger()
    {
        csc.DoActivationCheckout();
    }
}
