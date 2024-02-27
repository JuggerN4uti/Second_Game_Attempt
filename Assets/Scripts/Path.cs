using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Path : MonoBehaviour
{
	[Header("Scripts")]
	public PlayerMovement Player;
	public EnemyEncounters Encounters;

	[Header("UI")]
	public GameObject ForwardTile;
	public GameObject AmbushTile;
	public GameObject PathChoice, Land;
	public GameObject[] Events;
	public Sprite[] TileSprites;
	public Sprite TreasureSprite, EliteSprite, BattleSprite;
	public Image[] EventImages;
	public Image TileEndImage;
	public int tile, possibilities;
	public int[] chances;
	public int[] rolls, chosenPath;
	public PathEvents[] Paths;
	public bool campFatigue, shopFatigue;
	public char end;
	int totalChance, roll, tempi;
	bool viable;

	[Header("Events")]
	public ObeliskHud Waygate;
	public CacheHud Stash;
	public AltarHud Altar;
	public CampHud Camp;
	public MerchantHud Shop;

	[Header("Ambush/Fights")]
	public int tileCount;
	public float dangerLevel;
	float temp;

	void Start()
    {
		SumUpChances();
		RollPaths();
	}

	void SumUpChances()
    {
		totalChance = 0;
		for (int i = 0; i < 6; i++)
		{
			totalChance += chances[i];
		}
	}

	void Update()
    {
		/*if (Input.GetKeyDown(KeyCode.F))
			MoveForward();*/
	}

	public void MoveForward()
	{
		tile++;
		RollPaths();
		ForwardTile.SetActive(false);
	}

	void RollPaths()
	{
		PathChoice.SetActive(true);
		/*if (treasure >= 9)
			SetTreasure();*/
		for (int i = 0; i < 3; i++)
		{
			/*viable = false;
			while (!viable)
			{
				rolls[0] = Random.Range(0, TileSprites.Length);
				if (rolls[0] > 3)
				{
					if (rolls[0] == 4 && shopFatigue)
						viable = false;
					else viable = true;

					if (rolls[0] == 5 && campFatigue)
						viable = false;
					else viable = true;
				}
				else viable = true;
			}

			viable = false;
			while (!viable)
			{
				rolls[1] = Random.Range(0, TileSprites.Length);
				if (rolls[1] > 3)
				{
					if (rolls[1] == 4 && shopFatigue)
						viable = false;
					else
					{
						if (rolls[1] != rolls[0])
							viable = true;
					}

					if (rolls[1] == 5 && campFatigue)
						viable = false;
					else
					{
						if (rolls[1] != rolls[0])
							viable = true;
					}
				}
				else
				{
					if (rolls[1] != rolls[0])
						viable = true;
				}
			}

			viable = false;
			while (!viable)
			{
				rolls[2] = Random.Range(0, TileSprites.Length);
				if (rolls[2] > 3)
				{
					if (rolls[2] == 4 && shopFatigue)
						viable = false;
					else
					{
						if (rolls[2] != rolls[0] && rolls[2] != rolls[1])
							viable = true;
					}

					if (rolls[2] == 5 && campFatigue)
						viable = false;
					else
					{
						if (rolls[2] != rolls[0] && rolls[2] != rolls[1])
							viable = true;
					}
				}
				else
				{
					if (rolls[2] != rolls[0] && rolls[2] != rolls[1])
						viable = true;
				}
			}*/

			for (int j = 0; j < 3; j++)
            {
				Paths[i].events[j] = SetEvent();
				Paths[i].TileImages[j].sprite = TileSprites[Paths[i].events[j]];
			}
		}
	}

	int SetEvent()
    {
		roll = 0;
		tempi = Random.Range(1, totalChance + 1);
		while (tempi > chances[roll])
        {
			tempi -= chances[roll];
			roll++;
        }
		return roll;
    }

	void SetTreasure()
	{
		/*treasure -= 9;
		rolls[1] = possibilities;
		Tiles[1].SetActive(true);
		TileImages[1].sprite = TileSprites[possibilities];
		Tiles[0].SetActive(false); Tiles[2].SetActive(false);*/
	}

	public void ChoosePath(int choice)
	{
		PathChoice.SetActive(false);
		tile++;

		if (tile == 4 || tile == 12)
		{
			TileEndImage.sprite = TreasureSprite;
			end = 't';
		}
		else if (tile == 8 || tile == 16)
		{
			TileEndImage.sprite = EliteSprite;
			end = 'e';
		}
		else
		{
			TileEndImage.sprite = BattleSprite;
			end = 'b';
		}

		for (int i = 0; i < 3; i++)
        {
			chosenPath[i] = Paths[choice].events[i];
			EventImages[i].sprite = TileSprites[Paths[choice].events[i]];
			Events[i].SetActive(true);
		}

		Land.SetActive(true);

		/*
		if (rolls[choice] == 0)
        {
			if (Random.Range(1, 126) < ambushChance)
				Ambushed();
			else
            {
				ambushChance += 2 + ambushChance / 10;
				ForwardTile.SetActive(true);
			}
        }
		else if (rolls[choice] == 5 && rolls[choice] == 6)
        {
			if (Random.Range(1, 151) < ambushChance)
				Ambushed();
			else
			{
				ambushChance += 1 + ambushChance / 30;
				ForwardTile.SetActive(true);
			}
		}
		else
        {
			if (Random.Range(1, 101) < ambushChance)
				Ambushed();
			else
			{
				ambushChance += 4 + ambushChance / 3;
				ForwardTile.SetActive(true);
			}
		}*/
	}

	public void ChooseEvent(int which)
    {
		Events[which].SetActive(false);

		switch (chosenPath[which])
		{
			case 0:
				Waygate.FoundWaygate();
				break;
			case 1:
				Stash.FoundStash();
				break;
			case 2:
				Altar.FoundAltar();
				break;
			case 3:
				dangerLevel += 0.25f;
				SetFight(false, false);
				break;
			case 4:
				Shop.Display();
                for (int i = 0; i < 4; i++)
                {
					chances[i]++;
                }
				chances[5]++;
				SumUpChances();
				break;
			case 5:
				Camp.SetCamp();
				for (int i = 0; i < 5; i++)
				{
					chances[i]++;
				}
				SumUpChances();
				break;
			case 6:
				SetFight(false, true);
				break;
		}
	}

	/*void Ambushed()
    {
		AmbushTile.SetActive(true);
		ambushChance = 8;
	}

	public void FaceAmbush()
    {
		AmbushTile.SetActive(false);
		SetFight(true, false);
		ForwardTile.SetActive(true);
	}*/

	public void NextTile()
    {
		dangerLevel += 0.75f + 0.05f * tileCount;
		if (end == 't')
			Stash.FoundTreasure();
		else if (end == 'e')
			SetFight(true, true);
		else SetFight(true, false);

		tileCount++;

		Land.SetActive(false);
		RollPaths();
	}

	void SetFight(bool next, bool elite)
    {
		if (elite)
			Encounters.Elite(tileCount / 8);
		else
        {
			if (!next)
				temp = dangerLevel * 0.75f;
			else temp = dangerLevel;

			if (temp < 2f)
				Encounters.SetSmallBattle(temp);
			else Encounters.SetBattle(temp);
			/*
			switch (CheckDifficulty(temp))
			{
				case 1:
					Encounters.Easy();
					break;
				case 2:
					Encounters.Medium();
					break;
				case 3:
					Encounters.Hard();
					break;
					//case 4:
					// mo¿e elite, idk
					//break;
			}*/
		}

		Player.freeToMove = false;
		Encounters.CombatScript.StartCombat();
	}

	int CheckDifficulty(float danger)
    {
		if (danger < 2.5f)
			return 1;
		else if (danger < 5.5f)
			return 2;
		else return 3;
    }
}
