using UnityEngine;
using System.Collections;

public class ShopButton : MonoBehaviour
{
    public string _id { get; set; }
    public GameObject _buyPanel;

    private Camera _cam;
    private ItemDatabase _database;

	void Start ()
    {
        _cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        _database = GameObject.FindGameObjectWithTag("Persistent").GetComponent<ItemDatabase>();
	}
	
	void Update ()
    {
	
	}

    public void OnClick()
    {
        if (!GameObject.FindGameObjectWithTag("BuyPanel"))
        {
            GameObject newPanel = Instantiate(_buyPanel) as GameObject;
            newPanel.GetComponent<ShopBuying>()._item = _database.GetItemDataByID(_id);
            newPanel.transform.SetParent(GameObject.FindGameObjectWithTag("UIShop").transform);
            newPanel.transform.localScale = Vector3.one;
            newPanel.transform.position = _cam.WorldToScreenPoint(Vector3.zero);
        }
    }
}
