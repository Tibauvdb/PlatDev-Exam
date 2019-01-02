using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SheepAIBehaviour : MonoBehaviour {
    private INode _rootNode;
    // Use this for initialization
    void Start()
    {  
        INode HungryAI = new SequenceNode(
            new ConditionNode(IsHungry),
            new ActionNode(Eat));
        INode SleepyAI = new SequenceNode(
            new ConditionNode(IsSleepy),
            new ActionNode(Sleep));

        //1ste en 2de waarde zijn een int en het geeft een int terug v
        //Func<int,int,int> addTwoNumber = (int a, int b) => a + b;


        //methode met als returnwaarde een nieuwe instantie !
        ParallelNode.Policy _2SuccesPolicy = () => new NSuccesIsSuccesAccumulator(2);

        _rootNode =
            new ParallelNode(NSuccesIsSuccesAccumulator.Factory(2),
                new SelectorNode(
                    HungryAI, 
                    SleepyAI,
                    new ActionNode(Roam)),
                new ActionNode(Blaah)
                );

        //Create nodes
        ConditionNode isHungryCondition = new ConditionNode(IsHungry);
        ConditionNode isSleepyCondition = new ConditionNode(IsSleepy);

        StartCoroutine(RunTree());
    }

    IEnumerator RunTree()
    {
        while (Application.isPlaying)
        {
            yield return _rootNode.Tick();
        }
    }


    NodeResult FindFood()
    {
        return NodeResult.Succes;
    }

    IEnumerator<NodeResult> Eat()
    {
        Debug.Log("Eating");
        yield return NodeResult.Succes;
    }
    IEnumerator<NodeResult> Sleep()
    {
    Debug.Log("Sleeping");
    yield return NodeResult.Succes;
    }
    IEnumerator<NodeResult> Blaah()
    {
        Debug.Log("blaah");
        yield return NodeResult.Succes;
    }
    IEnumerator<NodeResult> Roam()
    {
        Debug.Log("Roaming");
        yield return NodeResult.Succes;
    }

    bool IsHungry()
    {
        bool isHungry =Random.Range(0.0f, 1.0f) > 0.3f;
        Debug.Log("IsHungry " + isHungry);
        return isHungry;
    }

    bool IsSleepy()
    {
        bool IsSleepy = Random.Range(0.0f, 1.0f) > 0.3f;
        Debug.Log("IsSleepy " + IsSleepy);
        return IsSleepy;
    }



    
}
