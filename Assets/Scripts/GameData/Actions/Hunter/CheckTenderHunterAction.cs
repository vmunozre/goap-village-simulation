using UnityEngine;
public class CheckTenderHunterAction : GoapAction
{
    private bool isChecked = false;
    private CenterEntity targetCenter;

    private float startTime = 0;

    // Check tenders to coop hunting
    public CheckTenderHunterAction()
    {
        setActionName("Check hunter tender");
        setBaseDuration(1f);
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
        Agent abstractAgent = (Agent)agent.GetComponent(typeof(Agent));
        targetCenter = abstractAgent.center;
        target = targetCenter.gameObject;
        return targetCenter != null;
    }

    public override bool perform(GameObject agent)
    {
        if (startTime == 0)
        {
            enableBubbleIcon(agent);
            Hunter hunter = (Hunter)agent.GetComponent(typeof(Hunter));

            TenderRequest tender = targetCenter.checkTenderList();
            if(tender == null)
            {
                Debug.Log("AAAHH no tender");
                TenderRequest newTender = targetCenter.addTenderList(hunter);
                hunter.tenderRequest = newTender;
            } else
            {
                Hunter targetHunter = tender.hunter;
                // This hunter
                hunter.tenderRequest = tender;
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

        if (Time.time - startTime > duration)
        {
            disableBubbleIcon(agent);
            isChecked = true;
        }
        return true;
    }

}


