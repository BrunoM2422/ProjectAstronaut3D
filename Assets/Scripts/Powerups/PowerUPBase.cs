using UnityEngine;
using DG.Tweening;

public class PowerUPBase : ItemCollectableBase
{
    [Header("Power Up")]
    public float duration;


    protected override void OnCollect()
    {
        Transform player = GameManager.Instance.currentPlayer.GetComponent<PlayerScript>()?.transform;

        if (player == null)
        {
            Debug.LogError("Player não encontrado no momento da coleta!");
            return;
        }

        player.DOScale(1.2f, .2f)
              .SetEase(Ease.OutBack)
              .SetLoops(2, LoopType.Yoyo);

        StartPowerUp();

        base.OnCollect(); 
    }
    protected virtual void StartPowerUp()
    {
        Invoke(nameof(EndPowerUp), duration);


    }

    protected virtual void EndPowerUp()
    {


    }
}
