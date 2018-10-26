﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CheckRequestsBuilderAction : GoapAction
{
    private bool isChecked = false;
    private CenterEntity targetCenter;

    private float startTime = 0;
    public float checkDuration = 0.5f; // seconds

    public CheckRequestsBuilderAction()
    {
        addPrecondition("hasEnergy", true);
        addPrecondition("hasActualRequest", false);
        addPrecondition("hasActualBuilding", false);
        addEffect("hasActualRequest", true);
    }


    public override void reset()
    {
        isChecked = false;
        targetCenter = null;
        startTime = 0;
    }

    public override bool isDone()
    {
        return isChecked;
    }

    public override bool requiresInRange()
    {
        return false;
    }

    public override bool checkProceduralPrecondition(GameObject agent)
    {
        CenterEntity[] centers = (CenterEntity[])FindObjectsOfType(typeof(CenterEntity));
        CenterEntity closest = null;
        if (centers == null)
        {
            return false;
        }
        if (centers.Length > 0)
        {
            closest = centers[0];
        }

        if (closest == null)
            return false;

        targetCenter = closest;
        target = targetCenter.gameObject;
        return closest != null;
    }

    public override bool perform(GameObject agent)
    {

        if (startTime == 0)
        {
            Builder builder = (Builder)agent.GetComponent(typeof(Builder));

            Building building = targetCenter.getBuildingRequest();
            
            if (building == null)
            {
                builder.waiting = true;
                return false;
            } else
            {
                Debug.Log("Building added to builder");
                builder.actualRequest = building;
            }
            startTime = Time.time;
        }

        if (Time.time - startTime > checkDuration)
        {            
            isChecked = true;
        }
        return true;
    }

}


