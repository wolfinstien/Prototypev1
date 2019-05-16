using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HealthBar : MonoBehaviour
{
    public float _maxSize = 150f;
    public RectTransform _bar;
    public Color _color;

    public Entity _entity;

    void Start ()
    {
        _bar.GetComponent<Image>().color = _color;
	}
	
	void Update ()
    {
        _bar.sizeDelta = new Vector2((_entity.Health / _entity.MaxHealth) * _maxSize, _bar.rect.height);
	}

    public void SetEntity(Entity en)
    {
        if (en != null)
        {
            _entity = en;
        }
    }

    public bool TakeDamage(float damage)
    {
        if ((_entity.Health - damage) > 0)
        {
            if(damage <= 0)
            {
                damage = 1f;
            }
            _entity.Health -= damage;
            return true;
        }
        else
        {
            _entity.Health = 0;
            return false;
        }
    }
}
