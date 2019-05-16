using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

[System.Serializable]
public class InventoryItem
{
    [XmlAttribute("id")]
    public string _id;
    [XmlElement("Count")]
    public int _count;
}

[XmlRoot("Inventory")]
public class InventoryContainer
{
    [XmlArray("Items"), XmlArrayItem("Item")]
    public List<InventoryItem> _inventoryItems = new List<InventoryItem>();

    public void Save(string path)
    {
        var serializer = new XmlSerializer(typeof(InventoryContainer));
        using (var stream = new FileStream(path, FileMode.Create))
        {
            serializer.Serialize(stream, this);
        }
    }

    public static InventoryContainer Load(string path)
    {
        var serializer = new XmlSerializer(typeof(InventoryContainer));
        using (var stream = new FileStream(path, FileMode.Open))
        {
            return serializer.Deserialize(stream) as InventoryContainer;
        }
    }

    //Loads the xml directly from the given string. Useful in combination with www.text.
    public static InventoryContainer LoadFromText(string text)
    {
        var serializer = new XmlSerializer(typeof(InventoryContainer));
        return serializer.Deserialize(new StringReader(text)) as InventoryContainer;
    }
}

public class PersistentScript : MonoBehaviour
{
    public ChangeScene _sceneChange;
    public GameObject _inventoryObject;
    public GameObject _inventoryButton;
    public GameObject _openInventoryButton;
    public TextAsset _inventoryFile;
    public List<InventoryItem> _inventory;
    public bool _openInventory = false;
    public PlayerData _player;
    public int _gold;// { get; set; }
    public int _heat;
    public int _magic;
    public GameObject _battleObj;

    private ItemDatabase _database;
    private Camera _cam;
    private Transform _inventoryContent;
    private InventoryScript _inventoryScript;

    void Start ()
    {
        if(GameObject.FindGameObjectsWithTag("Persistent").Length > 1)
        {
            GameObject.Destroy(this.gameObject);
        }
        else
        {
            DontDestroyOnLoad(this.gameObject);
        }
        _database = GameObject.FindGameObjectWithTag("Persistent").GetComponent<ItemDatabase>();
        UpdateCamera();
        LoadFromFile();
	}

	void Update ()
    {

	}

    void UpdateCamera()
    {
        _cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }

    public void LoadInventoryButton()
    {
        GameObject newButton = Instantiate(_openInventoryButton);
        newButton.transform.SetParent(GameObject.FindGameObjectWithTag("UI").transform, false);
        UpdateCamera();
        newButton.transform.position = _cam.WorldToScreenPoint(new Vector3(-6f, -4.5f));
    }

    public void OpenInventory()
    {
        if(_openInventory)
        {
            _openInventory = false;
            if (_inventoryContent != null)
            {
                GameObject.Destroy(_inventoryContent.parent.parent.gameObject);
            }
        }
        else
        {
            _openInventory = true;
            LoadInventory();
        }
    }

    public void LoadFromFile()
    {
        string filename = Application.persistentDataPath + "/" + _inventoryFile.name + ".xml";
        if (File.Exists(filename))
        {
            TextAsset textAsset = (TextAsset)Resources.Load(_inventoryFile.name);
            _inventory = InventoryContainer.LoadFromText(textAsset.text)._inventoryItems;
        }
        else
        {
            print("no file to load from");
        }
    }

    public void LoadInventory()
    {
        GameObject inventory = Instantiate(_inventoryObject) as GameObject;
        inventory.transform.SetParent(GameObject.FindGameObjectWithTag("UI").transform);
        inventory.transform.localScale = Vector3.one;
        inventory.transform.position = _cam.WorldToScreenPoint(Vector3.zero);

        List<Item> consumables = new List<Item>();
        if(_player.Consumables.Count > 0)
        {
            foreach(Consumable cons in _player.Consumables)
            {
                consumables.Add(_database.GetItemDataByID(cons.ID));
            }
        }

        inventory.GetComponent<InventoryScript>().AddEquipment(_database.GetItemDataByID(_player.Weapon.ID), _database.GetItemDataByID(_player.Armor.ID), consumables);
        _inventoryContent = inventory.transform.GetChild(0).GetChild(0);

        int count = 0;

        foreach(InventoryItem item in _inventory)
        {
            if (item._count > 0)
            {
                GameObject newButton = Instantiate(_inventoryButton) as GameObject;
                newButton.transform.SetParent(_inventoryContent);
                newButton.transform.localScale = Vector3.one;
                Item newItem = _database.GetItemDataByID(item._id);
                newButton.transform.GetChild(0).GetComponent<Text>().text = newItem.Name;
                newButton.transform.GetChild(1).GetComponent<Image>().sprite = newItem.Icon;
                newButton.transform.GetChild(2).GetComponent<Text>().text = "x " + item._count;
                newButton.GetComponent<InventoryEquip>().SetButtonValue(this, item);
                count++;
            }
        }

        _inventoryContent.GetComponent<RectTransform>().sizeDelta = new Vector2(400f, 200f * count);
        _inventoryScript = inventory.GetComponent<InventoryScript>();
    }

