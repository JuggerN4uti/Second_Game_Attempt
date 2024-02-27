using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Scripts")]
    public GameObject Player;
    public Rigidbody2D playerBody;
    public PlayerMovement PlayerScript;

    [Header("Mob Info")]
    public int[] enemyTypes;
    public int enemyCount, experienceGranted, silverGranted;
    public bool itemGranted;

    [Header("Effects")]
    public GameObject GlowingEyes;

    [Header("Movement")]
    public Transform LastScentLocation;
    public float detectionRange, detectionRangeIncrease;
    public float movementSpeed, chaseSpeed;
    bool chasing, searching;

    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        playerBody = Player.GetComponent<Rigidbody2D>();
        PlayerScript = Player.GetComponent(typeof(PlayerMovement)) as PlayerMovement;

        Invoke("Sniff", 3f);
        Invoke("Flash", 3f);
    }

    void Update()
    {
        if (PlayerScript.freeToMove)
        {
            if (chasing)
                transform.position = Vector2.MoveTowards(transform.position, Player.transform.position, movementSpeed * Time.deltaTime);
            else if (searching)
                transform.position = Vector2.MoveTowards(transform.position, LastScentLocation.position, movementSpeed * Time.deltaTime);
        }
    }

    void Sniff()
    {
        if (Vector3.Distance(transform.position, Player.transform.position) <= detectionRange)
            Chase();
        else
        {
            searching = true;
            if (PlayerScript.freeToMove)
                detectionRange += detectionRangeIncrease;
            LastScentLocation.position = new Vector2(Player.transform.position.x + Random.Range(-1.5f, 1.5f), Player.transform.position.y + Random.Range(-1.5f, 1.5f));
            Invoke("LoseScent", 2f);
        }
    }

    void LoseScent()
    {
        searching = false;
        Invoke("Sniff", 5f);
    }

    void Chase()
    {
        chasing = true;
        movementSpeed *= chaseSpeed;
    }

    void Flash()
    {
        GlowingEyes.SetActive(true);
        Invoke("CloseEyes", 0.2f);
    }

    void CloseEyes()
    {
        GlowingEyes.SetActive(false);
        Invoke("Flash", Random.Range(3.5f, 6.8f));
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.tag == "Player")
        {
            switch (enemyCount)
            {
                case 1:
                    PlayerScript.CombatScript.SetOneEnemy(enemyTypes[0]);
                    break;
                case 2:
                    PlayerScript.CombatScript.SetTwoEnemies(enemyTypes[0], enemyTypes[1]);
                    break;
            }
            //PlayerScript.experienceFromCombat = experienceGranted;
            //PlayerScript.silverFromCombat = silverGranted;
            //PlayerScript.itemFromCombat = itemGranted;
            PlayerScript.StartCombat();
            Destroy(gameObject);
        }
    }
}
