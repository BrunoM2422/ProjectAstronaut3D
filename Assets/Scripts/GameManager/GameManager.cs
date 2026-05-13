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

    [Header("UI Reference")]
    public TextMeshProUGUI lifeText;
    public GameObject loseScreen; 

    public Transform currentPlayer;
    public PlayerHealthUpdater healthUI;
    public TextMeshProUGUI checkpointText;

    public int lifes = 3;

    private void Start()
    {
        Init();
        SaveSetup savedData = SaveManager.Instance.Load();

        Vector3 startPos;
        Quaternion startRot;

        if (savedData != null)
        {
            lifes = savedData.globalLifes;

            if (savedData.hasActiveCheckpoint)
            {
                startPos = savedData.checkpointPosition;
                startRot = Quaternion.identity;
            }
            else
            {
                startPos = spawnPoint != null ? spawnPoint.position : transform.position;
                startRot = spawnPoint != null ? spawnPoint.rotation : transform.rotation;
            }
        }
        else
        {
            startPos = spawnPoint != null ? spawnPoint.position : transform.position;
            startRot = spawnPoint != null ? spawnPoint.rotation : transform.rotation;
        }

        Spawn(startPos, startRot);

        if (savedData != null && savedData.hasActiveCheckpoint)
        {
            var ps = currentPlayer.GetComponent<PlayerScript>();
            if (ps != null) ps.SetCurrentHealth(savedData.health);
        }

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
        lifes--;

        SaveManager.Instance.SaveItemsAndHealth();
        SaveManager.Instance.Save();

        UpdateLivesUI();

        if (lifes <= 0)
        {
            SaveManager.Instance.ClearSave();
            stateMachine.SwitchState(GameStates.LOSE);

            if (loseScreen != null) loseScreen.SetActive(true); 

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            return;
        }

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
            lifeText.text = lifes.ToString();
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

        yield return new WaitForSeconds(textDisplayDuration);

        checkpointText.transform.DOScale(0f, 0.3f).SetEase(Ease.InBack).OnComplete(() => {
            checkpointText.gameObject.SetActive(false);
        });
    }
}