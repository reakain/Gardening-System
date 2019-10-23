using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GardenSystem
{
    [System.Serializable]
    public enum Season
    {
        Spring,
        Summer,
        Fall,
        Winter
    }

    [System.Serializable]
    public class Plant
    {
        public Season[] seasons { get; private set; }

        public PlantStage[] stages { get; private set; }

        public PlantStage currentStage { get { return stages[stageIndex]; } }
        private int stageIndex = 0;

        public int shrivelTime { get; private set; }

        public bool multiHarvest { get; private set; }

        public int cropYield { get; private set; }

        public Plant(Season[] activeSeasons, PlantStage[] plantStages, int shriveldays, bool loopHarvest, int yield)
        {
            seasons = activeSeasons;
            stages = plantStages;
            shrivelTime = shriveldays;
            stageIndex = 0;
            multiHarvest = loopHarvest;
            cropYield = yield;
        }

        public void GoToNextStage()
        {
            // Increment our current stage
            if (stageIndex < stages.Length - 1)
            {
                stageIndex++;
            }
        }

        public int Harvest()
        {
                if (multiHarvest)
                {
                    stageIndex--;
                }
                return cropYield;
        }

        public bool InSeason(Season season)
        {
            foreach(var seas in seasons)
            {
                if (seas == season)
                {
                    return true;
                }
            }
            return false;
        }
    }

    [System.Serializable]
    public class PlantStage
    {
        public int stageNum { get; private set; }
        public int daysInStage { get; private set; }
        public bool harvestable { get; private set; }
        public Sprite sprite { get; private set; }

        public PlantStage(int stage, int days, bool canHarvest, Sprite stageSprite)
        {
            stageNum = stage;
            daysInStage = days;
            harvestable = canHarvest;
            sprite = stageSprite;
        }
    }
}