using UnityEngine;

namespace ck.crack
{
    public class BrustTimeDecider : ICrackEmissionDecider
    {
        private BrustTimeDeciderSO _originSO;
        private Crack _crack;
        private ICrackEmissionDecider _emissionTimeDecider;

        private bool _isBrustTime;
        private float _pickedBurstTime;
        private float _lastBurstTime;

        public void Build(CrackEmissionDeciderSO originSO, Crack crack)
        {
            _originSO = (BrustTimeDeciderSO)originSO;
            _crack = crack;

            _emissionTimeDecider = _originSO.EmissionTimeSO.Create();
            _emissionTimeDecider.Build(_originSO.EmissionTimeSO, _crack);

            _pickedBurstTime = Random.Range(_originSO.MinBurstTime, _originSO.MaxBurstTime);
        }

        public bool Decide()
        {
            if (!_isBrustTime)
                return false;

            return _emissionTimeDecider.Decide();
        }

        public void UpdateState()
        {
            if(_isBrustTime)
            {
                _emissionTimeDecider.UpdateState();

                if (_lastBurstTime + _originSO.BurstDurationTime > Time.time)
                    return;

                _isBrustTime = false;
                _lastBurstTime = Time.time;

                _originSO.OnUnbrustTimeEventSO.RaiseEvent(_crack);
            }
            else
            {
                if (_lastBurstTime + _pickedBurstTime > Time.time)
                    return;

                _isBrustTime = true;
                _lastBurstTime = Time.time;
                _pickedBurstTime = Random.Range(_originSO.MinBurstTime, _originSO.MaxBurstTime);

                _originSO.OnBrustTimeEventSO.RaiseEvent(_crack);
            }
        }
    }

    [CreateAssetMenu(fileName = "new crack brust time so", menuName = "Game/Crack/BrustTime")]
    public class BrustTimeDeciderSO : CrackEmissionDeciderSO<BrustTimeDecider>
    {
        [Header("Raise events")]
        public EventTypeCrack OnBrustTimeEventSO;
        public EventTypeCrack OnUnbrustTimeEventSO;

        [Header("Brust Desc")]
        public float MinBurstTime = 10.0f;
        public float MaxBurstTime = 20.0f;
        public float BurstDurationTime = 10.0f;
        public EmissionTimeDeciderSO EmissionTimeSO;
    }
}
