using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public PlayerScript Player;
    public GameObject powerUp;
    // Start is called before the first frame update
    private void Start()
    {
        if (Player == null)
        {
            FindObjectOfType<PlayerScript>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Player == null)
        {
            FindObjectOfType<PlayerScript>();
        }
        else if (Player.isDashUnlocked)
        {
            powerUp.SetActive(false);
        }
    }
}
