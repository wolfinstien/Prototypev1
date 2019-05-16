using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

//Inventory Item Data
#region
public class ItemData
{
    [XmlElement("ID")]
    public string _id;
    [XmlAttribute("name")]
    public string _name;
    [XmlElement("Rarity")]
    public int _rarity;
    [XmlElement("Type")]
    public ItemType _type;
    [XmlElement("Cost")]
    public int _cost;
    [XmlElement("Icon")]
    public string _iconName;
    [XmlElement("Count")]
    public int _count;
}

[System.Serializable]
public class Item
{
    public string ID;// { get; set; }
    public string Name;// { get; set; }
    public int Rarity;// { get; set; }
    public ItemType Type;// { get; set; }
    public int Cost;// { get; set; }
    public Sprite Icon;// { get; set; }
    public int Count;// { get; set; }
}

[XmlRoot("List")]
public class ItemContainer
{
    [XmlArray("Items"), XmlArrayItem("Item")]
    public List<ItemData> _itemData = new List<ItemData>();

    public static ItemContainer LoadFromText(string text)
    {
        var serializer = new XmlSerializer(typeof(ItemContainer));
        return serializer.Deserialize(new StringReader(text)) as ItemContainer;
    }
}
#endregion
//Equipment Data
#region
public enum EquipmentType
{
    Weapon,
    Armor,
    Accessory,
    Boots
}
[System.Serializable]
public class Equipment
{
    [XmlAttribute("name")]
    public string Name { get; set; }
    [XmlElement("ID")]
    public string ID { get; set; }
    [XmlElement("Type")]
    public EquipmentType Type { get; set; }
    [XmlElement("Health")]
    public int Health;
    [XmlElement("Strength")]
    public int Strength;
    [XmlElement("Magic")]
    public int Magic;
    [XmlElement("Agility")]
    public int Agility;
}

[XmlRoot("List")]
public class EquipmentContainer
{
    [XmlArray("Item"), XmlArrayItem("Items")]
    public List<Equipment> _data = new List<Equipment>();

    //Loads the xml directly from the given string. Useful in combination with www.text.
    public static EquipmentContainer LoadFromText(string text)
    {
        var serializer = new XmlSerializer(typeof(EquipmentContainer));
        return serializer.Deserialize(new StringReader(text)) as EquipmentContainer;
    }
}
#endregion
//Consumable Data
#region
public enum ConsumableType
{
    Heal,
    Strength,
    Magic,
    Agility
}

[System.Serializable]
public class Consumable
{
    [XmlAttribute("name")]
    public string Name;
    [XmlElement("ID")]
    public string ID;
    [XmlElement("Type")]
    public ConsumableType Type;
    [XmlElement("Strength")]
    public int Strength; //Amount healed or % increase
    [XmlElement("Duration")]
    public int Duration; //tme in seconds, if 0 then doesn't wear off
}

[XmlRoot("List")]
public class ConsumableContainer
{
    [XmlArray("Consumables"), XmlArrayItem("Consumable")]
    public List<Consumable> _data = new List<Consumable>();

    //Loads the xml directly from the given string. Useful in combination with www.text.
    public static ConsumableContainer LoadFromText(string text)
    {
        var serializer = new XmlSerializer(typeof(ConsumableContainer));
        return serializer.Deserialize(new StringReader(text)) as ConsumableContainer;
    }
}
#endregion
//Ability Data
#region
[XmlRoot("List")]
public class AbilityContainer
{
    [XmlArray("Abilities"), XmlArrayItem("Ability")]
    public List<Ability> _data = new List<Ability>();

