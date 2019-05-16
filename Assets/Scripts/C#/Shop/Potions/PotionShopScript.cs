using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

#region
[XmlRoot("Container")]
public class PotionContainer
{
    [XmlArray("List"), XmlArrayItem("Recipes")]
    public List<Recipe> _recipes;

    public static PotionContainer LoadFromText(string text)
    {
        var serializer = new XmlSerializer(typeof(PotionContainer));
        return serializer.Deserialize(new StringReader(text)) as PotionContainer;
    }
}
#endregion

public class PotionShopScript : MonoBehaviour
{
    public TextAsset _potionFile;
    public GameObject _potionShopObject;
    public bool _open;

    private List<Recipe> _potionRecipes = new List<Recipe>();
    private Camera _cam;

	void Start ()
    {
        _cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        LoadPotions();
	}
	
	void Update ()
    {
	
	}

    void LoadPotions()
    {
        if (_potionFile != null)
        {
            TextAsset textAsset = (TextAsset)Resources.Load(_potionFile.name);
            _potionRecipes = PotionContainer.LoadFromText(textAsset.text)._recipes;
        }
    }

    public void OpenClosePotionShop()
    {
        if(_open)
        {
            _open = false;
            GameObject.Destroy(GameObject.FindGameObjectWithTag("PotionUI"));
        }
        else
        {
            _open = true;
            GameObject newShop = Instantiate(_potionShopObject) as GameObject;
            newShop.transform.SetParent(GameObject.FindGameObjectWithTag("UI").transform);
            newShop.transform.localScale = Vector3.one;
            newShop.transform.position = _cam.WorldToScreenPoint(Vector3.zero);
        }
    }

    public List<Recipe> GetRecipes()
    {
        return _potionRecipes;
    }
}
