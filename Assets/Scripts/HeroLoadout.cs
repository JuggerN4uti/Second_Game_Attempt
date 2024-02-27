using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HeroLoadout : MonoBehaviour
{
    public TMPro.TextMeshProUGUI[] HeroStats, UnitCounts;
    public TMPro.TextMeshProUGUI LeadershipLeft;
    public Button[] AddUnitButton, SubtractUnitButton;
    public Button ProceedButton;
    public int Leadership, HP, AD, MP, IN, AR, Class, Footmen, Marksmen, Cavalry, Mages;
    public int[] units;
    public int[] bHP, bAD, bMP, bIN, bAR;
    public int[] uHP, uAD, uMP, uIN, uAR;

    void Start()
    {
        HP = bHP[Class]; AD = bAD[Class]; MP = bMP[Class]; IN = bIN[Class]; AR = bAR[Class];

        UpdateHud();
    }

    void UpdateHud()
    {
        LeadershipLeft.text = Leadership.ToString("");
        HeroStats[0].text = HP.ToString("");
        HeroStats[1].text = AD.ToString("");
        HeroStats[2].text = MP.ToString("");
        HeroStats[3].text = IN.ToString("");
        HeroStats[4].text = AR.ToString("");

        for (int i = 0; i < 4; i++)
        {
            UnitCounts[i].text = units[i].ToString("");

            if (Leadership > 0)
                AddUnitButton[i].interactable = true;
            else AddUnitButton[i].interactable = false;

            if (units[i] > 0)
                SubtractUnitButton[i].interactable = true;
            else SubtractUnitButton[i].interactable = false;
        }

        if (Leadership == 0)
            ProceedButton.interactable = true;
        else ProceedButton.interactable = false;
    }

    public void AddUnit(int which)
    {
        units[which]++;
        Leadership--;

        HP += uHP[which + Class * 4];
        AD += uAD[which + Class * 4];
        MP += uMP[which + Class * 4];
        IN += uIN[which + Class * 4];
        AR += uAR[which + Class * 4];

        UpdateHud();
    }

    public void SubtractUnit(int which)
    {
        units[which]--;
        Leadership++;

        HP -= uHP[which + Class * 4];
        AD -= uAD[which + Class * 4];
        MP -= uMP[which + Class * 4];
        IN -= uIN[which + Class * 4];
        AR -= uAR[which + Class * 4];

        UpdateHud();
    }

    public void Proceed()
    {
        PlayerPrefs.SetInt("HP", HP);
        PlayerPrefs.SetInt("AD", AD);
        PlayerPrefs.SetInt("MP", MP);
        PlayerPrefs.SetInt("IN", IN);
        PlayerPrefs.SetInt("AR", AR);
        PlayerPrefs.SetInt("Footmen", units[0]);
        PlayerPrefs.SetInt("Marksmen", units[1]);
        PlayerPrefs.SetInt("Cavalry", units[2]);
        PlayerPrefs.SetInt("Mages", units[3]);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
