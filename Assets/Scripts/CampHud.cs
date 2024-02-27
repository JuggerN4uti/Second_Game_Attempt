using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CampHud : MonoBehaviour
{
    public GameObject CampHudObject;
    public PlayerMovement Player;

    public TMPro.TextMeshProUGUI ActionsValue, HealthRestoreValue, experienceGainValue;

    public int actions;
    public int healthRestore, experienceGain;

    public void SetCamp()
    {
        CampHudObject.SetActive(true);

        actions = 2;

        if (Player.itemCheck[14])
            Player.GainBones(3);
        if (Player.itemCheck[22])
            actions++;

        UpdateValues();
    }

    void UpdateValues()
    {
        ActionsValue.text = actions.ToString("0");

        healthRestore = (Player.maxHealth * 3) / 22;
        HealthRestoreValue.text = "Restore " + healthRestore.ToString("0") + " Health";

        experienceGain = 7 + Player.level * 2;
        experienceGainValue.text = "Gain " + experienceGain.ToString("0") + " Experience";
    }

    public void Rest()
    {
        Player.RestoreHealth(healthRestore);

        actions--;
        if (actions == 0)
            Leave();

        else UpdateValues();
    }

    public void Feast()
    {
        Player.GainHP(3);

        actions--;
        if (actions == 0)
            Leave();

        else UpdateValues();
    }

    public void Train()
    {
        Player.GainExperience(experienceGain);

        actions--;
        if (actions == 0)
            Leave();

        else UpdateValues();
    }

    public void Leave()
    {
        CampHudObject.SetActive(false);
        Player.freeToMove = true;
    }
}
