using System.Collections.Generic;

// Interface for agents
public interface IGoap
{
    // Get world state form agent
    Dictionary<string, object> getWorldState();

    // create goal state
    Dictionary<string, object> createGoalState();

    void planFailed(Dictionary<string, object> failedGoal);

    void planFound(Dictionary<string, object> goal, Queue<GoapAction> actions);

    void planAborted(GoapAction aborter);

    // All actions done
    void actionsFinished();

    //  Move the agent towards the target
    bool moveAgent(GoapAction nextAction);

}

