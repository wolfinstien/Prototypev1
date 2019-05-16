using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class ChangeScene : MonoBehaviour
{
    
	void Start ()
    {
	
	}
	
	void Update ()
    {
	
	}

    public void OnClick(int i)
    {
        SceneManager.LoadScene(i);
    }
}
