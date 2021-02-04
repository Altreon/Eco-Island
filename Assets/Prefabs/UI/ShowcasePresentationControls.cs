// GENERATED AUTOMATICALLY FROM 'Assets/Prefabs/UI/ShowcasePresentationControls.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @ShowcasePresentationControls : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @ShowcasePresentationControls()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""ShowcasePresentationControls"",
    ""maps"": [
        {
            ""name"": ""ShowcaseActions"",
            ""id"": ""f03ca8e7-72c9-4410-ba28-2b76fc8e8a55"",
            ""actions"": [
                {
                    ""name"": ""NextLine"",
                    ""type"": ""Button"",
                    ""id"": ""399dc5a0-eac8-4fba-bc3d-979e2de88911"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""bdf6a7b7-de81-4936-8378-f9a280c33f49"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""NextLine"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // ShowcaseActions
        m_ShowcaseActions = asset.FindActionMap("ShowcaseActions", throwIfNotFound: true);
        m_ShowcaseActions_NextLine = m_ShowcaseActions.FindAction("NextLine", throwIfNotFound: true);
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

    // ShowcaseActions
    private readonly InputActionMap m_ShowcaseActions;
    private IShowcaseActionsActions m_ShowcaseActionsActionsCallbackInterface;
    private readonly InputAction m_ShowcaseActions_NextLine;
    public struct ShowcaseActionsActions
    {
        private @ShowcasePresentationControls m_Wrapper;
        public ShowcaseActionsActions(@ShowcasePresentationControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @NextLine => m_Wrapper.m_ShowcaseActions_NextLine;
        public InputActionMap Get() { return m_Wrapper.m_ShowcaseActions; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(ShowcaseActionsActions set) { return set.Get(); }
        public void SetCallbacks(IShowcaseActionsActions instance)
        {
            if (m_Wrapper.m_ShowcaseActionsActionsCallbackInterface != null)
            {
                @NextLine.started -= m_Wrapper.m_ShowcaseActionsActionsCallbackInterface.OnNextLine;
                @NextLine.performed -= m_Wrapper.m_ShowcaseActionsActionsCallbackInterface.OnNextLine;
                @NextLine.canceled -= m_Wrapper.m_ShowcaseActionsActionsCallbackInterface.OnNextLine;
            }
            m_Wrapper.m_ShowcaseActionsActionsCallbackInterface = instance;
            if (instance != null)
            {
                @NextLine.started += instance.OnNextLine;
                @NextLine.performed += instance.OnNextLine;
                @NextLine.canceled += instance.OnNextLine;
            }
        }
    }
    public ShowcaseActionsActions @ShowcaseActions => new ShowcaseActionsActions(this);
    public interface IShowcaseActionsActions
    {
        void OnNextLine(InputAction.CallbackContext context);
    }
}
