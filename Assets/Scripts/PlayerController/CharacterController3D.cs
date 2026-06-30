using System;
using UnityEngine;
using UnityEngine.Serialization;
using Photon.Pun;
using TMPro;

public class CharacterController3D : MonoBehaviourPun
{
    
    [SerializeField] private float speed = 5f;

    [SerializeField] private Rigidbody rb = null;
    public Animator animator;
    public bool unbalance = false;
    private Vector3 _movement;
    private  TMP_InputField inputMes;
    
    #region MonoBehaviour

    private void Start()
    {
        animator.SetBool(AnimationKey.IsMelee, false);
        animator.SetBool(AnimationKey.IsEquipGun, false);
        animator.SetBool(AnimationKey.Is3D, true);
        inputMes = GameObject.Find("inputMes").GetComponent<TMP_InputField>();
    }

    private void Update()
    {
        if (!photonView.IsMine)
            return;
        
        if (this.rb.velocity.x > speed || this.rb.velocity.z > speed ||
            this.rb.velocity.x < -speed || this.rb.velocity.z < -speed)
        {
            unbalance = true;
        }
        else
        {
            unbalance = false;
        }
        /*animator.SetBool (WALK_PROPERTY,
          Math.Abs (_movement.sqrMagnitude) > Mathf.Epsilon);*/
        ProcessInput();
    }

    private void ProcessInput()
    {
        //Get Input
        float inputX = 0;
        float inputY = 0;

        inputX = Input.GetAxisRaw("Horizontal");
        inputY = Input.GetAxisRaw("Vertical");
        // Normalize
        _movement = new Vector3(inputX, 0, inputY).normalized;
    }

    private void FixedUpdate()
    {
        if (!unbalance)
            Move();
    }
    
    
    private void Move()
    {
        if(inputMes.isFocused)
            return;
        
        rb.velocity = new Vector3(_movement.x * speed, rb.velocity.y, _movement.z * speed);
        
        animator.SetFloat(AnimationKey.Speed, rb.velocity.x + rb.velocity.z / speed);
        if (_movement.x < 0)
        {
            animator.SetFloat(AnimationKey.Speed, -rb.velocity.x + rb.velocity.z/ speed);
            var transformLocalScale = this.transform.localScale;
            transformLocalScale.x  = -1;
            transform.localScale = transformLocalScale;
        }
        else if (_movement.x > 0)
        {
            animator.SetFloat(AnimationKey.Speed, rb.velocity.x + rb.velocity.z/ speed);
            var transformLocalScale = this.transform.localScale;
            transformLocalScale.x  = 1;
            transform.localScale = transformLocalScale;
        }
        else if (_movement.z < 0)
        {
            animator.SetFloat(AnimationKey.Speed, rb.velocity.x + -rb.velocity.z/ speed);
        }
    }

    #endregion
}