using UnityEngine;

public class CoopSurroundTargetHunterAction : GoapAction
{
    private bool positioned = false;
    public Vector3 nextPosition;

    public float angleRotation = 0;
    public float firstAngle = 0;
    public bool positiveRotation = true;
    public Vector3 tentativePos;
    public CoopSurroundTargetHunterAction()
    {
        addPrecondition("hasEnergy", true);
        addPrecondition("hasActualPrey", true);
        addPrecondition("hasCoopHunter", true);
        addPrecondition("hasDeadPrey", false);
        addPrecondition("hasFood", false);
        addPrecondition("isInPosition", false);
        addEffect("isInPosition", true);
    }


    public override void reset()
    {
        positioned = false;
        nextPosition = Vector3.zero;
        tentativePos = Vector3.zero;
        positiveRotation = true;
        angleRotation = 0;
        firstAngle = 0;
    }

    public override bool isDone()
    {
        return positioned;
    }

    public override bool requiresInRange()
    {
        return true;
    }

    public override bool checkProceduralPrecondition(GameObject agent)
    {

        Hunter hunter = (Hunter)agent.GetComponent(typeof(Hunter));
        if (hunter.actualPrey != null)
        {
            float posX = hunter.actualPrey.transform.position.x - 2f;
            if (hunter.actualPrey.transform.position.x < agent.transform.position.x)
            {
                posX = hunter.actualPrey.transform.position.x + 2f;
            }

            float posY = hunter.actualPrey.transform.position.y - 2f;
            if (hunter.actualPrey.transform.position.y < agent.transform.position.y)
            {
                posY = hunter.actualPrey.transform.position.y + 2f;
            }

            nextPosition = new Vector3(posX, posY, agent.transform.position.z);
            targetPosition = nextPosition;
            Debug.DrawLine(targetPosition, agent.transform.position, Color.black, 3, false);
        }

        return hunter.actualPrey != null;
    }

    public override bool perform(GameObject agent)
    {
        enableBubbleIcon(agent);
        Hunter hunter = (Hunter)agent.GetComponent(typeof(Hunter));
        float step = 1 * Time.deltaTime;
        float posX = hunter.actualPrey.transform.position.x - 2f;
        if (hunter.actualPrey.transform.position.x < agent.transform.position.x)
        {
            posX = hunter.actualPrey.transform.position.x + 2f;
        }

        float posY = hunter.actualPrey.transform.position.y - 2f;
        if (hunter.actualPrey.transform.position.y < agent.transform.position.y)
        {
            posY = hunter.actualPrey.transform.position.y + 2f;
        }

        Vector3 actualTarget = new Vector3(posX, posY, agent.transform.position.z);
       
        
        // Leader wait other
        if (hunter.leader)
        {
            hunter.transform.position = Vector3.MoveTowards(hunter.transform.position, actualTarget, step);
            Debug.DrawLine(hunter.actualPrey.transform.position, agent.transform.position, Color.red);
            hunter.isInPosition = true;
            if (hunter.coopHunter.isInPosition)
            {
                disableBubbleIcon(agent);
                positioned = true;
                return true;
            }
        }
        else
        {
            if (!tentativePos.Equals(Vector3.zero))
            {
                hunter.transform.position = Vector3.MoveTowards(hunter.transform.position, tentativePos, step);
                float dist = Vector3.Distance(hunter.transform.position, tentativePos);
                if(dist <= 0.3f)
                {
                    tentativePos = Vector3.zero;
                }
            } else
            {
                
                if (Mathf.Abs(angleRotation - firstAngle) > 3f)
                {
                    disableBubbleIcon(agent);
                    hunter.isInPosition = true;
                    positioned = true;
                    return true;
                }
                // Move near
                hunter.transform.position = Vector3.MoveTowards(hunter.transform.position, actualTarget, step);
                if (firstAngle == 0)
                {
                    Vector3 dir = hunter.actualPrey.transform.position - nextPosition;
                    dir = hunter.actualPrey.transform.InverseTransformDirection(dir);
                    float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                    float converseAngle = Mathf.Abs(angle % 6.301f);
                    if(angle > 0)
                    {
                        disableBubbleIcon(agent);
                        positiveRotation = false;
                        //converseAngle *= -1;
                    }
                    angleRotation = converseAngle;
                    firstAngle = angleRotation;
                }
                //Rotación
                Vector2 hunter2D = new Vector2(hunter.transform.position.x, hunter.transform.position.y);
                Vector2 prey2D = new Vector2(hunter.actualPrey.transform.position.x, hunter.actualPrey.transform.position.y);
                Vector2 heading = hunter2D - prey2D;
                float radius = Mathf.Abs(heading.magnitude);

                Vector2 offset = new Vector2(Mathf.Sin(angleRotation), Mathf.Cos(angleRotation)) * radius;
                tentativePos = hunter.actualPrey.transform.position + new Vector3(offset.x, offset.y, 0);
                if (positiveRotation) {
                    angleRotation += 1f * Time.deltaTime;
                } else
                {
                    angleRotation -= 1f * Time.deltaTime;
                }
            }
            // coop check position
            /*
            if (Mathf.Abs(firstAngle - angleRotation) > 3f)
            {
                hunter.isInPosition = true;
                positioned = true;
                return true;
            } else
            {*/
            // Set angle rotation
            
            //}
            Debug.DrawLine(hunter.actualPrey.transform.position, agent.transform.position, Color.magenta);
        }
        
        return true;
    }

}



