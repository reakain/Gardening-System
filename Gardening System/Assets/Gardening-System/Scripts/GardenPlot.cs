using System.Collections;
using System.Collections.Generic;
using CleverCrow.Fluid.BTs.Tasks;
using CleverCrow.Fluid.BTs.Trees;
using UnityEngine;

namespace GardenSystem
{
    public class GardenPlot : MonoBehaviour
    {
        [SerializeField]
        private BehaviorTree _tree;
        float dayTime = 0f;
        float dayTimer = 20f;
        [SerializeField] SpriteRenderer plotSprite;
        [SerializeField] SpriteRenderer plantSprite;
        [SerializeField] Sprite emptySprite;
        [SerializeField] Sprite tilledSprite;
        [SerializeField] Sprite shrivelSprite;
        [SerializeField] Color waterColor = new Color(124f / 256f, 120f / 256f, 142f / 256f);

        [SerializeField] bool isSeeded = false;
        [SerializeField] bool isTilled = false;
        [SerializeField] bool isWatered = false;
        [SerializeField] bool isShriveled = false;
        [SerializeField] bool inSeason = false;

        [SerializeField] int days = 0;
        [SerializeField] Season season = Season.Spring;
        [SerializeField] int shrivelDays = 0;

        [SerializeField] Plant plant;

        void Awake()
        {
            plotSprite = GetComponent<SpriteRenderer>();
            plotSprite.sprite = emptySprite;
            plotSprite.color = Color.white;
            plantSprite.sprite = null;
            
            _tree = new BehaviorTreeBuilder(gameObject)
                .Sequence()
                    .Condition("Is Tilled", () => isTilled)
                    .Selector()
                        .Sequence("Not Planted")
                            .Condition("Is Seeded", () => !isSeeded)
                            .Do("Erode", () =>
                            {
                            isTilled = false;
                            isWatered = false;
                            plotSprite.sprite = emptySprite;
                            plotSprite.color = Color.white;
                            plantSprite.sprite = null;
                            plant = null;
                            return TaskStatus.Success;
                            })
                        .End()
                        .Sequence("In Season")
                            .Condition("In Season", () => inSeason)
                            .Sequence("Grow")
                                .Condition("Is Watered", () => isWatered)
                                .Do("Grow", () =>
                                {
                                    isWatered = false;
                                    plotSprite.color = Color.white;
                                    days++;
                                    if (days >= plant.currentStage.daysInStage)
                                    {
                                        days = 0;
                                        plant.GoToNextStage();
                                        // Update sprite
                                        plantSprite.sprite = plant.currentStage.sprite;
                                    }
                                    return TaskStatus.Success;
                                })
                            .End()
                            .Sequence("Shrivel Countdown")
                                .Condition("Is Watered", () => !isWatered)
                                .Do("Countdown", () =>
                                {
                                    shrivelDays++;
                                    if(shrivelDays > plant.shrivelTime)
                                    {
                                        Shrivel();
                                    }
                                    return TaskStatus.Success;
                                })
                            .End()
                        .End()
                        .Do("Shrivel", () =>
                        {
                            Shrivel();
                            return TaskStatus.Success;
                        })
                        .End()
                    .End()
                .End()
                .Build();
        }

        // Update is called once per frame
        void Update()
        {
            dayTime += Time.deltaTime;
            //_tree.Tick();
            if(dayTime > dayTimer)
            {
                NewDay();
                dayTime = 0f;
            }
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
            if (plant != null)
            {
                inSeason = plant.InSeason(season);
            }
        }

        void Shrivel()
        {
            isTilled = false;
            isShriveled = true;
            isWatered = false;
            plant = null;
            days = 0;
            shrivelDays = 0;
            plotSprite.color = Color.white;
            plotSprite.sprite = tilledSprite;
            plantSprite.sprite = shrivelSprite;
        }

        public void Water()
        {
            if (isTilled)
            {
                isWatered = true;
                Debug.Log("Watered tile");
                plotSprite.color = waterColor;
            }
        }

        public void Till()
        {
            isTilled = true;
            Debug.Log("Tilled tile");
            // Change the tile sprite
            plotSprite.sprite = tilledSprite;
            plantSprite.sprite = null;
            plant = null;
            isShriveled = false;
            days = 0;
            shrivelDays = 0;
        }

        public void Harvest()
        {
            Debug.Log("Try harvest tile");
            if (plant != null && plant.currentStage.harvestable)
            {
                Debug.Log("Harvested!");
                plant.Harvest();
                if (plant.multiHarvest)
                {
                    plantSprite.sprite = plant.currentStage.sprite;
                }
                else
                {
                    plantSprite.sprite = null;
                    plant = null;
                }
            }
        }

        public void Seed(Plant newPlant)
        {
            if (isTilled && plant == null)
            {
                if (newPlant.InSeason(season))
                {
                    plant = newPlant;
                    plantSprite.sprite = plant.currentStage.sprite;
                    inSeason = true;
                }
            }
        }
    }

    [System.Serializable]
    public class GardenPlotData
    {

    }
}