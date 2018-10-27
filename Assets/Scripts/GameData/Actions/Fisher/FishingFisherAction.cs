﻿using UnityEngine;

public class FishingFisherAction : GoapAction
{
    private bool fished = false;
    private GameObject targetLakePosition = null;

    private float startTime = 0;
    public float fishingDuration = 10; // seconds
    private int energyCost = 30;

    // find settings
    private float radius = 2f;
    private int numTry = 1;
    public FishingFisherAction()
    {
        addPrecondition("hasEnergy", true);
        addPrecondition("hasFood", false);
        addEffect("hasFood", true);
    }


    public override void reset()
    {
        fished = false;
        targetLakePosition = null;
        startTime = 0;
    }

    public override bool isDone()
    {
        return fished;
    }

    public override bool requiresInRange()
    {
        return true;
    }

    public override bool checkProceduralPrecondition(GameObject agent)
    {
        Fisher fisher = (Fisher)agent.GetComponent(typeof(Fisher));
        if (fisher.actualLakePosition != null)
        {
            targetLakePosition = fisher.actualLakePosition;
            if (targetLakePosition != null)
            {
                target = targetLakePosition.gameObject;
            }
        }
        else
        {
            float localRadius = numTry * radius;
            numTry++;
            Collider2D[] colliders = Physics2D.OverlapCircleAll(agent.transform.position, localRadius);
            LakeEntity lakeClose = null;
            if (colliders == null)
            {
                return false;
            }
            foreach (Collider2D hit in colliders)
            {
                if (hit.tag != "Lake")
                {
                    continue;
                }

                LakeEntity lake = (LakeEntity)hit.gameObject.GetComponent(typeof(LakeEntity));
                if (lake.full)
                {
                    // TODO transformar en granjero
                    continue;
                }

                lakeClose = lake;
                break;
            }

            bool isClosest = lakeClose != null;
            if (isClosest)
            {               
                targetLakePosition = lakeClose.addFisher();
                fisher.actualLakePosition = targetLakePosition;
                target = targetLakePosition;
                numTry = 1;
            }
        }

        return targetLakePosition != null;
    }

    public override bool perform(GameObject agent)
    {
        if (startTime == 0)
        {
            startTime = Time.time;
        }

        if (Time.time - startTime > fishingDuration)
        {
            Fisher fisher = (Fisher)agent.GetComponent(typeof(Fisher));
            fisher.food =  Random.Range(1, 20);
            fisher.energy -= energyCost;
            fished = true;
        }
        return true;
    }

}


