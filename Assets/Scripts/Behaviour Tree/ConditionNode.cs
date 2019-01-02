using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//declareert Delegate
//Type waarin je een methode kan opslaan | Pointer naar een methode
//Je kan hier en methode instoppen die een bool teruggeeft en geen parameters aanvaard
//Als de delegate specifiek voor de class dient, stop hem in die class



//Composition (2de methode van werken) - Iets meegeven als parameter dat opgeslagen en gebruikt wordt
public class ConditionNode : INode
{
    public delegate bool Condition();

    private readonly Condition _condition;

    public ConditionNode(Condition condition)
    {
        _condition = condition;
    }

    public IEnumerator<NodeResult> Tick()
    {
        if (_condition())
        {
            yield return NodeResult.Succes;
        }
        else
        {
            yield return NodeResult.Failure;
        }

    }
  

    //Abstract manier van werken - Verplicht om iedere conditie een nieuwe class te maken
    //Public abstract class  ConditionNode : Inode
    //{


    //}


    //ConditionNode isHungryCondition = new ConditionNode(() => Random.Range(0, 1) > 0.3);
    //public class IsHungryCondition : ConditionNode
    //{
    //    protected override ConditionNode()
    //    {
    //        return Random.Range(0, 1) > 0.3;
    //    }
    //}
}
