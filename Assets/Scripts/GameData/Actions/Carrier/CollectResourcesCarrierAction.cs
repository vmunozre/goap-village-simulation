using UnityEngine;
public class CollectResourcesCarrierAction : GoapAction
{
    private bool collected = false;

    private float startTime = 0;
    public float checkDuration = 2.5f; // seconds

    public int agentCapacity = 30;
    private int energyCost = 10;

    // Collect resources 
    public CollectResourcesCarrierAction()
    {
        addPrecondition("hasEnergy", true);
        addPrecondition("hasResources", false);
        addEffect("hasResources", true);
    }


    public override void reset()
    {
        collected = false;
        startTime = 0;
    }

    public override bool isDone()
    {
        return collected;
    }

    public override bool requiresInRange()
    {
        return true;
    }

    public override bool checkProceduralPrecondition(GameObject agent)
    {
        Carrier carrier = (Carrier)agent.GetComponent(typeof(Carrier));

        if(carrier.sawmill != null)
        {
            target = carrier.sawmill.gameObject;
        } else
        {
            if(carrier.huntingShed != null)
            {
                target = carrier.huntingShed.gameObject;
            } else
            {
                SawmillBuilding[] sawmills = (SawmillBuilding[])FindObjectsOfType(typeof(SawmillBuilding));
                foreach (SawmillBuilding saw in sawmills)
                {
                    if (!saw.blueprint.done || saw.carriers >= saw.limitCarriers)
                    {
                        continue;
                    }
                    carrier.sawmill = saw;
                    carrier.sawmill.carriers++;
                    target = carrier.sawmill.gameObject;
                    break;
                }
                if (carrier.sawmill == null)
                {
                    HuntingShedBuilding[] huntingSheds = (HuntingShedBuilding[])FindObjectsOfType(typeof(HuntingShedBuilding));
                    foreach (HuntingShedBuilding shed in huntingSheds)
                    {
                        if (!shed.blueprint.done || shed.carriers >= shed.limitCarriers)
                        {
                            continue;
                        }
                        carrier.huntingShed = shed;
                        carrier.huntingShed.carriers++;
                        target = carrier.huntingShed.gameObject;
                        break;
                    }
                    if (carrier.huntingShed == null)
                    {
                        carrier.waiting = true;
                    }
                }
            }
        }
        return target != null;
    }

    public override bool perform(GameObject agent)
    {
        if (startTime == 0)
        {
            enableBubbleIcon(agent);
            Carrier carrier = (Carrier)agent.GetComponent(typeof(Carrier));

            if (carrier.sawmill != null)
            {
                if(carrier.sawmill.wood <= 0)
                {
                    disableBubbleIcon(agent);
                    carrier.waiting = true;
                    return false;
                }

                int wood = Mathf.Min(carrier.sawmill.wood, agentCapacity);
                carrier.wood += wood;
                carrier.sawmill.wood -= wood;
            }
            else
            {
                if (carrier.huntingShed.food <= 0)
                {
                    disableBubbleIcon(agent);
                    carrier.waiting = true;
                    return false;
                }

                int food = Mathf.Min(carrier.huntingShed.food, agentCapacity);
                carrier.food += food;
                carrier.huntingShed.food -= food;
            }

            startTime = Time.time;
        }

        if (Time.time - startTime > checkDuration)
        {
            disableBubbleIcon(agent);
            Carrier carrier = (Carrier)agent.GetComponent(typeof(Carrier));
            carrier.energy -= energyCost;
            collected = true;
        }
        return true;
    }

}


