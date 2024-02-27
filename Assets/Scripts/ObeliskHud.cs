using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObeliskHud : MonoBehaviour
{
    public GameObject WaygateHudObject;
    public PlayerMovement Player;

    public TMPro.TextMeshProUGUI ShardsLeft, ShardsNeeded, ForgeNeeded;
    public Button WaygateButton, ForgeButton;

    public int slots, forgeRequire, bonus;
    int roll, required, toForge;

    public void FoundWaygate()
    {
        WaygateHudObject.SetActive(true);

        roll = Random.Range(3, 7);
        required = slots - roll;
        toForge = forgeRequire - roll - bonus;

        ShardsLeft.text = "+ " + roll.ToString("0");
        ShardsNeeded.text = "- " + required.ToString("0");
        ForgeNeeded.text = "- " + toForge.ToString("0");

        if (Player.shards >= required)
            WaygateButton.interactable = true;
        else WaygateButton.interactable = false;

        if (Player.shards >= toForge)
            ForgeButton.interactable = true;
        else ForgeButton.interactable = false;
    }

    public void TakeShards()
    {
        Player.GainShards(roll);

        Leave();
    }

    public void CompleteWaygate()
    {
        Player.SpendShards(required);
        Player.GainSP(1);
        slots += 2;
        bonus += 3;

        Leave();
    }

    public void ForgeAnArcaneRune()
    {
        Player.SpendShards(toForge);
        Player.GainRunes(1);
        forgeRequire += 5;

        Leave();
    }

    public void Leave()
    {
        WaygateHudObject.SetActive(false);
        Player.freeToMove = true;
    }
}
