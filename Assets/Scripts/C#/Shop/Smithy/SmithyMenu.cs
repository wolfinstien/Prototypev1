using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class SmithyMenu : MonoBehaviour
{
    public Transform _smithyList;
    public Transform _combineList;
    public GameObject _recipeButton;
    public GameObject _ingredientPanel;
    public Text _currency;

    private SmityScript _smithy;
    private PersistentScript _persistent;
    private List<Recipe> _recipeList;
    private int _selectedID;
    private ItemDatabase _databse;

	void Start ()
    {
        _smithy = GameObject.FindGameObjectWithTag("Smithy").GetComponent<SmityScript>();
        _persistent = GameObject.FindGameObjectWithTag("Persistent").GetComponent<PersistentScript>();
        _databse = GameObject.FindGameObjectWithTag("Persistent").GetComponent<ItemDatabase>();
        LoadInSmithyItems();
	}
	
	void Update ()
    {
	
	}

    void LoadInSmithyItems()
    {
        _recipeList = _smithy.GetRecipes();
        _smithyList.GetComponent<RectTransform>().sizeDelta = new Vector2(400f, 200f * _recipeList.Count);
        int count = 0;

        foreach (Recipe recipe in _recipeList)
        {
            GameObject newRecipe = Instantiate(_recipeButton) as GameObject;
            Item newItem = _databse.GetItemDataByID(recipe._id);
            newRecipe.transform.SetParent(_smithyList);
            newRecipe.transform.localScale = Vector3.one;
            newRecipe.transform.GetChild(0).GetComponent<Text>().text = newItem.Name;
            newRecipe.transform.GetChild(1).GetComponent<Image>().sprite = newItem.Icon;
            newRecipe.transform.GetChild(2).GetComponent<Text>().text = "x " + recipe._cost;
            newRecipe.GetComponent<SmithyButton>()._id = count++;
        }
    }

    public void LoadInRecipeRequirements(int index)
    {
        if(GameObject.FindGameObjectWithTag("Deleteable"))
        {
            foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Deleteable"))
            {
                GameObject.Destroy(obj);
            }
        }
        List<Ingredient> ingredients = _recipeList[index]._ingredients;
        _combineList.GetComponent<RectTransform>().sizeDelta = new Vector2(280f, 200f * ingredients.Count);
        _selectedID = index;

        foreach (Ingredient ingredient in ingredients)
        {
            GameObject newIngredient = Instantiate(_ingredientPanel) as GameObject;
            Item newItem = _databse.GetItemDataByID(ingredient._id);
            newIngredient.transform.SetParent(_combineList);
            newIngredient.transform.localScale = Vector3.one;
            newIngredient.transform.GetChild(0).GetComponent<Text>().text = newItem.Name;
            newIngredient.transform.GetChild(1).GetComponent<Image>().sprite = newItem.Icon;
            newIngredient.transform.GetChild(2).GetComponent<Text>().text = _persistent.CheckItemQuantity(ingredient._id) + "\n___\n" + ingredient._quantity;
        }

        _currency.text = "x " + GameObject.FindGameObjectWithTag("Persistent").GetComponent<PersistentScript>()._heat;
    }

    public void BuildItem()
    {
        if(CheckComplete(_selectedID) && _persistent._heat >= _recipeList[_selectedID]._cost)
        {
            CreateItem(_recipeList[_selectedID]._ingredients, _recipeList[_selectedID]._id);
            _persistent._heat -= _recipeList[_selectedID]._cost;
            _smithy.OpenCloseSmithy();
        }
        else
        {
            this.gameObject.transform.GetChild(1).GetChild(1).GetComponent<Image>().color = Color.red;
        }
    }

    bool CheckComplete(int index)
    {
        List<Ingredient> ingredients = _recipeList[index]._ingredients;

        foreach(Ingredient ingredient in ingredients)
        {
            if(_persistent.CheckItemQuantity(ingredient._id) < ingredient._quantity)
            {
                return false;
            }
        }

        return true;
    }

    void CreateItem(List<Ingredient> ingredients, string itemID)
    {
        foreach(Ingredient ingredient in ingredients)
        {
            _persistent.DecreaseItemCount(ingredient._id, ingredient._quantity);
        }

        _persistent.AddInvenotryItem(itemID, 1);
    }
}
