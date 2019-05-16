using UnityEngine;
using System.Collections;

public class PotionButton : MonoBehaviour
{
    public int _id { get; set; }

    private PotionUIScript _menu;

	void Start ()
    {
        _menu = GameObject.FindGameObjectWithTag("PotionUI").GetComponent<PotionUIScript>();
	}
	
	void Update ()
    {
	
	}

    public void OnClick()
    {
        _menu.LoadIngredients(_id);
    }
}
