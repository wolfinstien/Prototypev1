using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class PlayerData : Entity
{
    public Equipment Weapon;
    public Equipment Armor;
    public List<Consumable> Consumables = new List<Consumable>();
    public List<Ability> Abilities = new List<Ability>();
    Consumable boost;

    public PlayerData(float hp, float str, float mag, float agi) : base(hp, str, mag, agi)
    {
        Weapon = new Equipment();
        Armor = new Equipment();
    }

    public PlayerData(float hp, float str, float mag, float agi, Equipment wpn, Equipment arm) : base(hp + wpn.Health + arm.Health, str + wpn.Health + arm.Health, mag, agi)
    {
        Weapon = wpn;
        Armor = arm;
    }

    public void AddBoost(Consumable consumable)
    {
        if(boost == null)
        {
            boost = consumable;
        }
    }

    public void CheckTime(float time)
    {
        if(time > boost.Duration)
        {
            boost = null;
        }
    }

    public override float GetDamage()
    {
        if (boost != null)
        {
            if (boost.Type == ConsumableType.Strength)
            {
                return (Strength * (1f + (boost.Strength / 10))) + Weapon.Strength;
            }
        }
        return Strength + Weapon.Strength;
    }

    public override float GetMagic()
    {
        if (boost != null)
        {
            if (boost.Type == ConsumableType.Magic)
            {
                return (Magic * (1f + (boost.Strength / 10))) + Weapon.Magic;
            }
        }
        return Magic + Weapon.Magic;
    }

    public override float GetArmor()
    {
        if (boost != null)
        {
            if (boost.Type == ConsumableType.Strength)
            {
                return (Strength * (1f + (boost.Strength / 10))) + Weapon.Strength;
            }
        }
        return Strength + Weapon.Strength;
    }

    public override float GetResistance()
    {
        if (boost != null)
        {
            if (boost.Type == ConsumableType.Magic)
            {
                return (Magic * (1f + (boost.Strength / 10))) + Weapon.Magic;
            }
        }
        return Magic + Weapon.Magic;
    }

    public override float GetAgility()
    {
        if (boost != null)
        {
            if (boost.Type == ConsumableType.Agility)
            {
                return (Agility * (1f + (boost.Strength / 10))) + Weapon.Agility + Armor.Agility;
            }
        }
        return Agility + Weapon.Agility + Armor.Agility;
    }

    public void SetConsumables(List<Consumable> consumables)
    {
        Consumables = consumables;
    }
}

public class PlayerScript : EntityScript
{
    public PlayerData _player;
    public bool _set;
    public GameObject _abilitySlots;
    public GameObject _abilityButton;
    public GameObject _potionButtons;
    public List<GameObject> _projectiles;

    private bool _blocking;
    private Ability _selectedAbility;

    void Start()
    {
        if(_set)
        {
            _player = new PlayerData(30f,10f,5f,12f);
        }
        CreateHealthBar(_player, Color.green);
        CreatAbilitySlots();
        CreateConsumableSlots();
    }

    public void Block()
    {
        _blocking = true;
    }

    public void Neutral()
    {
        _blocking = false;
    }

    public override void TakeDamage(float value, float invincible, DamageType type)
    {
        float damage = value;

        switch(type)
        {
            case DamageType.Physical:
                damage = value - _player.GetArmor();
                break;
            case DamageType.Magical:
                damage = value - _player.GetResistance();
                break;
        }

        if(_blocking)
        {
            damage = damage / 2f;
        }

        base.TakeDamage(damage, invincible, type);
    }

    public void ActivateCounter()
    {
        CreateHitBox(this.gameObject.transform.GetChild(1), _player.GetDamage(), 0.1f, new Vector2(-1f, 1f), 0.4f, 1f, DamageType.Physical);
    }

    public void Attack(Ability ability)
    {
        _selectedAbility = ability;
        StartCoroutine(HitBoxDelay(ability.Delay, ability));
    }

