﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CheckTenderHunterAction : GoapAction
{
    private bool isChecked = false;
    private CenterEntity targetCenter;

    private float startTime = 0;
    public float checkDuration = 1f; // seconds

    public CheckTenderHunterAction()
    {
        addPrecondition("hasEnergy", true); 
        addPrecondition("hasTender", false); 
        addEffect("hasTender", true);
        addEffect("checkTender", true);
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
            Hunter hunter = (Hunter)agent.GetComponent(typeof(Hunter));
            Hunter tender = (Hunter)targetCenter.checkTender("Coop-Hunt");
            if(tender == null)
            {
                targetCenter.addTender("Coop-Hunt", hunter);
                hunter.hasTender = true;
            } else
            {
                Hunter targetHunter = tender;
                //This hunter
                hunter.hasTender = true;
                hunter.coopHunter = targetHunter;
                hunter.leader = false;
                //The other hunter
                targetHunter.coopHunter = hunter;
                targetHunter.leader = true;

                //Copy other's states
                hunter.actualPrey = targetHunter.actualPrey;

                Debug.Log("Coop Hunter found!");
                // Debug line
                Debug.DrawLine(targetHunter.transform.position, agent.transform.position, Color.red, 3, false);
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

