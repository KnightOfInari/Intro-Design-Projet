﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityStandardAssets.ImageEffects;


public class GameManager : MonoBehaviour
{
    public static GameManager gameManager;
    private static AudioManager audioManager;

    private ColorCorrectionCurves colorCorrection;

    [SerializeField]
    private GameObject playerPrefab = null;
    private GameObject Player;
    [SerializeField]
    private GameObject GameOverUI = null;
    private Vector3 PlayerPosition;
    private int MaxHealth = 10;
    private int CurrentHealth;

    private string badGuyToPacify;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        CurrentHealth = MaxHealth;
        GameOverUI.SetActive(false);
        audioManager = AudioManager.instance;
    }

    public void BlockPlayer()
    {
        Player.GetComponent<MoveScript>().MovingAllowed(false);
    }

    public void PlayerCanAnswer(string badGuyName, ColorCorrectionCurves curves)
    {
        colorCorrection = curves;
        badGuyToPacify = badGuyName;
        Player.GetComponent<MoveScript>().ShowUI(true);
    }

    public void UnblockPlayer()
    {
        Player.GetComponent<MoveScript>().MovingAllowed(true);
        Player.GetComponent<MoveScript>().ShowUI(false);

    }

    public void PlayerFight(Vector3 playerPosition)
    {
        PlayerPosition = playerPosition;
        SceneManager.LoadScene(2);
    }

    public void ReturnFromFight()
    {
        SceneManager.LoadScene(1);
        StartCoroutine(SpawnPlayerAtPosition());
    }

    private IEnumerator SpawnPlayerAtPosition()
    {
        yield return new WaitForSeconds(1f);
        GameObject.Find(badGuyToPacify).GetComponent<AgressionScript>().aggression = false;
        Player = Instantiate(playerPrefab, PlayerPosition, Quaternion.identity);
        Camera mainCamera = Player.GetComponentInChildren<Camera>();
        mainCamera.GetComponent<ColorCorrectionCurves>().saturation = colorCorrection.saturation;
        UnblockPlayer();
    }

    public void EndGame()
    {
        SceneManager.LoadScene(3);
        StopAllSounds();
    }

    public void Play()
    {
        SceneManager.LoadScene(1);
        if (audioManager != null)
            StartSounds();
        else
        {
            SearchAudio();
        }
        StartCoroutine(PlayerFirstSpawn());
    }

    private IEnumerator PlayerFirstSpawn()
    {
        yield return new WaitForSeconds(1);

        Player = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
    }

    private void StartSounds()
    {
        if (audioManager != null)
        {
            audioManager.StopAllSounds();
            audioManager.PlaySound("Level1");
        }
        else
        {
            Debug.LogWarning("GameManager doesn't have an audioManager");
            InvokeRepeating("SearchAudio", 0f, 0.5f);
        }

    }

    public void SearchAudio()
    {
        audioManager = FindObjectOfType<AudioManager>();
        if (audioManager != null)
        {
            Debug.Log("audioManager found for game manager");
            CancelInvoke();
            StartSounds();
        }
    }

    IEnumerator StopAllSounds()
    {
        yield return new WaitForSeconds(3);
        AudioManager.instance.StopAllSounds();
    }

    public void Retry()
    {
        CurrentHealth = MaxHealth;
        GameOverUI.SetActive(false);
        Play();
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void DamagePlayer(int damage)
    {
        CurrentHealth -= damage;
        if (CurrentHealth <= 0)
        {
            GameOver();
        }
    }

    public void GameOver()
    {
        if (GameOverUI.activeSelf == false)
            GameOverUI.SetActive(true);
        else
            GameOverUI.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            GameOver();
        }

        if (Player == null)
        {
            Player = GameObject.FindGameObjectWithTag("Player");
        }
    }
}
