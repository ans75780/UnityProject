using System;
using UnityEngine;

namespace Gameplay
{
    public class RespawnPoint : MonoBehaviour
    {
        private void Start()
        {
            GameManager.Instance.CurrentRespawnPoint = this;
        }
    }
}