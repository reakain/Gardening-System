using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant
{
    public int season { get; private set; }
    public int numStages { get; private set; }

    public PlantStage[] stages { get; private set; }

    public PlantStage currentStage { get { return stages[stageIndex]; } }
    private int stageIndex = 0;

    public Plant()
    {

    }

    public void GoToNextStage()
    {
        // Increment our current stage
        if (stageIndex < stages.Length - 1)
        {
            stageIndex++;
        }
    }
}

public class PlantStage
{
    public int stageNum { get; private set; }
    public int daysInStage { get; private set; }
    public bool harvestable { get; private set; }

    public PlantStage(int stage, int days, bool canHarvest)
    {
        stageNum = stage;
        daysInStage = days;
        harvestable = canHarvest;
    }
}
