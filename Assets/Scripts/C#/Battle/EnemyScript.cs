using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum EnemyType
{
    Normal,
    Tank,
    Archer,
    Mage,
}

public class EnemyScript : EntityScript
{
    public GameObject _meleeAttack;
    public GameObject _projectileAttack;
    public GameObject _magicAttack;
    public Transform _meleeWeapon;
    public Vector2 _hitboxCentre;
    public EnemyType _type;
    public Animator _anim;
    public List<string> _animationStates;
    public Entity _stats;
    public Enemy _enemy;
    public int _delay;

    private Transform _playerTransform;
    private float _count;

	void Start ()
    {
        _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        _stats = new Entity(_enemy.Health, _enemy.Strength, _enemy.Magic, _enemy.Agility);
        CreateHealthBar(_stats, Color.red);
        _delay = Mathf.RoundToInt(25f - _stats.Agility);
    }

    void FixedUpdate()
    {
        _count += Time.deltaTime;
        if(_count > _delay)
        {
            int state = Random.Range(0, _animationStates.Count);
            _anim.Play(_animationStates[state]);
            _count = 0;
        }
    }

    public override void TakeDamage(float value, float invincible, DamageType type)
    {
        float damage = 0f;

        switch(type)
        {
            case DamageType.Physical:
                damage = value - _stats.Strength;
                break;
            case DamageType.Magical:
                damage = value - _stats.Magic;
                break;
            case DamageType.Direct:
                damage = value;
                break;
        }

        base.TakeDamage(damage, invincible, type);
    }

    public override void DIE()
    {
        ReveiveRewards();
        base.DIE();
    }

    public void InitaiteAttack()
    {
        switch(_type)
        {
            case EnemyType.Archer:
                ArcherAttack();
                break;
            case EnemyType.Mage:
                MageAttack();
                break;
            default:
                AdvanceToPlayer();
                break;
        }
    }

    public void AdvanceToPlayer()
    {

    }

    public void MeleeAttack()
    {
        _invincible = true;
        if(_meleeWeapon != null)
        {
            CreateHitBox(_meleeWeapon, _stats.GetDamage(), 0.1f, _hitboxCentre, 0.1f, 0.5f, DamageType.Physical);
        }
        else
        {
            CreateHitBox(this.gameObject.transform, _stats.GetDamage(), 0.001f, _hitboxCentre, 1f, 0.5f, DamageType.Physical);
        }
    }

    public void ArcherAttack()
    {
        GameObject newShot = Instantiate(_projectileAttack) as GameObject;
        newShot.transform.position = Vector3.MoveTowards(transform.position, _playerTransform.position, 10 * Time.deltaTime);
    }

    public void MageAttack()
    {
        GameObject newSpell = Instantiate(_magicAttack) as GameObject;
        _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        newSpell.transform.position = new Vector3(_playerTransform.position.x, _playerTransform.position.y + 2f);
        newSpell.GetComponent<SpellScript>().SpellSetup(_stats.GetMagic(), DamageType.Magical, this);
    }

    public void ReveiveRewards()
    {
        int rand = Random.Range(0, 100);
        PersistentScript per = GameObject.FindGameObjectWithTag("Persistent").GetComponent<PersistentScript>();
        per._gold += _enemy.Gold;
        per._heat += _enemy.Heat;
        per._magic += _enemy.Essence;

        if (rand > 90)
        {
            print("Got a rare");
            per.AddInvenotryItem(_enemy.RareID, 1);
            return;
        }

        if (rand > 80)
        {
            print("Get a common");
            per.AddInvenotryItem(_enemy.CommonID, 1);
            return;
        }

        print("No item");
    }
}
