using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class StrategySpawner : MonoBehaviour
{
    [SerializeField] private SpawnDeciderSO[] _decidersSO;
    private ISpawnDecider[] _deciders;

    [SerializeField] private SpawnTypeSO[] _spawnTypes;

    [SerializeField] private SpawnDeployerSO[] _deployersSO;
    private ISpawnDeployer[] _deployers;

    /// <summary>   
    /// 오브젝트가 배치되기 전 사전 이벤트
    /// </summary>
    public event UnityAction<GameObject[]> OnDeploy;

    /// <summary>
    /// 오브젝트가 배치되었을 때 발생하는 이벤트
    /// </summary>
    public event UnityAction<GameObject[]> OnDeployed;

    private void Awake()
    {
        Inits();
    }

    private void Update()
    {

    }

    /// <summary>
    /// 초기화 함수. 만약 상속받아 Awake를 재정의 했다면 이 함수를 호출 할 것 
    /// </summary>
    protected void Inits()
    {
        InitMembers();
        InitStrategy();
    }

    /// <summary>
    /// 맴버 초기화 함수
    /// </summary>
    protected virtual void InitMembers() { }

    private void InitStrategy()
    {

    }
}
