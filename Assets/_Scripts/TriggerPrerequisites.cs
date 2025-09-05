using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerPrerequisites : MonoBehaviour
{
    public int numTriggers;
    public TriggerUnityEvent eventTriggerScript;

    private int numTriggersActivated = 0;

    private void Awake()
    {
        if (!eventTriggerScript)
        {
            Debug.Log("No event trigger script found: self-destructing.");
            Destroy(this);
        }
    }

    public void PrereqTriggerActivated(){
        numTriggersActivated++;
        if (numTriggersActivated >= numTriggers)
        {
            EnableUnityEventTriggerer();
        }
    }
    
    public void PrereqTriggerDeactivated(){
        numTriggersActivated--;
        if (numTriggersActivated < numTriggers){
            DisableUnityEventTriggerer();
        }
    }

    private void EnableUnityEventTriggerer(){
        eventTriggerScript.canActivate = true;
    }
    
    private void DisableUnityEventTriggerer(){
        eventTriggerScript.canActivate = false;
    }
}
