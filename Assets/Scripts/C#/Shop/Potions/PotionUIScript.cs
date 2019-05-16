using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class PotionUIScript : MonoBehaviour
{
    public Toggle _ingredientToggle;
    public Toggle _essenceToggle;
    public Transform _potionContent;
    public Transform _ingredientContent;
    public Text _cost;
    public Text _currency;
    public GameObject _potionButton;
    public GameObject _ingredientButton;

    private PersistentScript _persistent;
    private PotionShopScript _potionShop;
    private ItemDatabase _databse;
    private int _selectedID;

    void Start ()
    {
        _persistent = GameObject.FindGameObjectWithTag("Persistent").GetComponent<PersistentScript>();
        _potionShop = GameObject.FindGameObjectWithTag("Potion").GetComponent<PotionShopScript>();
        _databse = GameObject.FindGameObjectWithTag("Persistent").GetComponent<ItemDatabase>();
        LoadPotions();
	}
	
	void Update ()
    {

    }

    void LoadPotions()
    {
        int count = 0;
        _potionContent.GetComponent<RectTransform>().sizeDelta = new Vector2(400f, 200f * _potionShop.GetRecipes().Count);

        foreach(Recipe recipe in _potionShop.GetRecipes())
        {
            GameObject newPotion = Instantiate(_potionButton);
            Item newItem = _databse.GetItemDataByID(recipe._id);
            newPotion.transform.SetParent(_potionContent);
            newPotion.transform.localScale = Vector3.one;
            newPotion.transform.GetChild(0).GetComponent<Image>().sprite = newItem.Icon;
            newPotion.transform.GetChild(1).GetComponent<Text>().text = newItem.Name;
            newPotion.GetComponent<PotionButton>()._id = count++;
        }
    }

    public void LoadIngredients(int index)
    {
        if (GameObject.FindGameObjectWithTag("Deleteable"))
        {
            foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Deleteable"))
            {
                GameObject.Destroy(obj);
            }
        }
        
        List<Ingredient> ingredients = _potionShop.GetRecipes()[index]._ingredients;
        _ingredientContent.GetComponent<RectTransform>().sizeDelta = new Vector2(260f, 140f * ingredients.Count);
        _selectedID = index;

        foreach (Ingredient ingredient in ingredients)
        {
            GameObject newIngredient = Instantiate(_ingredientButton) as GameObject;
            Item newItem = _databse.GetItemDataByID(ingredient._id);
            newIngredient.transform.SetParent(_ingredientContent);
            newIngredient.transform.localScale = Vector3.one;
            newIngredient.transform.GetChild(0).GetComponent<Image>().sprite = newItem.Icon;
            newIngredient.transform.GetChild(1).GetComponent<Text>().text = _persistent.CheckItemQuantity(ingredient._id) + "\n___\n" + ingredient._quantity;
        }

        _cost.text = "x " + _potionShop.GetRecipes()[index]._cost;
        _currency.text = "/ " + GameObject.FindGameObjectWithTag("Persistent").GetComponent<PersistentScript>()._magic;
    }

    public void BrewPotion()
    {
        if (_ingredientToggle.isOn)
        {
            if(CheckComplete(_selectedID))
            {
                foreach(Ingredient ingredient in _potionShop.GetRecipes()[_selectedID]._ingredients)
                {
                    _persistent.DecreaseItemCount(ingredient._id, ingredient._quantity);
                }
            }
            else
            {
                return;
            }
        }
        else if (_essenceToggle.isOn)
        {
            if(_persistent._magic < _potionShop.GetRecipes()[_selectedID]._cost)
            {
                return;
            }
            else
            {
                _persistent._magic -= _potionShop.GetRecipes()[_selectedID]._cost;
            }
        }
        _persistent.AddInvenotryItem(_potionShop.GetRecipes()[_selectedID]._id, 1);
        _potionShop.OpenClosePotionShop();
    }

    bool CheckComplete(int index)
    {
        List<Ingredient> ingredients = _potionShop.GetRecipes()[index]._ingredients;

        foreach (Ingredient ingredient in ingredients)
        {
            if (_persistent.CheckItemQuantity(ingredient._id) < ingredient._quantity)
            {
                return false;
            }
        }

        return true;
    }
}
