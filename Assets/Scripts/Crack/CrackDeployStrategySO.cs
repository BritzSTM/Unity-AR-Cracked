using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICrackDeployStrategy
{
    /// <summary>
    /// 전략 객체의 초기화를 위한 함수
    /// </summary>
    void Awake(CrackManager manager);

    /// <summary>
    /// 전락 객체의 상태 업데이트
    /// </summary>
    void Update();

    /// <summary>
    /// 배치 가능한 상태 여부를 반환
    /// </summary>
    bool Deployable();

    /// <summary>
    /// 생성된 객체를 반환.
    /// </summary>
    GameObject Deploy();
}

/// <summary>
/// CrackManager에서 사용하는 전략객체 타입
/// </summary>
public abstract class CrackDeployStrategySO : ScriptableObject
{
    public abstract ICrackDeployStrategy CreateStrategy();
}

/// <summary>
/// CrackDeployStrategySO의 구체화를 위한 제네릭 타입
/// </summary>
public class CrackDeployStrategySO<T> : CrackDeployStrategySO
    where T : ICrackDeployStrategy, new()
{
    public override ICrackDeployStrategy CreateStrategy() => new T();
}
