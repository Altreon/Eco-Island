using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/**
Classe : Player
Système qui permet au joueur de ramasser et de lâcher des Foods
*/

public class PlayerPickAndDrop : MonoBehaviour
{
    private CharacterController m_controller;
    public GameObject FruitHolder;
    public bool IsHandleFood = false;
    public Food TransportableFruit {get; private set;}
    private Food PickedUpFruit;
    private float MaxRange = 2.0f;
    private Collider[] ObjectsInViewRadius;
    private Rigidbody FuitRigidbody = null;
    private Vector3 NormalPosFruitHolder;

    private void Start()
    {
        m_controller = GetComponent<CharacterController>();
        NormalPosFruitHolder = FruitHolder.transform.localPosition;
        InputManager.Input.PlayerGhost.PickUpFood.performed += PickAndDropFood;
    }

    private void Update() 
    {
        if(PickedUpFruit)
        {
            PickedUpFruit.LastPlayerHoldTime = Time.time;
            FruitHolder.transform.localPosition = Vector3.Lerp(FruitHolder.transform.localPosition,NormalPosFruitHolder, 10f * Time.deltaTime); 
        }    
    }

     private void PickAndDropFood(InputAction.CallbackContext _context)
    {
        if(IsHandleFood)
        {
            DropOffFood();
        }
        else
        {
            if(ScreenshotHandler.instance.PhotoUIactive)
            {
                return;
            }
            else
            {
                PickUpFood();
            }
        }
    }

    private void PickUpFood()
    {
        ObjectsInViewRadius = Physics.OverlapSphere(transform.position,MaxRange);
        foreach(Collider obj in ObjectsInViewRadius)
        {
            if(obj.gameObject.layer == LayerMask.NameToLayer("Food"))
            {
                if(TransportableFruit == null || Vector3.Distance(transform.position,obj.transform.position) < Vector3.Distance(transform.position,TransportableFruit.transform.position))
                {
                    TransportableFruit = obj.gameObject.GetComponent<Food>();
                }
            }
        }

        if(TransportableFruit != null && TransportableFruit.Eatable)
        {
            IsHandleFood = true;
            PickedUpFruit = TransportableFruit;

            FuitRigidbody = PickedUpFruit.GetComponent<Rigidbody>();

            FuitRigidbody.useGravity = false;
            FuitRigidbody.isKinematic = true;

            FruitHolder.transform.position = PickedUpFruit.transform.position;
            PickedUpFruit.transform.SetParent(FruitHolder.transform);
            PickedUpFruit.transform.localPosition = Vector3.zero;

            switch (PickedUpFruit.FoodType)
            {
                case FoodType.Fruit:
                Physics.IgnoreCollision(m_controller,PickedUpFruit.GetComponent<SphereCollider>(),true);
                break;

                case FoodType.Vegetable:
                Physics.IgnoreCollision(m_controller,PickedUpFruit.GetComponent<CapsuleCollider>(),true);
                break;
            }
        }
    }

    public void DropOffFood()
    {
        PickedUpFruit.transform.parent = null;
        FuitRigidbody.useGravity = true;
        FuitRigidbody.isKinematic = false;
            
        switch (PickedUpFruit.FoodType)
        {
            case FoodType.Fruit:
            Physics.IgnoreCollision(m_controller,PickedUpFruit.GetComponent<SphereCollider>(),false);
            break;

            case FoodType.Vegetable:
            Physics.IgnoreCollision(m_controller,PickedUpFruit.GetComponent<CapsuleCollider>(),false);
            break;
        }

        PickedUpFruit.LastPlayerHoldTime = Time.time;

        PickedUpFruit = null;
        TransportableFruit = null;

        IsHandleFood = false;  
    }

    void OnDestroy () 
    {
        InputManager.Input.PlayerGhost.PickUpFood.performed -= PickAndDropFood;
    }
}
