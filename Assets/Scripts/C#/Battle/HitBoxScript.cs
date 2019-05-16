using UnityEngine;
using System.Collections;

public class HitBoxScript : MonoBehaviour
{
    public float _damage;
    public float _durationInSeconds;
    public Vector2 _offset;
    public float _size;
    public float _invincible;
    public DamageType _type;

    private float _timer;
    private bool _active = true;

	void Start ()
    {
        _timer = 0f;
	}
	
	void Update ()
    {
	    if(_timer < _durationInSeconds)
        {
            _timer += Time.deltaTime;
            CheckRayCollideWithEntity();
        }
        else
        {
            Destroy(this.gameObject);
        }
	}

    public void HitBoxConstructor(float dam, float duration, Vector2 offset, float size, float invincible, DamageType type)
    {
        _damage = dam;
        _durationInSeconds = duration;
        _offset = offset;
        _size = size;
        _invincible = invincible;
        _type = type;
    }

    public void ChangeDamage(float damage)
    {
        _damage = damage;
    }

    public void ChangeOffset(Vector2 offset)
    {
        _offset = offset;
    }

    public void ChangeSize(float size)
    {
        _size = size;
    }

    public void ResetTimer()
    {
        _timer = 0f;
    }

    void CheckRayCollideWithEntity()
    {
        if (_active)
        {
            Vector2 ray = new Vector2(this.gameObject.transform.position.x + _offset.x, this.gameObject.transform.position.y + _offset.y);
            RaycastHit2D hit = Physics2D.CircleCast(ray, _size, Vector3.forward);

            if (hit.collider != null)
            {
                if (hit.collider.gameObject.GetComponent<EntityScript>())
                {
                    //print(hit.collider.gameObject.name + " Took " + _damage + " Damage");
                    hit.collider.gameObject.GetComponent<EntityScript>().TakeDamage(_damage, _invincible, _type);
                    _active = false;
                }
            }
        }
    }
}
