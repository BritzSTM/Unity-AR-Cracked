using UnityEngine;

public class Crack : MonoBehaviour
{
    [Header("RaiseEvents")]
    [SerializeField] private EventTypeGameObject _trackingRequestEventSO;
    [SerializeField] private EventTypeGameObject _untrackingRequestEventSO;

    [Header("Deciders")]
    [SerializeField] private CrackEmissionDeciderSO[] _deciderSOs;
    private ICrackEmissionDecider[] _deciders;

    [Header("Spawn desc")]
    [SerializeField] private GameObject[] _dimObjects;


    public bool WasEmissionPrevFrame { get; private set; }
    public float CreationTime { get; private set; }
    public float LastEmissionTime { get; private set; }
    public float EmissionForce = 4.0f;

    private Transform _tr;
    private Vector3[] _cahcedVector3 = new Vector3[8];

    private void Awake()
    {
        Debug.Assert(_trackingRequestEventSO != null & _untrackingRequestEventSO != null);
        _tr = GetComponent<Transform>();
    }

    private void OnEnable()
    {
        _trackingRequestEventSO.RaiseEvent(gameObject);
    }

    private void Start()
    {
        CreationTime = Time.time;
        InitDeciders();
    }

    private void OnDisable()
    {
        _untrackingRequestEventSO.RaiseEvent(gameObject);
    }

    private void Update()
    {
        UpdateDeciders();

        if(!DecideEmission())
        {
            WasEmissionPrevFrame = false;
            return;
        }

        EmitDim();
        WasEmissionPrevFrame = true;
        LastEmissionTime = Time.time;
    }

    private void InitDeciders()
    {
        _deciders = new ICrackEmissionDecider[_deciderSOs.Length];
        for(int i = 0; i < _deciderSOs.Length; ++i)
        {
            _deciders[i] = _deciderSOs[i].Create();
            _deciders[i].Build(_deciderSOs[i], this);
        }
    }

    private void EmitDim()
    {
        int pickedDIm = Random.Range(0, _dimObjects.Length);
        var createdDim = Instantiate(_dimObjects[pickedDIm], _tr.position, Quaternion.identity);
        var rbody = createdDim.GetComponent<Rigidbody>();

        // 발사할 각도 조정
        float half = _tr.eulerAngles.x / 2.0f;
        _cahcedVector3[0].x = Random.Range(-half, half);
        _cahcedVector3[0].y = _tr.eulerAngles.y;
        half = _tr.eulerAngles.z / 2.0f;
        _cahcedVector3[0].z = Random.Range(-half, half);

        var prevRot = _tr.rotation;
        _tr.rotation = Quaternion.LookRotation(_cahcedVector3[0]);
        rbody.AddForce(_tr.forward * EmissionForce);
        _tr.rotation = prevRot;
    }

    private void UpdateDeciders()
    {
        for(int i = 0; i < _deciders.Length; ++i)
        {
            _deciders[i].UpdateState();
        }
    }

    private bool DecideEmission()
    {
        bool b = false;
        for(int i = 0; i < _deciders.Length; ++i)
        {
            if(_deciders[i].Decide())
            {
                b = true;
                break;
            }
        }

        return b;
    }
}
