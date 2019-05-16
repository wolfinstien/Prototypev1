using UnityEngine;
using System.Collections;

public class SwitchActive : MonoBehaviour
{
    public GameObject _slotA;
    public GameObject _slotB;
    public bool _switch = false;

	void Start ()
    {
	
	}
	
	void Update ()
    {

	}

    public void OnClick()
    {
        _switch = !_switch;
        _slotA.SetActive(!_switch);
        _slotB.SetActive(_switch);
    }
}
