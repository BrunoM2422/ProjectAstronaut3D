using Assets.Scripts;
using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;
public enum GameStates
{
    INTRO,
    GAMEPLAY,
    PAUSE,
    WIN,
    LOSE
}

public class GameManager : Singleton<GameManager>
{

    public CinemachineFreeLook freeLook;

    public StateMachine<GameStates> stateMachine;

    public GameObject playerPrefab;
    public Transform spawnPoint;

    public TextMeshProUGUI lifeText;

    public Transform currentPlayer;
    public PlayerHealthUpdater healthUI;
    public TextMeshProUGUI checkpointText;

    public int lifes = 3;

    private void Start()
    {
        Init();

        Vector3 startPos = spawnPoint != null ? spawnPoint.position : transform.position;
        Quaternion startRot = spawnPoint != null ? spawnPoint.rotation : transform.rotation;

        Spawn(startPos, startRot);

        UpdateLivesUI();
    }

    public void Init()
    {
        stateMachine = new StateMachine<GameStates>();
        stateMachine.Init();
        stateMachine.RegisterStates(GameStates.INTRO, new GMStateIntro());
        stateMachine.RegisterStates(GameStates.GAMEPLAY, new StateBase());
        stateMachine.RegisterStates(GameStates.PAUSE, new StateBase());
        stateMachine.RegisterStates(GameStates.WIN, new StateBase());
        stateMachine.RegisterStates(GameStates.LOSE, new StateBase());

        stateMachine.SwitchState(GameStates.INTRO);
    }

    


    public Transform currentCheckpoint;



    public void SetCheckpoint(Transform checkpoint)
    {
        currentCheckpoint = checkpoint;
    }

    public void RespawnPlayer()
    {
        if (lifes <= 0)
        {
            stateMachine.SwitchState(GameStates.LOSE);
            return;
        }

        lifes--;
        UpdateLivesUI();

        if (currentCheckpoint == null)
        {
            
            Spawn(spawnPoint.position, spawnPoint.rotation);
            return;
        }

        Vector3 spawnOffset = currentCheckpoint.forward * 2f + Vector3.up * 1.5f;
        Vector3 spawnPosition = currentCheckpoint.position + spawnOffset;

        Spawn(spawnPosition, currentCheckpoint.rotation);
    }

    void UpdateLivesUI()
    {
        if (lifeText != null)
        {
            lifeText.text =lifes.ToString();
        }
    }



    public void Spawn(Vector3 position, Quaternion rotation)
    {
        GameObject player = Instantiate(playerPrefab, position, rotation);
        currentPlayer = player.transform;
        PlayerScript ps = player.GetComponent<PlayerScript>();
        ps.healthUI = healthUI;


        freeLook.Follow = player.transform;
        freeLook.LookAt = player.transform;
    }

    public float textDisplayDuration = 2f;

    public void ShowCheckpointText()
    {
        StopAllCoroutines();
        StartCoroutine(CheckpointTextRoutine());
    }

    IEnumerator CheckpointTextRoutine()
    {
        checkpointText.gameObject.SetActive(true);
        checkpointText.text = "NOVO CHECKPOINT ALCANÇADO!";

        checkpointText.transform.localScale = Vector3.zero;
        checkpointText.transform.DOScale(1f, 0.5f).SetEase(Ease.OutBack);
        checkpointText.transform.DOScale(0f, 0.3f).SetEase(Ease.InBack);

        yield return new WaitForSeconds(textDisplayDuration);

        checkpointText.gameObject.SetActive(false);
    }

}
