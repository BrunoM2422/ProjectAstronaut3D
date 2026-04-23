using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerScript;

public class PowerUPInfiniteBullets : PowerUPBase
{
    protected override void StartPowerUp()
    {
        GameManager.Instance.currentPlayer.GetComponent<PlayerScript>().ApplyPowerUp(PowerUpType.InfiniteBullets, duration);
    }
}