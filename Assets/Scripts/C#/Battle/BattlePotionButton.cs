using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BattlePotionButton : MonoBehaviour
{
    public Sprite _blank;
    public Consumable _consumable;
    public bool _usable = true;

    public PersistentScript _persistent;

	void Start ()
    {
        _persistent = GameObject.FindGameObjectWithTag("Persistent").GetComponent<PersistentScript>();
	}
	
	void Update ()
    {
	
	}

    public void OnClick()
    {
        if (_usable)
        {
            _usable = false;
            GameObject.FindGameObjectWithTag("Player").transform.GetChild(0).GetComponent<PlayerScript>().TakeConsumable(_consumable);
            _persistent._player.Consumables.Remove(_consumable);
            this.gameObject.GetComponent<Image>().sprite = _blank;
        }
    }
}
