using UnityEngine;

public class Gun : MonoBehaviour
{
    [Header("Gun type desc")]
    [SerializeField] private Color _color;
    public Color CurrentColor { get => _color; }

    [SerializeField] private int _damage = 1;

    [Header("Raise Events")]
    [SerializeField] private EventTypeAudioClip _playSoundFXSO;

    [Header("FXs")]
    [SerializeField] private ParticleSystem _gunVisualEffect;
    [SerializeField] private AudioClip[] _gunSoundEffects;

    private Transform _tr;
    private RaycastHit[] _raycastHits = new RaycastHit[1];

    private void Awake()
    {
        _tr = GetComponent<Transform>();
    }

    public void Fire()
    {
        if (Physics.RaycastNonAlloc(_tr.position, _tr.forward, _raycastHits) > 0)
        {
            if (_raycastHits[0].transform.tag == "Unit")
            {
                var target = _raycastHits[0].transform.GetComponent<IDamageable>();
                if (target != null)
                    ToDamage(target);
            }
        }

        PlayEffects();
    }

    public void Fire(Vector3 target)
    {
        Aim(target);
        Fire();
    }

    public void Aim(Vector3 target) => transform.rotation = Quaternion.LookRotation(target - _tr.position);

    private void PlayEffects()
    {
        _gunVisualEffect.Play();

        int pickedSound = Random.Range(0, _gunSoundEffects.Length);
        _playSoundFXSO?.RaiseEvent(_gunSoundEffects[pickedSound]);
    }

    private void ToDamage(IDamageable target)
    {
        IDamageable.DamageType damage;
        damage.color = _color;
        damage.damage = _damage;

        target.OnDamage(damage);
    }
}
