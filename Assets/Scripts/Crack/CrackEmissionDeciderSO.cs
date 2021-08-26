using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICrackEmissionDecider
{
    void Build(Crack crack);

    bool Decide();
}

public abstract class CrackEmissionDeciderSO : MonoBehaviour
{
    public abstract ICrackEmissionDecider Create();
}

public class CrackEmissionDeciderSO<T> : MonoBehaviour
    where T : ICrackEmissionDecider, new() 
{
    public ICrackEmissionDecider Create() => new T();
}
