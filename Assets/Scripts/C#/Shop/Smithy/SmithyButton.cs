using UnityEngine;
using System.Collections;

public class SmithyButton : MonoBehaviour
{
    public int _id;

    private SmithyMenu _smithyUI;

	void Start ()
    {
        _smithyUI = GameObject.FindGameObjectWithTag("SmithyUI").GetComponent<SmithyMenu>();
	}
	
	void Update ()
    {
	
	}

    public void OnClick()
    {
        _smithyUI.LoadInRecipeRequirements(_id);
    }
}
