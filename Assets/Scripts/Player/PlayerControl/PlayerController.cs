using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/**
Classe : PlayerController
Contrôle les déplacement du joueur selon les entrée clavier/souris de l'application
*/

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    private CharacterController controller;
    private float walkSpeed;
    private float runSpeed;
    public float currentSpeed;
    public Vector2 MoveVector;
    private float slowFactor;
    public bool isSlow = false;
    private float gravity;
    private Vector3 velocity;

    //Pour le calcul de velocité (unit/second)
    private Vector3 current_pos;
    private Vector3 last_pos;
    public Vector3 Velocity {get; private set;}
    public bool IsOnGround {get{return controller.isGrounded; }}

    private float jumpForce;

     public bool ControllerActive {
        get{ return controller.enabled; }
        set{
            if(controller.enabled == value) return;

            controller.enabled = value;
        }
    }

    private void Start()
    {
        controller = GetComponent<CharacterController>();

        InputManager.Input.PlayerGhost.Movement.performed += OnMove;
        InputManager.Input.PlayerGhost.Jump.performed += OnJump;

        InputManager.Input.PlayerGhost.Sprint.started += StartSprint;
        InputManager.Input.PlayerGhost.Sprint.canceled += StopSprint;

        InputManager.Input.PlayerGhost.Slow.started += OnSlow;
        InputManager.Input.PlayerGhost.Slow.canceled += OffSlow;

        this.walkSpeed = Player.Instance.PlayerPreset.walkSpeed;
        this.runSpeed = Player.Instance.PlayerPreset.runSpeed;
        this.slowFactor = Player.Instance.PlayerPreset.slowFactor;
        this.gravity = Player.Instance.PlayerPreset.gravity;
        this.jumpForce = Player.Instance.PlayerPreset.jumpForce;


        current_pos = transform.position;
        last_pos = transform.position;

        currentSpeed = walkSpeed;
    }

    private void OnMove (InputAction.CallbackContext _context) 
    {
        Vector2 move = _context.ReadValue<Vector2>();
        MoveVector = (move.magnitude < 0.05f ? Vector2.zero : move);
    }

    private void OnSlow (InputAction.CallbackContext _context) 
    {
        isSlow = true;
    }

    private void OffSlow (InputAction.CallbackContext _context) 
    {
        isSlow = false;
    }

    public void OnJump(InputAction.CallbackContext _context)
    {
        if(controller.isGrounded)
        {
            velocity.y = jumpForce;
        }
    }

    private void Update()
    {
        if(!InputManager.Input.PlayerGhost.enabled)
        {
            MoveVector = Vector2.zero;
        }

        Move(MoveVector);
        ApplyGravity();
    }

    private void FixedUpdate () {
        current_pos = transform.position;
        Velocity = (current_pos - last_pos) / Time.deltaTime;
        last_pos = current_pos;
    }

    private void LateUpdate () {
        Rotate();
    }

    private void Move(Vector2 _moveVector)
    {
        if(_moveVector.sqrMagnitude < 0.01) return;

        Vector3 moveDirection = new Vector3(_moveVector.x,0,_moveVector.y);

        Vector3 toWater = (Quaternion.Euler(0, CameraController.Instance.transform.rotation.eulerAngles.y, 0) * moveDirection).normalized;
        toWater = (toWater + Vector3.down).normalized;

        RaycastHit hit;
        bool canMove = true;
        if (Physics.Raycast(transform.position, toWater.normalized, out hit, 10, LayerMask.GetMask("Water", "Terrain"))) {
            if(hit.collider.gameObject.layer == LayerMask.NameToLayer("Water")) canMove = false;
        }
        if(!canMove) return;

        moveDirection = transform.TransformDirection(moveDirection);
        moveDirection *= currentSpeed;
        if(isSlow) moveDirection /= slowFactor;

        controller.Move(moveDirection * Time.deltaTime);
    }

    public void ForceMove (Vector3 moveVector)
    {
        if(moveVector.sqrMagnitude < 0.01) return;

        Vector3 moveDirection = new Vector3(moveVector.x,0,moveVector.y);

        Vector3 toWater = (Quaternion.Euler(0, CameraController.Instance.transform.rotation.eulerAngles.y, 0) * moveDirection).normalized;
        toWater = (toWater + Vector3.down).normalized;

        RaycastHit hit;
        bool canMove = true;
        if (Physics.Raycast(transform.position, toWater.normalized, out hit, 10, LayerMask.GetMask("Water", "Terrain"))) {
            if(hit.collider.gameObject.layer == LayerMask.NameToLayer("Water")) canMove = false;
        }
        if(!canMove) return;

        controller.Move(moveVector * Time.deltaTime);
    }

    private void Rotate () {
        transform.rotation = Quaternion.Euler(0, CameraController.Instance.transform.rotation.eulerAngles.y, 0);
    }

    private void ApplyGravity()
    {
        if(controller.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        velocity.y -= gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    private void StartSprint(InputAction.CallbackContext _context)
    {
        currentSpeed = runSpeed;
    }

    private void StopSprint(InputAction.CallbackContext _context)
    {
        currentSpeed = walkSpeed;
    }

    void OnDestroy () 
    {
        InputManager.Input.PlayerGhost.Movement.performed -= OnMove;
        InputManager.Input.PlayerGhost.Sprint.started -= StartSprint;
        InputManager.Input.PlayerGhost.Sprint.canceled -= StopSprint;
        InputManager.Input.PlayerGhost.Jump.performed -= OnJump;
        InputManager.Input.PlayerGhost.Slow.started -= OnSlow;
        InputManager.Input.PlayerGhost.Slow.canceled -= OffSlow;
    }
}
