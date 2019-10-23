using System.Collections;
using System.Collections.Generic;
using CleverCrow.Fluid.BTs.Tasks;
using CleverCrow.Fluid.BTs.Trees;
using UnityEngine;

public class GardenPlot : MonoBehaviour
{
    [SerializeField]
    private BehaviorTree _tree;

    bool isSeeded = false;
    bool isTilled = false;
    bool isWatered = false;

    int days = 0;
    int season = 0;

    Plant plant;

    void Awake()
    {
        _tree = new BehaviorTreeBuilder(gameObject)
            .Selector()
                .Condition("Is Tilled", () => isTilled)
                .Sequence("Erode")
                    .Condition("Is Seeded", () => !isSeeded )
                    .Do("Erode", () =>
                    {
                        isTilled = false;
                        // Change sprite
                        return TaskStatus.Success;
                    })
                .End()
                .Sequence("Grow")
                    .Condition("Is Seeded", () => isSeeded)
                    .Condition("In Season", () => season == plant.season)
                    .Condition("Is Watered", () => isWatered)
                    .Do("Grow", () =>
                    {
                        days++;
                        if (days >= plant.currentStage.daysInStage)
                        {
                            days = 0;
                            plant.GoToNextStage();
                            // Update sprite
                        }
                        return TaskStatus.Success;
                    })
                .End()
                .Sequence("Shrivel")
                .End()
            .End()
            .Build();
    }

    // Update is called once per frame
    void Update()
    {
        _tree.Tick();
    }

    void NewDay()
    {
        // Get day count for grow
        _tree.Tick();

    }

    void NewSeason()
    {
        // Get the new season
        // change tile sprite
    }

    public void Water()
    {
        isWatered = true;
        // Change tile sprite to water
    }

    public void Till()
    {
        isTilled = true;
        // Change the tile sprite
    }

    public void Harvest()
    {
        if(plant.currentStage.harvestable)
        {
            // Get your produce
            // Do something with the plot??
        }
    }
}