    //Loads the xml directly from the given string. Useful in combination with www.text.
    public static AbilityContainer LoadFromText(string text)
    {
        var serializer = new XmlSerializer(typeof(AbilityContainer));
        return serializer.Deserialize(new StringReader(text)) as AbilityContainer;
    }
}
#endregion
//Enemy Data
#region
[System.Serializable]
public class Enemy
{
    [XmlAttribute("name")]
    public string Name;
    [XmlElement("Health")]
    public int Health;
    [XmlElement("Strength")]
    public int Strength;
    [XmlElement("Magic")]
    public int Magic;
    [XmlElement("Agility")]
    public int Agility;
    [XmlElement("Common")]
    public string CommonID;
    [XmlElement("Rare")]
    public string RareID;
    [XmlElement("Gold")]
    public int Gold { get; set; }
    [XmlElement("Heat")]
    public int Heat { get; set; }
    [XmlElement("Essence")]
    public int Essence { get; set; }
    [XmlElement("Prefab")]
    public string PrefabName;
}

[XmlRoot("List")]
public class EnemyContainer
{
    [XmlArray("Enemies"), XmlArrayItem("Enemy")]
    public List<Enemy> _data = new List<Enemy>();

    //Loads the xml directly from the given string. Useful in combination with www.text.
    public static EnemyContainer LoadFromText(string text)
    {
        var serializer = new XmlSerializer(typeof(EnemyContainer));
        return serializer.Deserialize(new StringReader(text)) as EnemyContainer;
    }
}
#endregion

public class ItemDatabase : MonoBehaviour
{
    public TextAsset _itemFile;
    public TextAsset _consumableFile;
    public TextAsset _equipmentFile;
    public TextAsset _abilityFile;
    public TextAsset _enemyFile;

    public List<Item> _itemData;
    public List<Consumable> _consumableData;
    public List<Equipment> _equipmentData;
    public List<Ability> _abilityData;
    public List<Enemy> _enemyData;

	void Start ()
    {
        LoadItems();
        LoadEquipment();
        LoadConsumables();
        LoadAbilities();
        LoadEnemies();
	}
	
	void Update ()
    {
	
	}

    void LoadItems()
    {
        TextAsset textAsset = (TextAsset)Resources.Load(_itemFile.name);
        List<ItemData> items = ItemContainer.LoadFromText(textAsset.text)._itemData;

        foreach(ItemData item in items)
        {
            Item data = new Item();
            data.ID = item._id;
            data.Name = item._name;
            data.Rarity = item._rarity;
            data.Type = item._type;
            data.Cost = item._cost;
            data.Icon = Resources.Load<Sprite>("Icons/" + item._iconName);
            data.Count = item._count;
            _itemData.Add(data);
        }
    }

    public Item GetItemDataByName(string name)
    {
        foreach(Item data in _itemData)
        {
            if(data.Name == name)
            {
                return data;
            }
        }

        return null;
    }

    public Item GetItemDataByID(string id)
    {
        foreach (Item data in _itemData)
        {
            if (data.ID == id)
            {
                return data;
            }
        }

        return null;
    }

    void LoadConsumables()
    {
        TextAsset textAsset = (TextAsset)Resources.Load(_consumableFile.name);
        _consumableData = ConsumableContainer.LoadFromText(textAsset.text)._data;
    }

    public Consumable GetConsumable(string id)
    {
        foreach(Consumable consumable in _consumableData)
        {
            if(consumable.ID == id)
            {
                return consumable;
            }
        }
        return null;
    }

    void LoadEquipment()
    {
        TextAsset textAsset = (TextAsset)Resources.Load(_equipmentFile.name);
        _equipmentData = EquipmentContainer.LoadFromText(textAsset.text)._data;
    }

    public Equipment GetEquipment(string id)
    {
        foreach(Equipment equipment in _equipmentData)
        {
            if(equipment.ID == id)
            {
                return equipment;
            }
        }
        return null;
    }

    void LoadAbilities()
    {
        TextAsset textAsset = (TextAsset)Resources.Load(_abilityFile.name);
        _abilityData = AbilityContainer.LoadFromText(textAsset.text)._data;
    }

    void LoadEnemies()
    {
        TextAsset textAsset = (TextAsset)Resources.Load(_enemyFile.name);
        _enemyData = EnemyContainer.LoadFromText(textAsset.text)._data;
    }
}
