using System.Collections;
using Ables;
using UnityEngine;
using UnityEngine.UI;
using Character.Player;

namespace UI
{
    public class PlayerHUD : MonoBehaviour
    {
        private Slider hpBar;
        private Slider staminaBar;
        private Player player;


        private float targetRatio;
        private Coroutine corLerpHpBar;

        [Range(1, 5)] 
        public float lerpSpeed = 1f;
        
        void Awake()
        {
            if (hpBar == null)
            {
                hpBar = GameObject.Find("HPBar").GetComponent<Slider>();
            }
            if (staminaBar == null)
            {
               staminaBar = GameObject.Find("StaminaBar").GetComponent<Slider>();
            }
            
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        }
        
        void OnEnable()
        {
            player.GetComponent<Damageable>().OnChangedHpRatio += Receive_OnChangedHpRatio;
        }
        
        void OnDisable()
        {
            player.GetComponent<Damageable>().OnChangedHpRatio -= Receive_OnChangedHpRatio;
        }

        void Receive_OnChangedHpRatio(float hpRatio)
        {
            if (corLerpHpBar != null)
            {
                StopCoroutine(corLerpHpBar);
                corLerpHpBar = null;
            }
            corLerpHpBar = StartCoroutine(CorLerpHpBar(hpRatio));
        }

        IEnumerator CorLerpHpBar(float targetRatio)
        {
            while (Mathf.Abs(hpBar.value - targetRatio) > 0.001f)
            {
                hpBar.value = Mathf.Lerp(hpBar.value, targetRatio, lerpSpeed * Time.deltaTime);
                yield return null;
            }
            hpBar.value = targetRatio;

            // 마지막에 정확히 맞춰주기
            hpBar.value = targetRatio;
            corLerpHpBar = null;
        }
    }
}