using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class StateMachine : MonoBehaviour
{
    public enum States    
    {
        None,
    }

    public Dictionary<States, StateBase> dictionaryState;

    private StateBase _currentState;
    public float timeToStartGame;

    private void Awake()
    {
        dictionaryState = new Dictionary<States, StateBase>();
        dictionaryState.Add(States.None, new StateBase());

        SwitchState(States.None);

        Invoke(nameof(StartGame),timeToStartGame);
    }

    [Button]

    private void StartGame()
    {
        SwitchState(States.None); 
    }

    [Button]

    private void SwitchState(States state)
    {
        if(_currentState != null)
        {
            _currentState.OnStateExit();
        }
        _currentState = dictionaryState[state];

        _currentState.OnStateEnter();
    }

    private void Update()
    {
        if(_currentState != null)
        {
            _currentState.OnStateStay();
        }
        if(Input.GetKeyDown(KeyCode.Space))
        {
            //SwitchState(States.Dead);
        }
    }
}
