using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISetBehaviour<T,Y,A>
{
    T SetPlayerBehaviour(T pbs);
    Y SetAIBehaviour(Y ais,A animator);
}

public interface LimitVelocity<T,Y>
{
    T LimitXZVel(T Velocity, Y maxVel);
}
