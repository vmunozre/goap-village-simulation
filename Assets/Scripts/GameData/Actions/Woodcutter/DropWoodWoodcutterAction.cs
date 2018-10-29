using UnityEngine;

public class DropWoodWoodcutterAction : GoapAction
{
    private bool droppedWood = false;

    private float startTime = 0;
    public float dropDuration = 1.5f; // seconds

    public DropWoodWoodcutterAction()
    {
        addPrecondition("hasWood", true);
        addEffect("hasWood", false);
        addEffect("collectWood", true);
    }

    public override void reset()
    {
        droppedWood = false;
        startTime = 0;
    }

    public override bool isDone()
    {
        return droppedWood;
    }

    public override bool requiresInRange()
    {
        return true;
    }

    public override bool checkProceduralPrecondition(GameObject agent)
    {
        Woodcutter woodcutter = (Woodcutter)agent.GetComponent(typeof(Woodcutter));

        if (woodcutter.sawmill != null)
        {
            target = woodcutter.sawmill.gameObject;
        } else
        {
            target = woodcutter.warehouse.gameObject;
        }
        // Debug line
        // Debug.DrawLine(target.transform.position, agent.transform.position, Color.yellow, 3, false);
        return target != null;
    }

    public override bool perform(GameObject agent)
    {
        if (startTime == 0)
        {
            enableBubbleIcon(agent);
            startTime = Time.time;
        }

        if (Time.time - startTime > dropDuration)
        {
            disableBubbleIcon(agent);
            Woodcutter woodcutter = (Woodcutter)agent.GetComponent(typeof(Woodcutter));
            if (woodcutter.sawmill != null)
            {
                woodcutter.sawmill.wood += woodcutter.wood;
            }
            else
            {
                woodcutter.warehouse.wood += woodcutter.wood;
                SawmillBuilding[] sawmills = (SawmillBuilding[])FindObjectsOfType(typeof(SawmillBuilding));
                foreach (SawmillBuilding saw in sawmills)
                {
                    if (!saw.blueprint.done)
                    {
                        continue;
                    }
                    woodcutter.sawmill = saw;
                    woodcutter.sawmill.workers++;
                    break;

                }
                if (woodcutter.sawmill == null)
                {
                    Building building = new Building("Prefabs/Buildings/Sawmill", 200, 150, 5, 2);
                    woodcutter.center.addNewBuildingRequest(building);
                }
            }

            woodcutter.wood = 0;
            droppedWood = true;
        }
        return true;
    }
}

