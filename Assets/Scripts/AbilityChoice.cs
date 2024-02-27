using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityChoice : MonoBehaviour
{
    public AbilitiesLibrary library;
    public AbilitiesLoadout HeroLoadout;
    public Image[] AbilityIcons;
    public GameObject[] AbilityTooltips;
    public TMPro.TextMeshProUGUI[] AbilityLevel;
    public TMPro.TextMeshProUGUI ChoicesCount;

    public int choicesLeft;
    public int[] rolls;
    public Button ProceedButton;
    public GameObject AbilityChoiceHud;

    void Start()
    {
        choicesLeft = 2;
        ChoicesCount.text = choicesLeft.ToString("");
        RollAbilities();
    }

    void RollAbilities()
    {
        rolls[0] = Random.Range(0, library.LightAbilities.Length);

        do
        {
            rolls[1] = Random.Range(0, library.LightAbilities.Length);
        } while (rolls[0] == rolls[1]);

        do
        {
            rolls[2] = Random.Range(0, library.LightAbilities.Length);
        } while (rolls[0] == rolls[2] || rolls[1] == rolls[2]);

        for (int i = 0; i < 3; i++)
        {
            AbilityIcons[i].sprite = library.LightAbilities[rolls[i]].Icon;
            //AbilityTooltips[i] = library.LightAbilities[rolls[i]].Tooltip;
        }
    }

    public void ChooseAbility(int which)
    {
        HeroLoadout.ChooseAbility(rolls[which]);
        HeroLoadout.DisplayAbilities();
        choicesLeft--;
        ChoicesCount.text = choicesLeft.ToString("");
        if (choicesLeft > 0)
            RollAbilities();
        else
        {
            ProceedButton.interactable = true;
            AbilityChoiceHud.SetActive(false);
        }
    }
}
