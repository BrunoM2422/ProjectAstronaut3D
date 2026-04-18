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

    void Update()
    {
        if (!_recharging)
        {
            UpdateUI();
        }
    }

    protected override IEnumerator StartShoot()
    {
        if (_recharging)
        {
            yield break;
        }

        while (true)
        {
            if (PlayerScript.Instance.IsInfiniteBulletsActive() || _currentShots < maxShots)
            {
                Shoot();

                if (!PlayerScript.Instance.IsInfiniteBulletsActive())
                {
                    _currentShots++;
                    CheckRecharge();
                }

 
                yield return new WaitForSeconds(timeBetweenShots);
            }
        }
    }

    private void CheckRecharge()
    {
        if (PlayerScript.Instance.IsInfiniteBulletsActive())
            return;

        if (_currentShots >= maxShots)
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
        if (_recharging)
            return; 

        if (PlayerScript.Instance.IsInfiniteBulletsActive())
        {
            GunUIUpdater.ForEach(i => i.ShowInfinite());
            return;
        }

        GunUIUpdater.ForEach(i =>
        {
            i.ShowNormal();
            i.UpdateValue(maxShots, _currentShots);
        });
    }


    private void GetAllUIs()
    {
            GunUIUpdater = GameObject.FindObjectsOfType<GunUIUpdater>().ToList();
    }




}
