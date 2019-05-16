using UnityEngine;
using System.Collections;

public class SpellScript : MonoBehaviour
{
    public float _damage;
    public DamageType _type;
    public float _size;
    public Vector2 _offset;

    private EntityScript _reference;

	void Start ()
    {
	
	}
	
	void Update ()
    {

	}

    public void SpellSetup(float damage, DamageType type, EntityScript entity)
    {
        _damage = damage;
        _type = type;
        _reference = entity;

    }

    public void SpellHitBox()
    {
        //EntityScript newEntity = new EntityScript();
        _reference.CreateHitBox(this.gameObject.transform, _damage, 0.1f, _offset, _size, 0.5f, _type);
    }
}
