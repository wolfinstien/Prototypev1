using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DamageResult : MonoBehaviour
{
    public float _time;
    public Text _text;

    private float _count;

    void Start()
    {
    }

	void Update ()
    {
        _count += Time.deltaTime;

        if(_count > _time)
        {
            GameObject.Destroy(this.gameObject);
        }
	}

    public void SetResult(float result, Color colour)
    {
        _text.text = result.ToString("F0");
        _text.color = colour;
    }
}
