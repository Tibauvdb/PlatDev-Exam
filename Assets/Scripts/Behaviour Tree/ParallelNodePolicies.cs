using System;
using UnityEngine;
using UnityScript.Steps;
//Currying Example
//Curry method

public class Test
{
    public delegate int add(int z);

    public add CreateAdder(int i)
    {
        //InputParameters => Expressions
        //When creating CreateAdder(int i), you will assign the i
        //when calling the var originally made, you will assign the a
        return (a) => (i + a);
    }

    public void Testing()
    {
        int i = Console.Read();
        var addSomeNumber = CreateAdder(i);

        var addTwo = CreateAdder(2);
        //a+3
        var AddThree = CreateAdder(3);
        //5+3
        Debug.Log(addTwo(5));
        
    }
}















//Delegate is niet voldoende
//we hebben iets nodig die states heeft -> interface
public interface ParallelNodePolicyAccumulator
{
    NodeResult Policy(NodeResult result);
}

//If N succes' , stop ParallelNode
public class NSuccesIsSuccesAccumulator : ParallelNodePolicyAccumulator
{    

    public static ParallelNodePolicyAccumulator Factory()
    {
        return new NSuccesIsSuccesAccumulator(2);
    }
    //currying ?
    public static ParallelNode.Policy Factory(int n)
    {
        return () => new NSuccesIsSuccesAccumulator(n);
    }

    private readonly int _n = 1;
    private int _count = 0;

    public NSuccesIsSuccesAccumulator(int n)
    {
        _n = n;
    }

    public NodeResult Policy(NodeResult result)
    {
        if (result == NodeResult.Succes)
        {
            _count++;
        }
        return (_count >= _n) ? NodeResult.Succes : NodeResult.Failure;
    }
}

//Add other Policies Below

