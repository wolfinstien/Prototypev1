using UnityEngine;
using System.Collections;

public enum DamageType
{
    Physical,
    Magical,
    Direct
}

[System.Serializable]
public class Entity
{
    public float MaxHealth;
    public float Health;
    public float Strength;
    public float Magic;
    public float Agility;

    public Entity(float hp, float str, float mag, float agi)
    {
        MaxHealth = hp;
        Health = hp;
        Strength = str;
        Magic = mag;
        Agility = agi;
    }

    public float GetDamageFromType(DamageType type)
    {
        switch(type)
        {
            case DamageType.Magical:
                return GetMagic();
            case DamageType.Physical:
                return GetDamage();
            default:
                return GetDamage();
        }
    }

    public float GetResistanceFromType(DamageType type)
    {
        switch(type)
        {
            case DamageType.Magical:
                return GetResistance();
            case DamageType.Physical:
                return GetArmor();
            case DamageType.Direct:
                return 0f;
            default:
                return GetArmor();
        }
    }

    public virtual float GetDamage()
    {
        return Strength;
    }

    public virtual float GetMagic()
    {
        return Magic;
    }

    public virtual float GetArmor()
    {
        return Strength;
    }

    public virtual float GetResistance()
    {
        return Magic;
    }

    public virtual float GetAgility()
    {
        return Agility;
    }
}

public class EntityScript : MonoBehaviour
{
    public Sprite _hitboxSprite;
    public GameObject _barObj;
    public HealthBar _healthBar;
    public bool _invincible;
    public float _duration;
    public float _timer;
    public GameObject _result;

    void Start()
    {

    }
	
	void Update ()
    {
        if(_invincible)
        {
            _timer += Time.deltaTime;
            if(_timer > _duration)
            {
                _timer = 0f;
                _duration = 0f;
                _invincible = false;
            }
        }
    }

    public void CreateHealthBar(Entity entity, Color color)
    {
        if(_barObj != null)
        {
            GameObject newBar = Instantiate(_barObj) as GameObject;
            newBar.transform.SetParent(GameObject.FindGameObjectWithTag("UI").transform, false);
            newBar.transform.position = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>().WorldToScreenPoint(
                new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y + 3f));
            newBar.GetComponent<HealthBar>().SetEntity(entity);
            newBar.GetComponent<HealthBar>()._color = color;
            _healthBar = newBar.GetComponent<HealthBar>();
        }
    }

    public void GameObjectMaker(GameObject obj)
    {

    }

    public virtual void TakeDamage(float value, float invincible, DamageType type)
    {
        if (!_invincible)
        {
            float damage = value;
            _invincible = true;
            _duration = invincible;
            if(damage <= 1)
            {
                damage = 1f;
            }
            if(!_healthBar.TakeDamage(damage))
            {
                DIE();
            }
            ShowResult(damage, type);
        }
    }

    public virtual void DIE()
    {
        GameObject.FindGameObjectWithTag("Battle").GetComponent<FightSceneScript>().EntityDeath(this.gameObject.name);
    }

    public void ShowResult(float value, DamageType type)
    {
        Color colour = Color.white;
        GameObject newResult = Instantiate(_result) as GameObject;
        newResult.transform.SetParent(GameObject.FindGameObjectWithTag("UI").transform, false);
        newResult.transform.position = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>().WorldToScreenPoint(
            new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y - 2f));

        switch(type)
        {
            case DamageType.Magical:
                colour = Color.magenta;
                break;
            case DamageType.Physical:
                colour = Color.red;
                break;
        }

        newResult.GetComponent<DamageResult>().SetResult(value, colour);
    }

    public void CreateHitBox(Transform parent, float damage, float duration, Vector2 offset, float size, float invincible, DamageType type)
    {
        GameObject hitBoxObject = new GameObject("HitBox");
        hitBoxObject.transform.SetParent(parent);
        hitBoxObject.transform.position = new Vector3(parent.position.x + offset.x, parent.transform.position.y + offset.y, 0f);
        hitBoxObject.AddComponent<HitBoxScript>();
        hitBoxObject.GetComponent<HitBoxScript>().HitBoxConstructor(damage, duration, Vector2.zero, size, invincible, type);
        if(_hitboxSprite != null)
        {
            hitBoxObject.AddComponent<SpriteRenderer>().sprite = _hitboxSprite;
        }
    }
}
