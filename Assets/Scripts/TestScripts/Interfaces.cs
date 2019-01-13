using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISetStateMachineBehaviour<T>
{
    void SetStateBehaviour(T state);
}

public interface ISetPlayerBehaviour<T>
{
    void SetPlayerBehaviour(T pbs);
}

public interface LimitVelocity<T,Y>
{
    T LimitXZVel(T Velocity, Y maxVel);
}
