﻿
using UnityEngine;
using System.Collections.Generic;

public abstract class GoapAction : MonoBehaviour
{
    private Dictionary<string, object> preconditions;
    private Dictionary<string, object> effects;    

    private bool inRange = false;

    // Sprite bubble
    public Sprite bubbleSprite;
    // Coste de la acción
    public float cost = 1f;

    // Objetivo, si aplica (puede ser null)
    public GameObject target;

    public Vector3 targetPosition;

    public GoapAction()
    {
        preconditions = new Dictionary<string, object>();
        effects = new Dictionary<string, object>();
    }

    public void doReset()
    {
        inRange = false;
        target = null;
        targetPosition = Vector3.zero;
        reset();
    }

    /**
	 * Reset any variables that need to be reset before planning happens again.
	 */
    public abstract void reset();

    /**
	 * Is the action done?
	 */
    public abstract bool isDone();

    /**
	 * Procedurally check if this action can run. Not all actions
	 * will need this, but some might.
	 */
    public abstract bool checkProceduralPrecondition(GameObject agent);

    /**
	 * Run the action.
	 * Returns True if the action performed successfully or false
	 * if something happened and it can no longer perform. In this case
	 * the action queue should clear out and the goal cannot be reached.
	 */
    public abstract bool perform(GameObject agent);

    /**
	 * Does this action need to be within range of a target game object?
	 * If not then the moveTo state will not need to run for this action.
	 */
    public abstract bool requiresInRange();


    /**
	 * Are we in range of the target?
	 * The MoveTo state will set this and it gets reset each time this action is performed.
	 */
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

    public void enableBubbleIcon(GameObject _agent)
    {
        Transform bubble = _agent.transform.GetChild(0);
        GameObject icon = bubble.GetChild(0).gameObject;
        SpriteRenderer iconSr = icon.GetComponent<SpriteRenderer>();
        iconSr.sprite = bubbleSprite;
        bubble.gameObject.SetActive(true);
    }

    public void disableBubbleIcon(GameObject _agent)
    {
        GameObject bubble = _agent.transform.GetChild(0).gameObject;
        bubble.SetActive(false);
    }
}