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
            hpBar.value = hpRatio;
        }
        
    }
}