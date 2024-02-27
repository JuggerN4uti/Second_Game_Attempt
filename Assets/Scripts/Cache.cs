using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cache : MonoBehaviour
{
    public GameObject Player;
    public PlayerMovement PlayerScript;

    public int[] SilverRange, ExperienceRange;
    public int Silver, Experience;
    public bool Item;

    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        PlayerScript = Player.GetComponent(typeof(PlayerMovement)) as PlayerMovement;
        Silver = Random.Range(SilverRange[0], SilverRange[1] + 1);
        Experience = Random.Range(ExperienceRange[0], ExperienceRange[1] + 1);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.tag == "Player")
        {
            PlayerScript.freeToMove = false;
            //PlayerScript.Cache.Display(Silver, Experience, Item);
            Destroy(gameObject);
        }
    }
}
