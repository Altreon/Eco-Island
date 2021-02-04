using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CreatureController : MonoBehaviour
{
    private float vertical;
    private float horizontal;
    
    [SerializeField] private float _moveSpeed = 3;
    [SerializeField] private float _rotationSpeed = 4;

    // Start is called before the first frame update
    void Start()
    {
        InputManagerJirachi.Input.PlayerGhost.Movement.performed += Movement;
    }

    // OnDestroy est appelé à la destruction de l'objet
    void OnDestroy()
    {
        InputManagerJirachi.Input.PlayerGhost.Movement.performed -= Movement;
    }

    private void Movement(InputAction.CallbackContext cbc)
    {
        Debug.Log(cbc.ReadValue<Vector2>());
        horizontal = cbc.ReadValue<Vector2>().x;
        vertical = cbc.ReadValue<Vector2>().y;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(0, _rotationSpeed * horizontal, 0));
        transform.position += transform.forward * vertical * _moveSpeed * Time.deltaTime;
    }
}
