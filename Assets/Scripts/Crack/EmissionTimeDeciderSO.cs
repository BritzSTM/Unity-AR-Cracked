using UnityEngine;

namespace ck.crack
{
    public class EmissionTimeDecider : ICrackEmissionDecider
    {
        private EmissionTimeDeciderSO _originSO;

        private Crack _crack;
        private float _pickedEmissionTime;

        public void Build(CrackEmissionDeciderSO originSO, Crack crack)
        {
            Debug.Assert(originSO != null && crack != null);

            _originSO = (EmissionTimeDeciderSO)originSO;
            _crack = crack;
        }

        public bool Decide()
        {
            if (Time.time > _crack.LastEmissionTime + _pickedEmissionTime)
                return true;

            return false;
        }

        public void UpdateState()
        {
            if(_crack.WasEmissionPrevFrame)
                _pickedEmissionTime = Random.Range(_originSO.MinSpawnTime, _originSO.MaxSpawnTime);
        }
    }

    [CreateAssetMenu(fileName = "new crack emission time so", menuName = "Game/Crack/EmissionTime")]
    public class EmissionTimeDeciderSO : CrackEmissionDeciderSO<EmissionTimeDecider>
    {
        public float MinSpawnTime = 3.0f;
        public float MaxSpawnTime = 5.0f;
    }
}
