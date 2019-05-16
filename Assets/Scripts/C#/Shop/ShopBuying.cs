using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ShopBuying : MonoBehaviour
{
    public Item _item { get; set; }
    public Text _countText;
    public Text _nameText;
    public Text _currency;

    private ShopScript _shop;
    private int _count;

	void Start ()
    {
        _shop = GameObject.FindGameObjectWithTag("Shop").GetComponent<ShopScript>();
        ChangeCount(1);
        _nameText.text = _item.Name;
        _currency.text = "x " + GameObject.FindGameObjectWithTag("Persistent").GetComponent<PersistentScript>()._gold;
	}
	
	void Update ()
    {

	}

    public void IncreaseCount()
    {
        if (_count < 99)
        {
            ChangeCount(1);
        }
    }

    public void DecreaseCount()
    {
        if (_count > 1)
        {
            ChangeCount(-1);
        }
    }

    void ChangeCount(int i)
    {
        _count += i;
        _countText.text = "" + _count; 
    }

    public void BuyItem()
    {
        if(_shop.BuyFromShop(_item.ID, _count))
        {
            Cancel();
        }
        else
        {
            this.gameObject.transform.GetChild(0).GetComponent<Image>().color = Color.red;
        }
    }

    public void Cancel()
    {
        GameObject.Destroy(this.gameObject);
    }
}
