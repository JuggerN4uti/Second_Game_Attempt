using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Torch : MonoBehaviour
{
    public GameObject Light;
    public SpriteRenderer image;
    public Sprite litTorch;
    bool lit;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.tag == "Player" && lit == false)
            Lit();
    }

    void Lit()
    {
        lit = true;
        image.sprite = litTorch;
        Light.SetActive(true);
    }
}
