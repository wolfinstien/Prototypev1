using UnityEngine;
using System.Collections;

public class AnimationController : MonoBehaviour
{
    public GameObject _toDestroy;

	void Start ()
    {
	
	}
	
	void Update ()
    {
	
	}

    public void Destroy()
    {
        GameObject.Destroy(_toDestroy);
    }
}
