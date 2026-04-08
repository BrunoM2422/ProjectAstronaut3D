using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateBase
{
    public virtual void OnStateEnter(object o = null)
    {
        Debug.Log("StateBase: OnStateEnter");
    }

    public virtual void OnStateStay(object o = null)
    {
        Debug.Log("StateBase: OnStateStay");
    }

    public virtual void OnStateExit(object o = null)
    {
        Debug.Log("StateBase: OnStateExit");
    }
}