    public void SaveInventoryItems()
    {
        string filename = Application.persistentDataPath + "/" + _inventoryFile.name + ".xml";
        print(filename);
        InventoryContainer inventory = new InventoryContainer();
        inventory._inventoryItems = _inventory;
        inventory.Save(filename);
    }

    public void AddInvenotryItem(string id, int count)
    {
        foreach(InventoryItem item in _inventory)
        {
            if(item._id == id)
            {
                item._count += count;
                return;
            }
        }

        InventoryItem newItem = new InventoryItem();
        newItem._id = id;
        newItem._count = count;
        _inventory.Add(newItem);
    }

    public void IncreaseItemCount(string id, int count)
    {
        foreach(InventoryItem item in _inventory)
        {
            if(item._id == id)
            {
                item._count += count;
            }
        }

        AddInvenotryItem(id, count);
    }

    public void DecreaseItemCount(string id, int count)
    {
        foreach(InventoryItem item in _inventory)
        {
            if(item._id == id)
            {
                item._count -= count;
                if(item._count <= 0)
                {
                    _inventory.Remove(item);
                }
                return;
            }
        }
    }

    public int CheckItemQuantity(string id)
    {
        int quantity = 0;

        foreach(InventoryItem item in _inventory)
        {
            if(item._id == id)
            {
                return item._count;
            }
        }

        return quantity;
    }

    public void EquipItem(string id)
    {
        Item item = _database.GetItemDataByID(id);

        if (CheckItemQuantity(id) > 0)
        {
            switch (item.Type)
            {
                case ItemType.Consumable:
                    if (_player.Consumables.Count < 4)
                    {
                        _inventoryScript.AddConsumable(item);
                        _player.Consumables.Add(_database.GetConsumable(item.ID));
                        DecreaseItemCount(id, 1);
                    }
                    break;
                case ItemType.Equipment:
                    _inventoryScript.AddArmor(item);
                    _player.Armor = _database.GetEquipment(id);
                    DecreaseItemCount(id, 1);
                    break;
                case ItemType.Weapon:
                    _inventoryScript.AddWeapon(item);
                    _player.Weapon = _database.GetEquipment(id);
                    DecreaseItemCount(id, 1);
                    break;
            }
        }
    }

    public void AddItemBackToInventory(string id)
    {
        InventoryItem item = new InventoryItem();
        item._id = id;
        item._count = CheckItemQuantity(id);
        Item newItem = _database.GetItemDataByID(id);

        GameObject newButton = Instantiate(_inventoryButton) as GameObject;
        newButton.transform.SetParent(_inventoryContent, false);
        newButton.transform.GetChild(0).GetComponent<Text>().text = newItem.Name;
        newButton.transform.GetChild(1).GetComponent<Image>().sprite = newItem.Icon;
        newButton.transform.GetChild(2).GetComponent<Text>().text = "x " + item._count;
        newButton.GetComponent<InventoryEquip>().SetButtonValue(this, item);

        _inventoryContent.GetComponent<RectTransform>().sizeDelta = new Vector2(400f, 200f * _inventoryContent.childCount);
    }

    public void LoadFight()
    {
        _sceneChange.OnClick(1);
        StartCoroutine(LoadFightInfo());
    }

    IEnumerator LoadFightInfo()
    {
        yield return new WaitForSeconds(0.01f);

        _player.Abilities = _database._abilityData;
        Enemy enemy = _database._enemyData[Random.Range(0, _database._enemyData.Count)];
        PlayerData player = new PlayerData(_player.Health, _player.Strength, _player.Magic, _player.Agility, _player.Weapon, _player.Armor);
        player.Consumables = new List<Consumable>();
        player.Abilities = new List<Ability>();

        if (_player.Consumables.Count > 0)
        {
            player.Consumables = _player.Consumables;
        }

        if(_player.Abilities.Count > 0)
        {
            player.Abilities = _player.Abilities;
        }

        GameObject newBattle = Instantiate(_battleObj) as GameObject;
        newBattle.GetComponent<FightSceneScript>().SetupFight(_player, enemy);
    }

    public void EntityDeath(Entity entity)
    {

    }
}
