using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

#region
public class Ingredient
{
    [XmlAttribute("id")]
    public string _id;
    [XmlElement("Quantity")]
    public int _quantity;
}

public class Recipe
{
    [XmlAttribute("id")]
    public string _id;
    [XmlArray("Recipe"), XmlArrayItem("Ingredient")]
    public List<Ingredient> _ingredients = new List<Ingredient>();
    [XmlElement("Cost")]
    public int _cost;
}

[XmlRoot("Container")]
public class SmithyList
{
    [XmlArray("List"), XmlArrayItem("Recipes")]
    public List<Recipe> _recipeList = new List<Recipe>();
    
    //Loads the xml directly from the given string. Useful in combination with www.text.
    public static SmithyList LoadFromText(string text)
    {
        var serializer = new XmlSerializer(typeof(SmithyList));
        return serializer.Deserialize(new StringReader(text)) as SmithyList;
    }
}
#endregion

public class SmityScript : MonoBehaviour
{
    public TextAsset _smithyFile;
    public GameObject _smithyObject;
    public bool _open;

    private List<Recipe> _smithyList = new List<Recipe>();
    private Camera _cam;

	void Start ()
    {
        _cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        LoadSmithyRecipes();
	}
	
	void Update ()
    {
	
	}

    public void LoadSmithyRecipes()
    {
        if(_smithyFile != null)
        {
            TextAsset textAsset = (TextAsset)Resources.Load(_smithyFile.name);
            _smithyList = SmithyList.LoadFromText(textAsset.text)._recipeList;
        }
    }

    public void OpenCloseSmithy()
    {
        if(_open)
        {
            _open = false;
            GameObject.Destroy(GameObject.FindGameObjectWithTag("SmithyUI"));
        }
        else
        {
            _open = true;
            GameObject newSmithy = Instantiate(_smithyObject) as GameObject;
            newSmithy.transform.SetParent(GameObject.FindGameObjectWithTag("UI").transform);
            newSmithy.transform.position = _cam.WorldToScreenPoint(Vector3.zero);
            newSmithy.transform.localScale = Vector3.one;
        }
    }

    public List<Recipe> GetRecipes()
    {
        return _smithyList;
    }
}
