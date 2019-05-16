using UnityEngine;
using System.Collections;

public class HitBox : MonoBehaviour
{
    public int _duration;
    public Vector2 _offset;

    private int _count;

	void Start ()
    {
	
	}
	
	void Update ()
    {
	    if(_count < _duration)
        {
            _count++;
        }
	}
}
