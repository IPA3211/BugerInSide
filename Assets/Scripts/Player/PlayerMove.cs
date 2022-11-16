using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{

    private Animator        _animator;
    private Rigidbody2D     _rigid;
    public float speed;
    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
        _rigid = GetComponent<Rigidbody2D>();
        transform.position = GameStatics._pos;
    }

    // Update is called once per frame
    void FixedUpdate()
    {   
        float x = 0;
        float y = 0;
        
        if(!PlayerStatics.isCanMove()){
            x = Input.GetAxis("Horizontal");
            y = Input.GetAxis("Vertical");
        }

        bool moving = false;

        if (Mathf.Abs(x) > Mathf.Epsilon || Mathf.Abs(y) > Mathf.Epsilon)
            moving = true;

        else
            moving = false;
    
        Vector2 moveVector = new Vector2(x, y);

        if(moveVector.magnitude > 1){
            moveVector = moveVector.normalized;
        }

        if(moveVector.x > 0){
            GetComponent<SpriteRenderer>().flipX = false;
        }
        else if(moveVector.x < 0){
            GetComponent<SpriteRenderer>().flipX = true;
        }

        _rigid.velocity = moveVector * speed;
        
        _animator.SetInteger("AnimState", moving ? 1 : 0);
    }
}
