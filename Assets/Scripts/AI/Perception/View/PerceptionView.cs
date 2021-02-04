using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
Classe : PerceptionView
Permet au module de perception de récoltées des informations visibles par l'agent grâce à un cône de perception
*/

public class PerceptionView 
{
    public float viewRadius;
    public float viewAngle;
    private LayerMask ObjectViewLayerMask;
    private LayerMask ObstacleViewLayerMask;

    private float UpdateRate = 0.2f;
    private float nextTime = 0.0f;
    private Agent owner;
    public List<Creature> OldVisibleCreature = new List<Creature>();
    public List<Creature> VisibleCreature = new List<Creature>();
    public List<Collider> VisibleObstacle = new List<Collider>();
    public List<GameObject> VisibleFood = new List<GameObject>();
    public List<SourceFood> VisibleSourceFood = new List<SourceFood>();
    public List<Nest> VisibleNest = new List<Nest>();
    public Collider[] ObjectsInViewRadius;

    public PerceptionView(Agent owner)
    {
        this.owner = owner;
        this.viewRadius = owner.PerceptionConfig.viewRadius;
        this.viewAngle = owner.PerceptionConfig.viewAngle;
        this.ObjectViewLayerMask = owner.PerceptionConfig.ObjectViewLayerMask;
        this.ObstacleViewLayerMask = owner.PerceptionConfig.ObstacleViewLayerMask;
    }
    public void Update()
    {
        if(Time.time > nextTime)
        {
            nextTime = Time.time + UpdateRate;
            FindVisibleTargets();
        }

        if(owner.Debug)
        {
            DisplayLine();
        }
    }

    private void FindVisibleTargets()
    {
        VisibleCreature.Clear();
        VisibleObstacle.Clear();
        VisibleFood.Clear();
        VisibleSourceFood.Clear();
        VisibleNest.Clear();

        ObjectsInViewRadius = Physics.OverlapSphere(owner.transform.position, viewRadius, ObjectViewLayerMask);

        foreach(Collider obj in ObjectsInViewRadius)
        {
            Vector3 target = obj.bounds.center;
            Vector3 dirToTarget = (target - owner.transform.position).normalized;

            // Si l'objet est dans la zone de perception
            if(Vector3.Angle(owner.transform.forward,dirToTarget ) < viewAngle / 2)
            {
                if (obj.gameObject.layer == LayerMask.NameToLayer("Obstacle"))
                {
                    AnalyseAndSaveObstacle(obj);
                }
                else
                {
                    float distToTarget = Vector3.Distance(owner.transform.position, target);

                    if(!Physics.Raycast(owner.transform.position,dirToTarget,distToTarget,ObstacleViewLayerMask))
                    {
                        if(obj.gameObject.layer == LayerMask.NameToLayer("Creature") && obj.GetComponent<Agent>() != owner)
                        {
                            AnalyseAndSaveCreature(obj.GetComponent<Creature>()); 
                        }

                        if(obj.gameObject.layer == LayerMask.NameToLayer("Food"))
                        {
                            AnalyseAndSaveFood(obj.gameObject);
                        }

                        if(obj.gameObject.layer == LayerMask.NameToLayer("SourceFood"))
                        {
                            AnalyseAndSaveFoodSource(obj.GetComponent<SourceFood>());
                        }

                        if(obj.gameObject.layer == LayerMask.NameToLayer("Nest"))
                        {
                            AnalyseAndSaveNest(obj.GetComponent<Nest>());
                        }

                        if(obj.gameObject.layer == LayerMask.NameToLayer("Player"))
                        {
                            AnalyseAndSavePlayer();
                        }
                    }
                }
            }
        }
        owner.Perception.RevisionView.RevisionUpdate(OldVisibleCreature,VisibleCreature);
        OldVisibleCreature = new List<Creature>(VisibleCreature);
    }

    #region AnalyseAndSaveFunctions  
    private void AnalyseAndSaveObstacle(Collider obstacle)
    {
        VisibleObstacle.Add(obstacle);
        owner.Memory.Obstacles.Write(new DataObstacle(obstacle));
    }

    private void AnalyseAndSaveCreature(Creature creature)
    {
        VisibleCreature.Add(creature);
        owner.Memory.Creatures.Write(new DataCreature(creature,creature.transform.position));
    }

    private void AnalyseAndSaveFood(GameObject food)
    {
        VisibleFood.Add(food);
        owner.Memory.Foods.Write(new DataFood(food.GetComponent<Food>(),food.transform.position));
    }

    private void AnalyseAndSaveFoodSource(SourceFood foodsource)
    {
        VisibleSourceFood.Add(foodsource);  
        owner.Memory.FoodSources.Write(new DataSourceFood(foodsource));
    }

    private void AnalyseAndSaveNest(Nest nest)
    {
        VisibleNest.Add(nest);  
        owner.Memory.Nests.Write(new DataNest(nest));
    }

    private void AnalyseAndSavePlayer()
    {
        owner.Memory.Player.lastSeeTime = Time.time;
    }
    #endregion  


    private void DisplayLine()
    {
        foreach(Creature visibleCreature in VisibleCreature)
        {
            if(visibleCreature != null)
            {
                Debug.DrawLine(owner.transform.position, visibleCreature.transform.position,Color.blue);
            }
        }

        foreach(Collider obstacle in VisibleObstacle)
        {
            Debug.DrawLine(owner.transform.position, obstacle.transform.position,Color.black);
        }

        foreach(GameObject food in VisibleFood)
        {
            if(food != null)
            {
                Debug.DrawLine(owner.transform.position, food.transform.position,Color.yellow);
            }
        }

        foreach(SourceFood foodsource in VisibleSourceFood)
        {
            Debug.DrawLine(owner.transform.position, foodsource.transform.position,Color.green);
        }

        foreach(Nest nest in VisibleNest)
        {
            Debug.DrawLine(owner.transform.position, nest.transform.position,Color.cyan);
        }
    }
}
