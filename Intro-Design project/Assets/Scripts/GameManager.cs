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
    private GameObject playerPrefab;
    private GameObject Player;
    [SerializeField]
    private GameObject GameOverUI;
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
        SceneManager.LoadSceneAsync(2);
    }

    public void ReturnFromFight()
    {
        SceneManager.LoadSceneAsync(1);
        StartCoroutine(SpawnPlayerAtPosition());
    }

    private IEnumerator SpawnPlayerAtPosition()
    {
        yield return new WaitForSeconds(1);
        GameObject.Find(badGuyToPacify).GetComponent<AgressionScript>().aggression = false;
        Player = Instantiate(playerPrefab, PlayerPosition, Quaternion.identity);
        Camera mainCamera = Player.GetComponentInChildren<Camera>();
        mainCamera.GetComponent<ColorCorrectionCurves>().saturation = colorCorrection.saturation;

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

    void GameOver()
    {
        GameOverUI.SetActive(true);
        BlockPlayer();
        Time.timeScale = 0; // stop toutes les actions basees sur le temps. Plus ou moins une pause
    }

    // Update is called once per frame
    void Update()
    {
        if (Player == null)
        {
            Debug.LogWarning("No player found");
            Player = GameObject.FindGameObjectWithTag("Player");
        }
    }
}
