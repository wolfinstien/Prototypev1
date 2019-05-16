using UnityEngine;
using System.Collections;

public class FindPersistentScript : MonoBehaviour
{
    private PersistentScript _persistent;

	void Start ()
    {
        _persistent = GameObject.FindGameObjectWithTag("Persistent").GetComponent<PersistentScript>();
	}
	
	void Update ()
    {
	
	}

    public void OpenInventory()
    {
        _persistent.OpenInventory();
    }

    public void LoadFight()
    {
        _persistent.LoadFight();
    }

    public void Open(string name)
    {
        if(_persistent._openInventory)
        {
            _persistent.OpenInventory();
        }

        if(GameObject.FindGameObjectWithTag("Shop").GetComponent<ShopScript>()._open)
        {
            GameObject.FindGameObjectWithTag("Shop").GetComponent<ShopScript>().OpenCloseShop();
        }

        if (GameObject.FindGameObjectWithTag("Potion").GetComponent<PotionShopScript>()._open)
        {
            GameObject.FindGameObjectWithTag("Potion").GetComponent<PotionShopScript>().OpenClosePotionShop();
        }

        if (GameObject.FindGameObjectWithTag("Smithy").GetComponent<SmityScript>()._open)
        {
            GameObject.FindGameObjectWithTag("Smithy").GetComponent<SmityScript>().OpenCloseSmithy();
        }

        switch(name)
        {
            case "Inventory":
                _persistent.OpenInventory();
                break;
            case "Shop":
                GameObject.FindGameObjectWithTag("Shop").GetComponent<ShopScript>().OpenCloseShop();
                break;
            case "Potion":
                GameObject.FindGameObjectWithTag("Potion").GetComponent<PotionShopScript>().OpenClosePotionShop();
                break;
            case "Smithy":
                GameObject.FindGameObjectWithTag("Smithy").GetComponent<SmityScript>().OpenCloseSmithy();
                break;
        }
    }
}