    IEnumerator HitBoxDelay(float seconds, Ability ability)
    {
        yield return new WaitForSeconds(seconds);

        switch (ability.StateName)
        {
            case "MeleeAttack":
                CreateHitBox(this.gameObject.transform.GetChild(1), ability.Base + (_player.GetDamage() * ability.Scale)
                    , 0.1f, new Vector2(1.5f, 0f), 0.4f, 1f, DamageType.Physical);
                break;
        }
    }

    public void SpellCast(int index)
    {
        GameObject newSpell = Instantiate(_projectiles[index]) as GameObject;
        newSpell.transform.SetParent(this.gameObject.transform.GetChild(0));
        newSpell.transform.position = this.gameObject.transform.position;
        newSpell.GetComponent<PlayerSpellScript>().Set(_selectedAbility, _player, this);
    }

    void CreatAbilitySlots()
    {
        GameObject newSlots = Instantiate(_abilitySlots) as GameObject;
        newSlots.transform.SetParent(GameObject.FindGameObjectWithTag("Abilities").transform, false);
        newSlots.transform.position = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>().WorldToScreenPoint(new Vector3(0f, -3f));

        print(_player.Abilities.Count);

        if (_player.Abilities.Count > 0)
        {
            foreach (Ability ability in _player.Abilities)
            {
                GameObject newButton = Instantiate(_abilityButton) as GameObject;
                newButton.transform.SetParent(newSlots.transform, false);
                newButton.GetComponent<PlayerAbilityButton>()._ability = ability;
            }
        }
        else
        { 
            GameObject newButton = Instantiate(_abilityButton) as GameObject;
            Ability newAbility = new Ability();
            //("MeleeAttack", 2f, 0.5f, DamageType.Physical, 5f, 1f)
            newAbility.Name = "Basic Attack";
            newAbility.StateName = "MeleeAttack";
            newAbility.Cooldown = 2f;
            newAbility.Delay = 0.5f;
            newAbility.Base = 5f;
            newAbility.Scale = 1f;
            newAbility.Damage = DamageType.Physical;
            newAbility.Type = AbilityType.Damage;
            newButton.transform.SetParent(newSlots.transform, false);
            newButton.GetComponent<PlayerAbilityButton>()._ability = newAbility;
        }

        newSlots.GetComponent<RectTransform>().sizeDelta = new Vector2(128f * newSlots.transform.childCount, 128f);
    }

    void CreateConsumableSlots()
    {
        if(_player.Consumables.Count > 0)
        {
            GameObject.FindGameObjectWithTag("Potions").SetActive(true);
            GameObject newSlots = Instantiate(_abilitySlots) as GameObject;
            newSlots.transform.SetParent(GameObject.FindGameObjectWithTag("Potions").transform, false);
            newSlots.transform.position = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>().WorldToScreenPoint(new Vector3(0f, -3f));
            newSlots.GetComponent<RectTransform>().sizeDelta = new Vector2((128f * _player.Consumables.Count) + 20f, 128f);

            foreach(Consumable consumable in _player.Consumables)
            {
                GameObject newPotion = Instantiate(_potionButtons) as GameObject;
                newPotion.transform.SetParent(newSlots.transform, false);
                newPotion.GetComponent<BattlePotionButton>()._consumable = consumable;
                newPotion.GetComponent<Image>().sprite = GameObject.FindGameObjectWithTag("Persistent").GetComponent<ItemDatabase>().GetItemDataByID(consumable.ID).Icon;
            }
            GameObject.FindGameObjectWithTag("Potions").SetActive(false);
        }
    }

    public void TakeConsumable(Consumable consumable)
    {
        if(consumable.Type == ConsumableType.Heal)
        {
            _player.Health += consumable.Strength;
            if (_player.Health > _player.MaxHealth)
            {
                _player.Health = _player.MaxHealth;
            }
        }
        else
        {
            _player.AddBoost(consumable);
        }
    }
}
