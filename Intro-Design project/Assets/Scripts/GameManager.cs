using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager gameManager;

    [SerializeField]
    private GameObject Player;
    [SerializeField]
    private GameObject GameOverUI;

    private int MaxHealth = 10;
    private int CurrentHealth;

    // Start is called before the first frame update
    void Start()
    {
        CurrentHealth = MaxHealth;
        GameOverUI.SetActive(false);
    }

    public void BlockPlayer()
    {
        Player.GetComponent<MoveScript>().MovingAllowed(false);
    }

    public void PlayerCanAnswer()
    {
        Player.GetComponent<MoveScript>().ShowUI(true);
    }

    public void UnblockPlayer()
    {
        Player.GetComponent<MoveScript>().MovingAllowed(true);
        Player.GetComponent<MoveScript>().ShowUI(false);

    }


    public void DamagePlayer(int damage)
    {
        CurrentHealth -= damage;
        if (CurrentHealth <= 0)
        {
            GameOver();
        }
    }

    void GameOver()
    {
        GameOverUI.SetActive(true);
        BlockPlayer();
        Time.timeScale = 0; // stop toutes les actions basees sur le temps. Plus ou moins une pause
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.DownArrow)) //section pour tester le gameOver
        {
            GameOver();
        }
        if (Player == null)
        {
            Debug.LogWarning("No player found");
            Player = GameObject.FindGameObjectWithTag("Player");
        }
    }
}
