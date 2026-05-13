using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Itens;

public class SaveManager : Singleton<SaveManager>
{
    [SerializeField] private SaveSetup _saveSetup;
    private string path;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
        path = Application.persistentDataPath + "/save.txt";
        _saveSetup = new SaveSetup();
    }

    public void SavePlayerState(Vector3 checkpointPos)
    {
        _saveSetup.checkpointPosition = checkpointPos;
        _saveSetup.hasActiveCheckpoint = true;
        SaveItemsAndHealth();
        Save();
    }

    public void ResetCheckpoint()
    {
        _saveSetup.hasActiveCheckpoint = false;
        Save();
    }

    public void SaveLastLevel(int level)
    {
        _saveSetup.lastLevel = level;
        _saveSetup.hasActiveCheckpoint = false;
        SaveItemsAndHealth();
        Save();
    }

    public void SaveItemsAndHealth()
    {
        _saveSetup.coins = ItemManager.Instance.GetItemByType(ItemType.Coin).soInt.value;
        _saveSetup.lifePacks = ItemManager.Instance.GetItemByType(ItemType.LifePack).soInt.value;
        _saveSetup.globalLifes = GameManager.Instance.lifes;

        if (GameManager.Instance.currentPlayer != null)
        {
            var player = GameManager.Instance.currentPlayer.GetComponent<PlayerScript>();
            _saveSetup.health = player.GetCurrentHealth();
        }
    }

    public void ClearSave()
    {
        if (File.Exists(path))
        {
            File.Delete(path);
        }
        _saveSetup = new SaveSetup();
        Debug.Log("Save deletado - O jogador perdeu tudo.");
    }

    [NaughtyAttributes.Button("Save")]
    public void Save()
    {
        string setupToJson = JsonUtility.ToJson(_saveSetup, true);
        File.WriteAllText(path, setupToJson);
    }

    [NaughtyAttributes.Button("Load")]
    public SaveSetup Load()
    {
        if (File.Exists(path))
        {
            string fileLoaded = File.ReadAllText(path);
            _saveSetup = JsonUtility.FromJson<SaveSetup>(fileLoaded);

            ItemManager.Instance.GetItemByType(ItemType.Coin).soInt.value = _saveSetup.coins;
            ItemManager.Instance.GetItemByType(ItemType.LifePack).soInt.value = _saveSetup.lifePacks;

            return _saveSetup;
        }
        return null;
    }
}

[System.Serializable]
public class SaveSetup
{
    public int lastLevel = 0;
    public int coins = 0;
    public int lifePacks = 0;
    public int globalLifes = 3;
    public float health = 20f;
    public Vector3 checkpointPosition;
    public bool hasActiveCheckpoint = false;
}