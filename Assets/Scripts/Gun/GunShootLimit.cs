using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GunShootLimit : GunBase
{

    public List<GunUIUpdater> GunUIUpdater;
    public float maxShots = 10;
    public float reloadTime = 2f;

    private bool _recharging = false;

    private float _currentShots;

    private void Awake()
    {
        GetAllUIs();
    }
    

    protected override IEnumerator StartShoot()
    {
        if(_recharging)
        {
            yield break;
        }
        while (true)
        {
            if(_currentShots < maxShots)
            {
                Shoot();
                _currentShots++;
                CheckRecharge();
                UpdateUI();
                yield return new WaitForSeconds(timeBetweenShots);
            }
        }
    }

    private void CheckRecharge()
    {
        if(_currentShots >= maxShots)
        {
            StopShooting();
            StartRecharge();
        }
    }

    private void StartRecharge()
    {
        _recharging = true;
        StartCoroutine(RechargeCoroutine());
    }

    IEnumerator RechargeCoroutine()
    {
        float time = 0;

        while(time <reloadTime)
        {   time += Time.deltaTime;
            GunUIUpdater.ForEach(i => i.UpdateValue(time/reloadTime));
            yield return new WaitForEndOfFrame();
        }
        _currentShots = 0;
        _recharging = false;
    }

    public void UpdateUI()
    {
        GunUIUpdater.ForEach(i => i.UpdateValue(maxShots, _currentShots));
    }


    private void GetAllUIs()
    {
            GunUIUpdater = GameObject.FindObjectsOfType<GunUIUpdater>().ToList();
    }

}
