﻿using System;
using System.Numerics;
using System.Net.Mime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Cinemachine;

public class MenuController : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject gameOver;
    public GameObject camObj;
    public CinemachineFreeLook freeLook;

    public Text gameScore;
    public Text gameOverScore;
    public Text gameOverRecord;   

    public Slider sliderX;
    public Slider sliderY;

    public Toggle crosshairToggled;

    public bool isOnPause;
    public bool isGameOver;

    public void Start()
    {
        camObj = GameObject.FindWithTag("Third Person Camera");
        freeLook = camObj.GetComponent<CinemachineFreeLook>();
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !isOnPause)
        {
            if (!isGameOver)
            {
                Debug.Log(PlayerPrefs.GetInt("PlayerRecord"));
                ShowPause();
            }
        }
        
        else if (Input.GetKeyDown(KeyCode.Escape) && isOnPause)
        {
            ExitPause();
        }

        if (isGameOver)
        {
            Time.timeScale = 0f;

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            
            gameOver.SetActive(true);
            gameOverScore.text = gameScore.text;

            // Record system
            if (PlayerPrefs.GetInt("PlayerRecord", 0) == 0)
            {
                PlayerPrefs.SetInt("PlayerRecord", Int32.Parse(gameOverScore.text));
                gameOverRecord.text = gameOverScore.text;
            }
            else
            {
                // If beat the record
                if (PlayerPrefs.GetInt("PlayerRecord") < Int32.Parse(gameOverScore.text))
                {
                    gameOverRecord.text = gameOverScore.text;
                    PlayerPrefs.SetInt("PlayerRecord", Int32.Parse(gameOverScore.text));
                }
                // If not, just show the record
                else
                {
                    gameOverRecord.text = PlayerPrefs.GetInt("PlayerRecord").ToString();
                }
            }

            // Play Sound
            AudioManager.AudioManagerInstance.PlaySound(AudioManager.SoundKey.TimeOver);
        }

        //Debug.Log(freeLook.m_XAxis.m_MaxSpeed);
        //Debug.Log(freeLook.m_YAxis.m_MaxSpeed);
    }

    public void ShowPause()
    {
        Time.timeScale = 0f;
        pauseMenu.SetActive(true);
        
        isOnPause = true;

        // Unlocking Mouse
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ExitPause()
    {
        Time.timeScale = 1f;
        pauseMenu.SetActive(false);

        isOnPause = false;

        // Locking Mouse
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        ApplyConfig();
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void Restart()
    {
        SceneManager.LoadScene("Menu");
    }

    public void Default()
    {
        sliderX.value = 450f;
        sliderY.value = 4f;

        crosshairToggled.isOn = false;
    }

    public void ApplyConfig()
    {
        // Free look camera player sense
        freeLook.m_XAxis.m_MaxSpeed = sliderX.value;
        freeLook.m_YAxis.m_MaxSpeed = sliderY.value;

        // Relative sense to the player camera sense
        TowerController.mouseSensitivity = sliderX.value / 4.5f;

        // Player prefs
        PlayerPrefs.SetInt("CrosshairToggle", crosshairToggled.isOn == true ? 1 : 0);

        PlayerPrefs.SetFloat("MouseSenseX", sliderX.value);
        PlayerPrefs.SetFloat("MouseSenseY", sliderY.value);
    }

}
