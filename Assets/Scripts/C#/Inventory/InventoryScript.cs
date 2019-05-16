using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class InventoryScript : MonoBehaviour
{
    public Image _weaponImage;
    public Image _armorImage;
    public Transform _consumableView;
    public Transform _itemView;
    public GameObject _consumableButton;

    public Item _weapon;
    public Item _armor;
    private List<Item> _consumables = new List<Item>();

	void Start ()
    {
	
	}
	
	void Update ()
    {
	
	}

    public void AddWeapon(Item item)
    {
        if (_weapon.ID != "")
        {
            GameObject.FindGameObjectWithTag("Persistent").GetComponent<PersistentScript>().IncreaseItemCount(_weapon.ID, 1);

            InventoryEquip button = GetButtonWithID(_weapon.ID);
            if(button != null)
            {
                button.UpdateCount(GameObject.FindGameObjectWithTag("Persistent").GetComponent<PersistentScript>().CheckItemQuantity(_weapon.ID));
            }
            else
            {
                GameObject.FindGameObjectWithTag("Persistent").GetComponent<PersistentScript>().AddItemBackToInventory(_weapon.ID);
            }
        }
        _weapon = item;
        UpdateInventory();
    }

    public void AddArmor(Item item)
    {
        if (_armor.ID != "")
        {
            GameObject.FindGameObjectWithTag("Persistent").GetComponent<PersistentScript>().IncreaseItemCount(_armor.ID, 1);

            InventoryEquip button = GetButtonWithID(_armor.ID);
            if (button != null)
            {
                button.UpdateCount(GameObject.FindGameObjectWithTag("Persistent").GetComponent<PersistentScript>().CheckItemQuantity(_armor.ID));
            }
            else
            {
                GameObject.FindGameObjectWithTag("Persistent").GetComponent<PersistentScript>().AddItemBackToInventory(_armor.ID);
            }
        }
        _armor = item;
        UpdateInventory();
    }

    public void AddConsumable(Item item)
    {
        _consumables.Add(item);
        UpdateInventory();
    }

    public void AddEquipment(Item weapon, Item armor, List<Item> consumables)
    {
        if (weapon != null)
        _weapon = weapon;
        if(armor != null)
        _armor = armor;

        if(consumables.Count > 0)
        {
            _consumables = consumables;
        }

        UpdateInventory();
    }

    void UpdateInventory()
    {
        if(_weapon != null)
        {
            _weaponImage.sprite = _weapon.Icon;
        }

        if(_armor != null)
        {
            _armorImage.sprite = _armor.Icon;
        }

        if(_consumables.Count > 0)
        {
            for(int i = _consumableView.transform.childCount; i > 0; i--)
            {
                GameObject.Destroy(_consumableView.transform.GetChild((i-1)).gameObject);
            }

            _consumableView.GetComponent<RectTransform>().sizeDelta = new Vector2(280, 280 * _consumables.Count + 1);

            foreach(Item item in _consumables)
            {
                GameObject newConsumable = Instantiate(_consumableButton) as GameObject;
                newConsumable.transform.SetParent(_consumableView, false);
                newConsumable.transform.GetChild(0).GetComponent<Text>().text = item.Name;
                newConsumable.transform.GetChild(1).GetComponent<Image>().sprite = item.Icon;
            }
        }
    }

    public InventoryEquip GetButtonWithID(string id)
    {
        for(int i = 0; i < this.gameObject.transform.GetChild(0).GetChild(0).childCount; i++)
        {
            if(this.gameObject.transform.GetChild(0).GetChild(0).GetChild(i).GetComponent<InventoryEquip>()._data._id == id)
            {
                return this.gameObject.transform.GetChild(0).GetChild(0).GetChild(i).GetComponent<InventoryEquip>();
            }
        }
        return null;
    }
}
