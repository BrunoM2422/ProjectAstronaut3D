using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerScript;

public class PowerUPSuperJump : PowerUPBase
{
    protected override void StartPowerUp()
    {
        PlayerScript.Instance.ApplyPowerUp(PowerUpType.SuperJump, duration);
    }
}
