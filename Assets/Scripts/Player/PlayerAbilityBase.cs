using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAbilityBase : MonoBehaviour
{
    protected PlayerScript player;

    protected Inputs inputs;

    private void OnValidate()
    {
        if(player == null)
        {
            player = GetComponentInParent<PlayerScript>();
        }
    }

    private void Start()
    {

        inputs = new Inputs();
        inputs.Enable();
        Init();
        OnValidate();
        RegisterListeners();
    }

    private void OnEnable()
    {
        if(inputs != null)
        {
            inputs.Enable();
        }
    }

    private void OnDisable()
    {
        if (inputs != null)
        {
            inputs.Disable();
        }
    }

    protected virtual void Init()
    {
        RemoveveListeners();
    }

    protected virtual void RegisterListeners()
    {

    }

    protected virtual void RemoveveListeners()
    {
    }
}
