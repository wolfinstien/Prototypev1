using UnityEngine;
using System.Collections;

public class PlayerSpellScript : MonoBehaviour
{
    public Vector2 _spawnLocation;
    public bool _mobile;
    public Vector2 _direction;
    public float _speed;
    public bool _animationCreated;
    public float _duration;
    public Vector2 _offset;
    public float _size;

    private Ability _ability;
    private PlayerData _stats;
    private EntityScript _reference;
    private float _count;

	void Start ()
    {
	
	}
	
	void Update ()
    {
        if (!_animationCreated)
        {
            _count += Time.deltaTime;
            if (_count > _ability.Delay)
            {
                _reference.CreateHitBox(this.gameObject.transform, _ability.Base + (_stats.GetDamageFromType(_ability.Damage) * _ability.Scale), _duration, _offset, _size, 1f, _ability.Damage);
            }
        }
	}

    public void Set(Ability ability, PlayerData player, EntityScript reference)
    {
        _ability = ability;
        _stats = player;
        _reference = reference;
    }

    public void CreateHitBox()
    {
        _reference.CreateHitBox(this.gameObject.transform, _ability.Base + (_stats.GetDamageFromType(_ability.Damage) * _ability.Scale), _duration, _offset, _size, 1f, _ability.Damage);
    }
}
