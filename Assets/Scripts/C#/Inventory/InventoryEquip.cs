using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class InventoryEquip : MonoBehaviour
{
    private PersistentScript _persistent;
    public InventoryItem _data;
    public Text _text;
    public int _location;

	void Start ()
    {
	
	}
	
	void Update ()
    {
	
	}

    public void SetButtonValue(PersistentScript persistent, InventoryItem data)
    {
        _persistent = persistent;
        _data = data;
    }

    public void OnClick()
    {
        if (_data._count > 0)
        {
            _persistent.EquipItem(_data._id);
            _data._count = GameObject.FindGameObjectWithTag("Persistent").GetComponent<PersistentScript>().CheckItemQuantity(_data._id);
           _text.text = "x " + _data._count;
        }
    }

    public void SetUnequipData(PersistentScript persistent, string id, int location)
    {
        _persistent = persistent;
        _data = new InventoryItem();
        _data._id = id;
        _location = location;
    }

    public void OnUnequip()
    {
        _persistent.IncreaseItemCount(_data._id, 1);
        GameObject.Destroy(this.gameObject);
    }

    public void UpdateCount(int i)
    {
        _data._count = i;
        _text.text = "x " + i;
    }
}
