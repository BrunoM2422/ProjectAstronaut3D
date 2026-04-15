using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAblityShoot : PlayerAbilityBase

{

    public List<GunBase> guns;

    public Transform gunPosition;

    private GunBase _currentGun;

    private int currentGunIndex = 0;

    protected override void Init()
    {
        base.Init();

        CreateGun();

        inputs.Gameplay.Shoot.performed += cts => StartShooting();
        inputs.Gameplay.Shoot.canceled += cts => CancelShooting();


    }

    private void Update()
    {
        if (Keyboard.current.digit1Key.wasPressedThisFrame)
        {

            ChangeGun(0);
        }

        if (Keyboard.current.digit2Key.wasPressedThisFrame)
        {

            ChangeGun(1);
        }
    }

    private void CreateGun()
    {
        if (_currentGun != null)
        {
            Destroy(_currentGun.gameObject);
        }

        _currentGun = Instantiate(guns[currentGunIndex], gunPosition);

        _currentGun.transform.localPosition = Vector3.zero;
        _currentGun.transform.localEulerAngles = Vector3.zero;
    }

    private void StartShooting()
    {
         _currentGun.StartShooting();
    }

    private void CancelShooting()
    {
        _currentGun.StopShooting();
    }

    private void ChangeGun(int index)
    {
        if (index >= guns.Count) return;

        currentGunIndex = index;
        CreateGun();

        UpdateCurrentGunUI();
    }

    private void UpdateCurrentGunUI()
    {
        if (_currentGun is GunShootLimit gunWithLimit)
        {
            gunWithLimit.UpdateUI();
        }
    }
}
