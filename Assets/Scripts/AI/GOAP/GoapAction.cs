using UnityEngine;
using System.Collections.Generic;

public abstract class GoapAction : MonoBehaviour
{
    private Dictionary<string, object> preconditions;
    private Dictionary<string, object> effects;    

    private bool inRange = false;

    // UI items
    public string actionName = "Default action";
    public Sprite uiImage;

    // Bubble sprite
    public Sprite bubbleSprite;
    // action cost
    public float cost = 1f;

    // targets
    public GameObject target;
    public Vector3 targetPosition;

    public float baseDuration;
    public float duration;

    public GoapAction()
    {
        preconditions = new Dictionary<string, object>();
        effects = new Dictionary<string, object>();
    }

    void Update()
    {
        duration = baseDuration / GameManager.instance.actualMuti;
    }

    protected void setActionName(string _actionName)
    {
        this.actionName = _actionName;
    }
    
    public void changeDefaultCost(float _cost)
    {
        cost = _cost;
    }

    public void setBaseDuration(float _duration)
    {
        baseDuration = _duration;
    }

    public void doReset()
    {
        inRange = false;
        target = null;
        targetPosition = Vector3.zero;
        reset();
    }

    // Reset variables
    public abstract void reset();

    // Check if is done the action
    public abstract bool isDone();

    // Check if this action can run
    public abstract bool checkProceduralPrecondition(GameObject agent);

    // Run the action.
    public abstract bool perform(GameObject agent);

    // Check if the agent need go to the target
    public abstract bool requiresInRange();

    // Check if is in range
    public bool isInRange()
    {
        return inRange;
    }

    public void setInRange(bool inRange)
    {
        this.inRange = inRange;
    }

    public void addPrecondition(string key, object value)
    {
        preconditions.Add(key, value);
    }

    public void removePrecondition(string key)
    {
        if (preconditions.ContainsKey(key))
        {
            preconditions.Remove(key);        
        }
    }

    public void addEffect(string key, object value)
    {
        effects.Add(key, value);
    }

    public void removeEffect(string key)
    {
        if (effects.ContainsKey(key))
        {
            effects.Remove(key);
        }
    }

    public Dictionary<string, object> Preconditions
    {
        get
        {
            return preconditions;
        }
    }

    public Dictionary<string, object> Effects
    {
        get
        {
            return effects;
        }
    }

    // Turn active the bubble and visible the icon
    public void enableBubbleIcon(GameObject _agent)
    {
        Transform bubble = _agent.transform.GetChild(0);
        GameObject icon = bubble.GetChild(0).gameObject;
        SpriteRenderer iconSr = icon.GetComponent<SpriteRenderer>();
        iconSr.sprite = bubbleSprite;
        bubble.gameObject.SetActive(true);
    }

    // Turn disble the bubble
    public void disableBubbleIcon(GameObject _agent)
    {
        GameObject bubble = _agent.transform.GetChild(0).gameObject;
        bubble.SetActive(false);
    }

    // Check if the _state have preconditions or effects presents in this action
    public int checkCoincidencies(Dictionary<string,object> _state)
    {
        int count = 0;
        foreach(string key in _state.Keys)
        {
            if (preconditions.ContainsKey(key) || effects.ContainsKey(key))
                count++;
        }
        return count;
    }
}