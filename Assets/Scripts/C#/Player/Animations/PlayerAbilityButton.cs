using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Xml;
using System.Xml.Serialization;

public enum AbilityType
{
    Damage,
    Heal,
    Buff
}

[System.Serializable]
public class Ability
{
    [XmlAttribute("name")]
    public string Name;
    [XmlElement("Type")]
    public AbilityType Type;
    [XmlElement("Damage")]
    public DamageType Damage;
    [XmlElement("Cooldown")]
    public float Cooldown;
    [XmlElement("Delay")]
    public float Delay;
    [XmlElement("Base")]
    public float Base;
    [XmlElement("Scale")]
    public float Scale;
    [XmlElement("Icon")]
    public string IconName;
    [XmlElement("State")]
    public string StateName;
}

public class PlayerAbilityButton : MonoBehaviour
{
    public Ability _ability;

    private float _timer;
    private bool _active = true;
    private RectTransform _child;
    private PlayerScript _player;
    private Animator _anim;

	void Start ()
    {
        _child = this.gameObject.transform.GetChild(0).GetComponent<RectTransform>();
        _player = GameObject.FindGameObjectWithTag("Player").transform.GetChild(0).GetComponent<PlayerScript>();
        _anim = GameObject.FindGameObjectWithTag("Player").transform.GetChild(0).GetComponent<Animator>();
        this.gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("AbilityIcons/" + _ability.IconName);
    }
	
	void Update ()
    {
        if (!_active)
        {
            if (_timer < _ability.Cooldown)
            {
                float ratio;

                _timer += Time.deltaTime;

                ratio = _timer / _ability.Cooldown;

                _child.sizeDelta = new Vector2(128f, 128f * (1f - ratio));
            }
            else
            {
                _timer = 0f;
                _active = true;
            }
        }
	}

    public void PlayAbility()
    {
        if(_active)
        {
            AnimatorStateInfo _state = _anim.GetCurrentAnimatorStateInfo(0);
            if (_state.IsName("Idle"))
            {
                _active = false;
                if(_anim != null)
                {
                    _anim.Play(_ability.StateName);
                    _player.Attack(_ability);
                }
            }
        }
    }
}
