using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D _rigid;
    public float horizontalSpeed = 5f;
    [SerializeField]
    private float _jumpforce = 4f;
    [SerializeField]
    private LayerMask _groundLayer;
    private bool resetJumpNeeded = false;

    private PlayerAnimation _playerAnim;
    private SpriteRenderer _playerSprite;

    void Start()
    {
        _rigid = GetComponent<Rigidbody2D>();
        _playerAnim = GetComponent<PlayerAnimation>();
        _playerSprite = GetComponentInChildren<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        Movment();

        if(Input.GetMouseButtonDown(0) && CheckGrounded() == true)
        {
            _playerAnim.Attack();
        }
    }

    void Movment()
    {
        float horizontalValue = Input.GetAxis("Horizontal");

        CheckGrounded();//Animasyon ve ray'i görebilmek için.
        FlipX(horizontalValue);

        if (Input.GetKeyDown(KeyCode.Space) && CheckGrounded() == true)
        {
            _rigid.velocity = new Vector2(_rigid.velocity.x, _jumpforce);
            StartCoroutine(ResetJumpNeededRoutine());
            _playerAnim.Jump(true);
        }
        
        _rigid.velocity = new Vector2(horizontalValue * horizontalSpeed, _rigid.velocity.y);
        _playerAnim.Move(horizontalValue);//animasyon için.
    }

    bool CheckGrounded()
    {
        RaycastHit2D HitInfo = Physics2D.Raycast(transform.position, Vector2.down, 0.75f, _groundLayer.value);
        Debug.DrawRay(transform.position, Vector2.down * 0.75f, Color.green);

        if (HitInfo.collider != null)
        {
            if (resetJumpNeeded == false)
            {
                _playerAnim.Jump(false);
                return true;
            }
        }
        return false;
    }

    void FlipX(float horizontalMove)
    {
        if (horizontalMove > 0)
        {
            _playerSprite.flipX = false;
        }
        else if (horizontalMove < 0)
        {
            _playerSprite.flipX = true;
        }
    }

    IEnumerator ResetJumpNeededRoutine()
    {
        resetJumpNeeded = true;
        yield return new WaitForSeconds(0.1f);
        resetJumpNeeded = false;
    }

}
