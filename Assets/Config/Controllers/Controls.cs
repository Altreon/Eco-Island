// GENERATED AUTOMATICALLY FROM 'Assets/Config/Controllers/Controls.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @Controls : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @Controls()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""Controls"",
    ""maps"": [
        {
            ""name"": ""Player"",
            ""id"": ""38910dc1-70c0-413d-a120-846c2e239497"",
            ""actions"": [
                {
                    ""name"": ""Menu"",
                    ""type"": ""Button"",
                    ""id"": ""636e87d0-f8f0-4edf-8e65-df3f9d559279"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Ecodex"",
                    ""type"": ""Button"",
                    ""id"": ""ce4ec132-cc2b-443a-914a-984d4c6bad4f"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""9ef0b4e0-c93f-4b42-858d-87b7b80ee0a9"",
                    ""path"": ""<Keyboard>/q"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Ecodex"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""4e0cf393-a07f-4ddf-84a9-6ad3b755c540"",
                    ""path"": ""<Gamepad>/select"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Ecodex"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""4aa6ad0d-736c-4528-8842-31b954d065a0"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Menu"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""40eeb401-a810-42c3-b41d-ed40398711fa"",
                    ""path"": ""<Gamepad>/start"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Menu"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""PlayerGhost"",
            ""id"": ""b906c108-6d02-4808-bae9-5eb9adbc6a01"",
            ""actions"": [
                {
                    ""name"": ""Jump"",
                    ""type"": ""Button"",
                    ""id"": ""76a7e030-a038-498a-8acc-840daaa28b31"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Movement"",
                    ""type"": ""PassThrough"",
                    ""id"": ""0f786c2e-cfb8-4094-9f37-02fcc8ef8710"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Camera"",
                    ""type"": ""PassThrough"",
                    ""id"": ""305fd9bb-298f-44ba-8f41-67335148aa2d"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""PickUpFood"",
                    ""type"": ""Button"",
                    ""id"": ""8dafa5a0-f4e5-4c45-bbb5-43f792fc53be"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Slow"",
                    ""type"": ""Button"",
                    ""id"": ""5c294979-8967-4159-bbd1-f6c8036665e6"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""TakePicture"",
                    ""type"": ""Button"",
                    ""id"": ""bfd66a14-33a6-4725-8d1c-8095d3dce5e6"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""PhotoMode"",
                    ""type"": ""Button"",
                    ""id"": ""aa26150d-90dc-419d-946b-dfbadad5512b"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Flashlight"",
                    ""type"": ""Button"",
                    ""id"": ""33771d1d-b3d1-46c6-83b6-afe556f4917c"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Sprint"",
                    ""type"": ""Button"",
                    ""id"": ""a0e1739c-bad4-4c83-aae8-39375026e6a6"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""ToggleFlash"",
                    ""type"": ""Button"",
                    ""id"": ""e7d9f844-dc1b-408b-9473-92f34d163906"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""TogglePhotoMode"",
                    ""type"": ""Button"",
                    ""id"": ""591b61e8-ecd4-4847-b7c0-daf130478b0f"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""267ab526-96ad-4a9b-a094-316be2af24a7"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""0cd90712-983d-466f-a7d4-2bbbec9801bf"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""Arrow"",
                    ""id"": ""7e738a61-5f41-4cd6-9a13-228558e7fdc0"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""61947dc4-b275-429c-b1ae-5acb3c0c3d59"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""45abd10f-30c9-4e0b-b490-d31395c5a355"",
                    ""path"": ""<Keyboard>/downArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""fb2cf835-e785-4b15-84dd-7e3ad39de2c7"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""d08aa287-bb56-4631-abc3-19af67089d9e"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""AZERTY"",
                    ""id"": ""16b35451-ad2f-4801-9347-d107ab7fa765"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""09668ef1-4c08-45cb-9207-ae46d1de6561"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""6e0c9a4d-eaf0-49c6-bccd-f2a2fce6b15d"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""620f92cc-8447-4616-9f5b-6a2b669c2953"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""8e7c129c-cfa8-449f-9c4d-9591742f2589"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""64b8e8e2-2e3b-4491-a574-12b94b0b8b57"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""7b8d3dc1-2019-45bf-812c-69b471e0cfa4"",
                    ""path"": ""<Mouse>/delta"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Mouse"",
                    ""action"": ""Camera"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c76f3a12-1e87-4601-82d6-585d8903b801"",
                    ""path"": ""<Gamepad>/rightStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Camera"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""3e0f9d95-f2b5-44b9-92ce-ff7e30e770c9"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""PickUpFood"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""82c82981-d53d-4f30-84df-5391c874cce9"",
                    ""path"": ""<Gamepad>/buttonWest"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""PickUpFood"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""67c93ce6-8281-46a2-8e5d-be533b8849a7"",
                    ""path"": ""<Keyboard>/leftCtrl"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Slow"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""5796e88c-aa7b-46b7-ab90-27cdb40fadbc"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""TakePicture"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""2913727c-8f87-4ed8-b5e9-01456a247949"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Mouse"",
                    ""action"": ""TakePicture"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a3ffc24f-b191-43f4-8c56-32df8d3bc44d"",
                    ""path"": ""<Gamepad>/rightTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""TakePicture"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""eeaa49db-ed59-4c6c-83ef-88f57d30806c"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Mouse"",
                    ""action"": ""PhotoMode"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a5e03f68-4986-4626-8287-757fd3ba1dc0"",
                    ""path"": ""<Gamepad>/leftTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""PhotoMode"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""2657f501-1bf7-47e5-b2d3-1ea5b261b11b"",
                    ""path"": ""<Keyboard>/tab"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Flashlight"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""074bbef1-40d9-4bc9-b8a9-3ae18edbb37c"",
                    ""path"": ""<Gamepad>/leftShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Flashlight"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""7cf872ad-e7b1-44f0-a854-16c493cd0691"",
                    ""path"": ""<Keyboard>/leftShift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Sprint"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""8088163d-2b26-45c6-b91a-ad0c284219e3"",
                    ""path"": ""<Gamepad>/rightShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Sprint"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""ea50b00d-5509-4bda-91d9-744330c26aca"",
                    ""path"": ""<Keyboard>/q"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""ToggleFlash"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""2c6505ab-b72d-4bf5-9fe0-d3c5c0118049"",
                    ""path"": ""<Gamepad>/select"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""ToggleFlash"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""dfa15fd5-36c9-445c-b194-29fc472cd881"",
                    ""path"": ""<Keyboard>/r"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""TogglePhotoMode"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""19b7f436-c4e4-4d1c-8943-be8e2da79bc9"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Mouse"",
                    ""action"": ""PickUpFood"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""UI"",
            ""id"": ""920a30c6-f11c-4118-b6b9-72b0b29408a9"",
            ""actions"": [
                {
                    ""name"": ""LeftClick"",
                    ""type"": ""Button"",
                    ""id"": ""bd9a5a6c-17c9-4e73-94fb-101bfd75c4ac"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""0157dcbe-d961-4de0-9802-5dfc8f67cd99"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Mouse"",
                    ""action"": ""LeftClick"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Keyboard"",
            ""bindingGroup"": ""Keyboard"",
            ""devices"": [
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        },
        {
            ""name"": ""Gamepad"",
            ""bindingGroup"": ""Gamepad"",
            ""devices"": [
                {
                    ""devicePath"": ""<Gamepad>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        },
        {
            ""name"": ""Mouse"",
            ""bindingGroup"": ""Mouse"",
            ""devices"": [
                {
                    ""devicePath"": ""<Mouse>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // Player
        m_Player = asset.FindActionMap("Player", throwIfNotFound: true);
        m_Player_Menu = m_Player.FindAction("Menu", throwIfNotFound: true);
        m_Player_Ecodex = m_Player.FindAction("Ecodex", throwIfNotFound: true);
        // PlayerGhost
        m_PlayerGhost = asset.FindActionMap("PlayerGhost", throwIfNotFound: true);
        m_PlayerGhost_Jump = m_PlayerGhost.FindAction("Jump", throwIfNotFound: true);
        m_PlayerGhost_Movement = m_PlayerGhost.FindAction("Movement", throwIfNotFound: true);
        m_PlayerGhost_Camera = m_PlayerGhost.FindAction("Camera", throwIfNotFound: true);
        m_PlayerGhost_PickUpFood = m_PlayerGhost.FindAction("PickUpFood", throwIfNotFound: true);
        m_PlayerGhost_Slow = m_PlayerGhost.FindAction("Slow", throwIfNotFound: true);
        m_PlayerGhost_TakePicture = m_PlayerGhost.FindAction("TakePicture", throwIfNotFound: true);
        m_PlayerGhost_PhotoMode = m_PlayerGhost.FindAction("PhotoMode", throwIfNotFound: true);
        m_PlayerGhost_Flashlight = m_PlayerGhost.FindAction("Flashlight", throwIfNotFound: true);
        m_PlayerGhost_Sprint = m_PlayerGhost.FindAction("Sprint", throwIfNotFound: true);
        m_PlayerGhost_ToggleFlash = m_PlayerGhost.FindAction("ToggleFlash", throwIfNotFound: true);
        m_PlayerGhost_TogglePhotoMode = m_PlayerGhost.FindAction("TogglePhotoMode", throwIfNotFound: true);
        // UI
        m_UI = asset.FindActionMap("UI", throwIfNotFound: true);
        m_UI_LeftClick = m_UI.FindAction("LeftClick", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }

    // Player
    private readonly InputActionMap m_Player;
    private IPlayerActions m_PlayerActionsCallbackInterface;
    private readonly InputAction m_Player_Menu;
    private readonly InputAction m_Player_Ecodex;
    public struct PlayerActions
    {
        private @Controls m_Wrapper;
        public PlayerActions(@Controls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Menu => m_Wrapper.m_Player_Menu;
        public InputAction @Ecodex => m_Wrapper.m_Player_Ecodex;
        public InputActionMap Get() { return m_Wrapper.m_Player; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerActions set) { return set.Get(); }
        public void SetCallbacks(IPlayerActions instance)
        {
            if (m_Wrapper.m_PlayerActionsCallbackInterface != null)
            {
                @Menu.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMenu;
                @Menu.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMenu;
                @Menu.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMenu;
                @Ecodex.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnEcodex;
                @Ecodex.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnEcodex;
                @Ecodex.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnEcodex;
            }
            m_Wrapper.m_PlayerActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Menu.started += instance.OnMenu;
                @Menu.performed += instance.OnMenu;
                @Menu.canceled += instance.OnMenu;
                @Ecodex.started += instance.OnEcodex;
                @Ecodex.performed += instance.OnEcodex;
                @Ecodex.canceled += instance.OnEcodex;
            }
        }
    }
    public PlayerActions @Player => new PlayerActions(this);

    // PlayerGhost
    private readonly InputActionMap m_PlayerGhost;
    private IPlayerGhostActions m_PlayerGhostActionsCallbackInterface;
    private readonly InputAction m_PlayerGhost_Jump;
    private readonly InputAction m_PlayerGhost_Movement;
    private readonly InputAction m_PlayerGhost_Camera;
    private readonly InputAction m_PlayerGhost_PickUpFood;
    private readonly InputAction m_PlayerGhost_Slow;
    private readonly InputAction m_PlayerGhost_TakePicture;
    private readonly InputAction m_PlayerGhost_PhotoMode;
    private readonly InputAction m_PlayerGhost_Flashlight;
    private readonly InputAction m_PlayerGhost_Sprint;
    private readonly InputAction m_PlayerGhost_ToggleFlash;
    private readonly InputAction m_PlayerGhost_TogglePhotoMode;
    public struct PlayerGhostActions
    {
        private @Controls m_Wrapper;
        public PlayerGhostActions(@Controls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Jump => m_Wrapper.m_PlayerGhost_Jump;
        public InputAction @Movement => m_Wrapper.m_PlayerGhost_Movement;
        public InputAction @Camera => m_Wrapper.m_PlayerGhost_Camera;
        public InputAction @PickUpFood => m_Wrapper.m_PlayerGhost_PickUpFood;
        public InputAction @Slow => m_Wrapper.m_PlayerGhost_Slow;
        public InputAction @TakePicture => m_Wrapper.m_PlayerGhost_TakePicture;
        public InputAction @PhotoMode => m_Wrapper.m_PlayerGhost_PhotoMode;
        public InputAction @Flashlight => m_Wrapper.m_PlayerGhost_Flashlight;
        public InputAction @Sprint => m_Wrapper.m_PlayerGhost_Sprint;
        public InputAction @ToggleFlash => m_Wrapper.m_PlayerGhost_ToggleFlash;
        public InputAction @TogglePhotoMode => m_Wrapper.m_PlayerGhost_TogglePhotoMode;
        public InputActionMap Get() { return m_Wrapper.m_PlayerGhost; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerGhostActions set) { return set.Get(); }
        public void SetCallbacks(IPlayerGhostActions instance)
        {
            if (m_Wrapper.m_PlayerGhostActionsCallbackInterface != null)
            {
                @Jump.started -= m_Wrapper.m_PlayerGhostActionsCallbackInterface.OnJump;
                @Jump.performed -= m_Wrapper.m_PlayerGhostActionsCallbackInterface.OnJump;
                @Jump.canceled -= m_Wrapper.m_PlayerGhostActionsCallbackInterface.OnJump;
                @Movement.started -= m_Wrapper.m_PlayerGhostActionsCallbackInterface.OnMovement;
                @Movement.performed -= m_Wrapper.m_PlayerGhostActionsCallbackInterface.OnMovement;
                @Movement.canceled -= m_Wrapper.m_PlayerGhostActionsCallbackInterface.OnMovement;
                @Camera.started -= m_Wrapper.m_PlayerGhostActionsCallbackInterface.OnCamera;
                @Camera.performed -= m_Wrapper.m_PlayerGhostActionsCallbackInterface.OnCamera;
                @Camera.canceled -= m_Wrapper.m_PlayerGhostActionsCallbackInterface.OnCamera;
                @PickUpFood.started -= m_Wrapper.m_PlayerGhostActionsCallbackInterface.OnPickUpFood;
                @PickUpFood.performed -= m_Wrapper.m_PlayerGhostActionsCallbackInterface.OnPickUpFood;
                @PickUpFood.canceled -= m_Wrapper.m_PlayerGhostActionsCallbackInterface.OnPickUpFood;
                @Slow.started -= m_Wrapper.m_PlayerGhostActionsCallbackInterface.OnSlow;
                @Slow.performed -= m_Wrapper.m_PlayerGhostActionsCallbackInterface.OnSlow;
                @Slow.canceled -= m_Wrapper.m_PlayerGhostActionsCallbackInterface.OnSlow;
                @TakePicture.started -= m_Wrapper.m_PlayerGhostActionsCallbackInterface.OnTakePicture;
                @TakePicture.performed -= m_Wrapper.m_PlayerGhostActionsCallbackInterface.OnTakePicture;
                @TakePicture.canceled -= m_Wrapper.m_PlayerGhostActionsCallbackInterface.OnTakePicture;
                @PhotoMode.started -= m_Wrapper.m_PlayerGhostActionsCallbackInterface.OnPhotoMode;
                @PhotoMode.performed -= m_Wrapper.m_PlayerGhostActionsCallbackInterface.OnPhotoMode;
                @PhotoMode.canceled -= m_Wrapper.m_PlayerGhostActionsCallbackInterface.OnPhotoMode;
                @Flashlight.started -= m_Wrapper.m_PlayerGhostActionsCallbackInterface.OnFlashlight;
                @Flashlight.performed -= m_Wrapper.m_PlayerGhostActionsCallbackInterface.OnFlashlight;
                @Flashlight.canceled -= m_Wrapper.m_PlayerGhostActionsCallbackInterface.OnFlashlight;
                @Sprint.started -= m_Wrapper.m_PlayerGhostActionsCallbackInterface.OnSprint;
                @Sprint.performed -= m_Wrapper.m_PlayerGhostActionsCallbackInterface.OnSprint;
                @Sprint.canceled -= m_Wrapper.m_PlayerGhostActionsCallbackInterface.OnSprint;
                @ToggleFlash.started -= m_Wrapper.m_PlayerGhostActionsCallbackInterface.OnToggleFlash;
                @ToggleFlash.performed -= m_Wrapper.m_PlayerGhostActionsCallbackInterface.OnToggleFlash;
                @ToggleFlash.canceled -= m_Wrapper.m_PlayerGhostActionsCallbackInterface.OnToggleFlash;
                @TogglePhotoMode.started -= m_Wrapper.m_PlayerGhostActionsCallbackInterface.OnTogglePhotoMode;
                @TogglePhotoMode.performed -= m_Wrapper.m_PlayerGhostActionsCallbackInterface.OnTogglePhotoMode;
                @TogglePhotoMode.canceled -= m_Wrapper.m_PlayerGhostActionsCallbackInterface.OnTogglePhotoMode;
            }
            m_Wrapper.m_PlayerGhostActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Jump.started += instance.OnJump;
                @Jump.performed += instance.OnJump;
                @Jump.canceled += instance.OnJump;
                @Movement.started += instance.OnMovement;
                @Movement.performed += instance.OnMovement;
                @Movement.canceled += instance.OnMovement;
                @Camera.started += instance.OnCamera;
                @Camera.performed += instance.OnCamera;
                @Camera.canceled += instance.OnCamera;
                @PickUpFood.started += instance.OnPickUpFood;
                @PickUpFood.performed += instance.OnPickUpFood;
                @PickUpFood.canceled += instance.OnPickUpFood;
                @Slow.started += instance.OnSlow;
                @Slow.performed += instance.OnSlow;
                @Slow.canceled += instance.OnSlow;
                @TakePicture.started += instance.OnTakePicture;
                @TakePicture.performed += instance.OnTakePicture;
                @TakePicture.canceled += instance.OnTakePicture;
                @PhotoMode.started += instance.OnPhotoMode;
                @PhotoMode.performed += instance.OnPhotoMode;
                @PhotoMode.canceled += instance.OnPhotoMode;
                @Flashlight.started += instance.OnFlashlight;
                @Flashlight.performed += instance.OnFlashlight;
                @Flashlight.canceled += instance.OnFlashlight;
                @Sprint.started += instance.OnSprint;
                @Sprint.performed += instance.OnSprint;
                @Sprint.canceled += instance.OnSprint;
                @ToggleFlash.started += instance.OnToggleFlash;
                @ToggleFlash.performed += instance.OnToggleFlash;
                @ToggleFlash.canceled += instance.OnToggleFlash;
                @TogglePhotoMode.started += instance.OnTogglePhotoMode;
                @TogglePhotoMode.performed += instance.OnTogglePhotoMode;
                @TogglePhotoMode.canceled += instance.OnTogglePhotoMode;
            }
        }
    }
    public PlayerGhostActions @PlayerGhost => new PlayerGhostActions(this);

    // UI
    private readonly InputActionMap m_UI;
    private IUIActions m_UIActionsCallbackInterface;
    private readonly InputAction m_UI_LeftClick;
    public struct UIActions
    {
        private @Controls m_Wrapper;
        public UIActions(@Controls wrapper) { m_Wrapper = wrapper; }
        public InputAction @LeftClick => m_Wrapper.m_UI_LeftClick;
        public InputActionMap Get() { return m_Wrapper.m_UI; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(UIActions set) { return set.Get(); }
        public void SetCallbacks(IUIActions instance)
        {
            if (m_Wrapper.m_UIActionsCallbackInterface != null)
            {
                @LeftClick.started -= m_Wrapper.m_UIActionsCallbackInterface.OnLeftClick;
                @LeftClick.performed -= m_Wrapper.m_UIActionsCallbackInterface.OnLeftClick;
                @LeftClick.canceled -= m_Wrapper.m_UIActionsCallbackInterface.OnLeftClick;
            }
            m_Wrapper.m_UIActionsCallbackInterface = instance;
            if (instance != null)
            {
                @LeftClick.started += instance.OnLeftClick;
                @LeftClick.performed += instance.OnLeftClick;
                @LeftClick.canceled += instance.OnLeftClick;
            }
        }
    }
    public UIActions @UI => new UIActions(this);
    private int m_KeyboardSchemeIndex = -1;
    public InputControlScheme KeyboardScheme
    {
        get
        {
            if (m_KeyboardSchemeIndex == -1) m_KeyboardSchemeIndex = asset.FindControlSchemeIndex("Keyboard");
            return asset.controlSchemes[m_KeyboardSchemeIndex];
        }
    }
    private int m_GamepadSchemeIndex = -1;
    public InputControlScheme GamepadScheme
    {
        get
        {
            if (m_GamepadSchemeIndex == -1) m_GamepadSchemeIndex = asset.FindControlSchemeIndex("Gamepad");
            return asset.controlSchemes[m_GamepadSchemeIndex];
        }
    }
    private int m_MouseSchemeIndex = -1;
    public InputControlScheme MouseScheme
    {
        get
        {
            if (m_MouseSchemeIndex == -1) m_MouseSchemeIndex = asset.FindControlSchemeIndex("Mouse");
            return asset.controlSchemes[m_MouseSchemeIndex];
        }
    }
    public interface IPlayerActions
    {
        void OnMenu(InputAction.CallbackContext context);
        void OnEcodex(InputAction.CallbackContext context);
    }
    public interface IPlayerGhostActions
    {
        void OnJump(InputAction.CallbackContext context);
        void OnMovement(InputAction.CallbackContext context);
        void OnCamera(InputAction.CallbackContext context);
        void OnPickUpFood(InputAction.CallbackContext context);
        void OnSlow(InputAction.CallbackContext context);
        void OnTakePicture(InputAction.CallbackContext context);
        void OnPhotoMode(InputAction.CallbackContext context);
        void OnFlashlight(InputAction.CallbackContext context);
        void OnSprint(InputAction.CallbackContext context);
        void OnToggleFlash(InputAction.CallbackContext context);
        void OnTogglePhotoMode(InputAction.CallbackContext context);
    }
    public interface IUIActions
    {
        void OnLeftClick(InputAction.CallbackContext context);
    }
}
