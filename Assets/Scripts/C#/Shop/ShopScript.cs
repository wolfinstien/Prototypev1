using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

#region
public enum ItemType
{
    Resource,
    Consumable,
    Weapon,
    Equipment
}

[System.Serializable]
public class ShopItem
{
    [XmlAttribute("id")]
    public string _id;
}

[XmlRoot("Shop")]
public class ShopContainer
{
    [XmlArray("Items"), XmlArrayItem("Item")]
    public List<ShopItem> _shopItems = new List<ShopItem>();
    
    public static ShopContainer Load(string path)
    {
        var serializer = new XmlSerializer(typeof(ShopContainer));
        using (var stream = new FileStream(path, FileMode.Open))
        {
            return serializer.Deserialize(stream) as ShopContainer;
        }
    }

    //Loads the xml directly from the given string. Useful in combination with www.text.
    public static ShopContainer LoadFromText(string text)
    {
        var serializer = new XmlSerializer(typeof(ShopContainer));
        return serializer.Deserialize(new StringReader(text)) as ShopContainer;
    }
}
#endregion

public class ShopScript : MonoBehaviour
{
    public GameObject _shopObject;
    public GameObject _button;
    public TextAsset _shopFile;
    public List<ShopItem> _items;
    public bool _open = false;

    private Camera _cam;
    private Transform _content;
    private PersistentScript _persistent;
    private ItemDatabase _database;

    void Start ()
    {
        _cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        _persistent = GameObject.FindGameObjectWithTag("Persistent").GetComponent<PersistentScript>();
        _database = GameObject.FindGameObjectWithTag("Persistent").GetComponent<ItemDatabase>();
        LoadInItems();
        GameObject.FindGameObjectWithTag("Persistent").GetComponent<PersistentScript>().LoadInventoryButton();
	}
	
	void Update ()
    {
	
	}

    public void LoadInItems()
    {
        if(_shopFile != null)
        {
            TextAsset textAsset = (TextAsset)Resources.Load(_shopFile.name);
            _items = ShopContainer.LoadFromText(textAsset.text)._shopItems;
        }
    }

    public void OpenCloseShop()
    {
        if(_open)
        {
            _open = false;
            GameObject.Destroy(_content.parent.gameObject);
        }
        else
        {
            _open = true;
            LoadShopInventory();
        }
    }

    void LoadShopInventory()
    {
        GameObject newShop = Instantiate(_shopObject);
        newShop.transform.SetParent(GameObject.FindGameObjectWithTag("UI").transform);
        newShop.transform.localScale = Vector3.one;
        newShop.transform.position = _cam.WorldToScreenPoint(new Vector3(0f, 4f));
        _content = newShop.transform.GetChild(0);

        _content.GetComponent<RectTransform>().sizeDelta = new Vector2(400f, 200f * _items.Count);

        for(int i = 0; i < _items.Count; i++)
        {
            GameObject newButton = Instantiate(_button) as GameObject;
            Item newItem = _database.GetItemDataByID(_items[i]._id);
            newButton.GetComponent<ShopButton>()._id = newItem.ID;
            newButton.transform.SetParent(_content);
            newButton.transform.localScale = Vector3.one;
            newButton.transform.GetChild(0).GetComponent<Text>().text = newItem.Name;
            newButton.transform.GetChild(1).GetComponent<Image>().sprite = newItem.Icon;
            newButton.transform.GetChild(2).GetComponent<Text>().text = newItem.Cost.ToString();
        }
    }

    public ShopItem GetItemFromShop(int index)
    {
        return _items[index];
    }

    public bool BuyFromShop(string id, int count)
    {
        if(_persistent._gold >= _database.GetItemDataByID(id).Cost * count)
        {
            _persistent.AddInvenotryItem(id, count);
            _persistent._gold -= _database.GetItemDataByID(id).Cost * count;
            return true;
        }
        return false;
    }

    void UpdateCurrency()
    {

    }
}
