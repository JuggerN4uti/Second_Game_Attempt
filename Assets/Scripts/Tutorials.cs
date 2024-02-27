using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorials : MonoBehaviour
{
    public PlayerMovement Player;
    public GameObject[] TutorialsObject;
    public GameObject AbilityHud, FirstCombatTooltip, NewAbilityTooltip;
    public bool On;
    int progress;

    void Start()
    {
        if (On)
        {
            Player.freeToMove = false;
            AbilityHud.SetActive(false);
            for (int i = 0; i < TutorialsObject.Length; i++)
            {
                TutorialsObject[i].SetActive(true);
            }
        }
    }

    public void Progress()
    {
        if (On)
        {
            progress++;

            if (progress == 1)
            {
                AbilityHud.SetActive(true);
                FirstCombatTooltip.SetActive(true);
            }
            else if (progress == 2)
                NewAbilityTooltip.SetActive(true);
        }
    }
}
