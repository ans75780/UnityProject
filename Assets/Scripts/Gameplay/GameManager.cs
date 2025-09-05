using System;
using System.Collections;
using System.Collections.Generic;
using Character.Player;
using Gameplay;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private Player player;

    public Player Player
    {
        get { return player; }
        set { player = value; }
    }
    
    
    private RespawnPoint currentRespawnPoint;

    public RespawnPoint CurrentRespawnPoint
    {
        get { return currentRespawnPoint; }
        set { currentRespawnPoint = value; }
    }
    
    private TextMeshProUGUI timerUI;
    
    [SerializeField]
    private Image fadeImage;
    
    [SerializeField]
    private float fadeOutDuration = 5f;
    
    [SerializeField]
    private float fadeInDuration = 1f;
    
    private static GameManager instance;

    public static GameManager Instance
    {
        get
        {
            return instance;
        }
        set
        {
            if (instance == null)
                instance = value;
        }
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Awake()
    {
        GameManager.Instance = this;
        fadeImage = GameObject.Find("FadeImage").GetComponent<Image>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        player.OnPlayerDeath += Receive_OnPlayerDeath;
    }
    
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Receive_OnPlayerDeath()
    {
       StartCoroutine(FadeCoroutine(fadeOutDuration, 0f, 1f, true));
    }

    private void Respawn()
    {
        Color color = fadeImage.color;
        color.a = 0f;
        fadeImage.color = color;
        StartCoroutine(FadeCoroutine(fadeInDuration, 1f, 0f, false));

        player.transform.position = currentRespawnPoint.transform.position;
        player.gameObject.SetActive(true);
        
    }
    
    IEnumerator FadeCoroutine(float duration, float _startAlpha, float _endAlpha, bool isRespawn = false)
    {
        float elapsed = 0f;
        Color color = fadeImage.color;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);

            // 알파값 보간
            color.a = Mathf.Lerp(_startAlpha, _endAlpha, t);
            fadeImage.color = color;
            yield return null; // 다음 프레임까지 대기
        }

        if (isRespawn)
        {
            player.gameObject.SetActive(false);
            Respawn();
        }
    }
}
