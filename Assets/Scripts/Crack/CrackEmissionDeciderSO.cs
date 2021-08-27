using UnityEngine;

public interface ICrackEmissionDecider
{
    void Build(CrackEmissionDeciderSO originSO, Crack crack);

    bool Decide();
    void UpdateState();
}

public abstract class CrackEmissionDeciderSO : ScriptableObject
{
    public abstract ICrackEmissionDecider Create();
}

public class CrackEmissionDeciderSO<T> : CrackEmissionDeciderSO
    where T : ICrackEmissionDecider, new() 
{
    public override ICrackEmissionDecider Create() => new T();
}
