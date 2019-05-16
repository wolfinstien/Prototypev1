using UnityEngine;
using System.Collections;

public class PlayerBattleAnimation : MonoBehaviour
{
    public Animator _animator;
    public Collider2D _collider;
    public int _maxFrames;

    private bool _start;
    private int _frames;
    private Vector2 _startPos;

	void Start ()
    {
	
	}
	
	void Update ()
    {
        if (!_animator.GetCurrentAnimatorStateInfo(0).IsName("Hurt"))
        {
            if (Application.platform == RuntimePlatform.Android)
            {
                if (Input.touchCount > 0)
                {
                    TouchControls(Input.GetTouch(0).position);
                }
            }
            else
            {
                if (Input.GetMouseButton(0))
                {
                    TouchControls(Input.mousePosition);
                }
            }
        }
	}

    void TouchControls(Vector2 vec)
    {
        if (!_start)
        {
            if (RayHitThisCollider(vec))
            {
                _startPos = Input.mousePosition;
                _start = true;
            }
        }
        else
        {
            if (RunThroughFrames())
            {
                CalculateDifference(vec);
            }
        }
    }

    void CalculateDifference(Vector2 vec)
    {
        Vector2 difference = _startPos - vec;
        AnimatorStateInfo _state = _animator.GetCurrentAnimatorStateInfo(0);

        if (_state.IsName("Idle"))
        {
            switch (CheckDirection(difference))
            {
                case 0:
                    _animator.Play("Counter");
                    break;
                case 1:
                    _animator.Play("BackStep");
                    break;
                case 2:
                    _animator.Play("Jump");
                    break;
                case 3:
                    _animator.Play("Block");
                    break;
            }
        }

        _start = false;
        _startPos = Vector2.zero;
        _frames = 0;
    }

    int CheckDirection(Vector2 vec)
    {
        if(Mathf.Abs(vec.x) > Mathf.Abs(vec.y))
        {
            if(vec.x > 0)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }
        else
        {
            if(vec.y > 0)
            {
                return 3;
            }
            else
            {
                return 2;
            }
        }
    }

    bool RunThroughFrames()
    {
        if (_frames < _maxFrames)
        {
            _frames++;
        }
        else
        {
            return true;
        }

        return false;
    }

    bool RayHitThisCollider(Vector2 vec)
    {
        RaycastHit2D hit = Physics2D.Raycast(GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>().ScreenToWorldPoint(vec), Vector3.forward);

        if (hit.collider != null)
        {
            if (hit.collider == _collider)
            {
                return true;
            }
        }
        return false;
    }
}
