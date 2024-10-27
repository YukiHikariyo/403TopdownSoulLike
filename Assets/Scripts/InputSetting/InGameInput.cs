//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.6.3
//     from Assets/Scripts/InputSetting/InGameInput.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public partial class @InGameInput: IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @InGameInput()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""InGameInput"",
    ""maps"": [
        {
            ""name"": ""Player"",
            ""id"": ""022a8b6e-c73e-4c99-a287-a8446a4cbfda"",
            ""actions"": [
                {
                    ""name"": ""MoveUp"",
                    ""type"": ""Button"",
                    ""id"": ""4ee97ef9-266e-4698-8c78-fa2eae2a1877"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""MoveDown"",
                    ""type"": ""Button"",
                    ""id"": ""3ba03088-3388-473c-97ea-44baacd8f96c"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""MoveLeft"",
                    ""type"": ""Button"",
                    ""id"": ""9e8dd44a-02f2-42e0-9b0e-36536179eef1"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""MoveRight"",
                    ""type"": ""Button"",
                    ""id"": ""9235845a-429c-4afe-9a92-94830088b3ad"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""LightAttack"",
                    ""type"": ""Button"",
                    ""id"": ""c85c3af7-2884-4c0b-8f4f-4e64fc2bd7d3"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""RightAttack"",
                    ""type"": ""Button"",
                    ""id"": ""05376558-d36c-4dd4-9cff-9bce4d052778"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Roll"",
                    ""type"": ""Button"",
                    ""id"": ""37cbf5ba-1214-47b8-8141-85a73a7eb5b8"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Magic_1"",
                    ""type"": ""Button"",
                    ""id"": ""ba04cbc6-ce6a-4f21-a5c2-d4e52c1fc257"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Magic_2"",
                    ""type"": ""Button"",
                    ""id"": ""229897ab-85e2-423c-92a3-768c4b3b254b"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Magic_3"",
                    ""type"": ""Button"",
                    ""id"": ""78e29d6a-4edc-4580-9785-108ee4ec4ccd"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Interaction"",
                    ""type"": ""Button"",
                    ""id"": ""c8d88901-f563-481f-adb3-77693263ad3b"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Health"",
                    ""type"": ""Button"",
                    ""id"": ""ff7a412c-faf8-47aa-9a61-750faf181b2e"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Mana"",
                    ""type"": ""Button"",
                    ""id"": ""65bec09e-bd71-4aa8-8e6d-dc53ff7fb88e"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Run"",
                    ""type"": ""Button"",
                    ""id"": ""7e81007f-d294-479b-bc8e-21c5d72414fe"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Package"",
                    ""type"": ""Button"",
                    ""id"": ""09401fce-d09b-45e1-a50a-9f00afd6436f"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""f7ca682b-10ba-40b8-81dd-2e981f0dce66"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyBoard/Mouse"",
                    ""action"": ""MoveUp"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c24dde2e-17da-450d-aca5-6dcdeed15b58"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyBoard/Mouse"",
                    ""action"": ""MoveDown"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""930dc7d1-6a3a-48f0-b36e-a6e05f46932d"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyBoard/Mouse"",
                    ""action"": ""MoveLeft"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""34ff0e34-c708-4998-a4ee-e43cc2db1819"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyBoard/Mouse"",
                    ""action"": ""MoveRight"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""8a48407c-d131-49be-b335-ec04d4fc6db0"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyBoard/Mouse"",
                    ""action"": ""LightAttack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""ae19c765-1051-4a9a-9383-c6687950a4cd"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyBoard/Mouse"",
                    ""action"": ""RightAttack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""8a5c28ae-e70f-4c03-8688-507235f63364"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyBoard/Mouse"",
                    ""action"": ""Roll"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""40935cd8-e473-4efe-8bee-d1be001f7c8e"",
                    ""path"": ""<Keyboard>/1"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Magic_1"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""350baab8-6397-4f17-b698-8dad04937e21"",
                    ""path"": ""<Keyboard>/2"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Magic_2"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b693e354-a223-4faa-a146-62048641b554"",
                    ""path"": ""<Keyboard>/3"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Magic_3"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""9601fe12-a8c5-43dc-be8b-fc5662f26813"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Interaction"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d7f1c9c8-0618-4923-a790-52a828953a53"",
                    ""path"": ""<Keyboard>/t"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Health"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""abd79035-fa7f-4c83-b4a5-b60b76d16cac"",
                    ""path"": ""<Keyboard>/g"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Mana"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""499dad65-a9f3-40bc-8e63-dc4faa63e17f"",
                    ""path"": ""<Keyboard>/shift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Run"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""5c9f98eb-2f26-4f40-a77c-8f59e70b7191"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Package"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""KeyBoard/Mouse"",
            ""bindingGroup"": ""KeyBoard/Mouse"",
            ""devices"": [
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // Player
        m_Player = asset.FindActionMap("Player", throwIfNotFound: true);
        m_Player_MoveUp = m_Player.FindAction("MoveUp", throwIfNotFound: true);
        m_Player_MoveDown = m_Player.FindAction("MoveDown", throwIfNotFound: true);
        m_Player_MoveLeft = m_Player.FindAction("MoveLeft", throwIfNotFound: true);
        m_Player_MoveRight = m_Player.FindAction("MoveRight", throwIfNotFound: true);
        m_Player_LightAttack = m_Player.FindAction("LightAttack", throwIfNotFound: true);
        m_Player_RightAttack = m_Player.FindAction("RightAttack", throwIfNotFound: true);
        m_Player_Roll = m_Player.FindAction("Roll", throwIfNotFound: true);
        m_Player_Magic_1 = m_Player.FindAction("Magic_1", throwIfNotFound: true);
        m_Player_Magic_2 = m_Player.FindAction("Magic_2", throwIfNotFound: true);
        m_Player_Magic_3 = m_Player.FindAction("Magic_3", throwIfNotFound: true);
        m_Player_Interaction = m_Player.FindAction("Interaction", throwIfNotFound: true);
        m_Player_Health = m_Player.FindAction("Health", throwIfNotFound: true);
        m_Player_Mana = m_Player.FindAction("Mana", throwIfNotFound: true);
        m_Player_Run = m_Player.FindAction("Run", throwIfNotFound: true);
        m_Player_Package = m_Player.FindAction("Package", throwIfNotFound: true);
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

    public IEnumerable<InputBinding> bindings => asset.bindings;

    public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
    {
        return asset.FindAction(actionNameOrId, throwIfNotFound);
    }

    public int FindBinding(InputBinding bindingMask, out InputAction action)
    {
        return asset.FindBinding(bindingMask, out action);
    }

    // Player
    private readonly InputActionMap m_Player;
    private List<IPlayerActions> m_PlayerActionsCallbackInterfaces = new List<IPlayerActions>();
    private readonly InputAction m_Player_MoveUp;
    private readonly InputAction m_Player_MoveDown;
    private readonly InputAction m_Player_MoveLeft;
    private readonly InputAction m_Player_MoveRight;
    private readonly InputAction m_Player_LightAttack;
    private readonly InputAction m_Player_RightAttack;
    private readonly InputAction m_Player_Roll;
    private readonly InputAction m_Player_Magic_1;
    private readonly InputAction m_Player_Magic_2;
    private readonly InputAction m_Player_Magic_3;
    private readonly InputAction m_Player_Interaction;
    private readonly InputAction m_Player_Health;
    private readonly InputAction m_Player_Mana;
    private readonly InputAction m_Player_Run;
    private readonly InputAction m_Player_Package;
    public struct PlayerActions
    {
        private @InGameInput m_Wrapper;
        public PlayerActions(@InGameInput wrapper) { m_Wrapper = wrapper; }
        public InputAction @MoveUp => m_Wrapper.m_Player_MoveUp;
        public InputAction @MoveDown => m_Wrapper.m_Player_MoveDown;
        public InputAction @MoveLeft => m_Wrapper.m_Player_MoveLeft;
        public InputAction @MoveRight => m_Wrapper.m_Player_MoveRight;
        public InputAction @LightAttack => m_Wrapper.m_Player_LightAttack;
        public InputAction @RightAttack => m_Wrapper.m_Player_RightAttack;
        public InputAction @Roll => m_Wrapper.m_Player_Roll;
        public InputAction @Magic_1 => m_Wrapper.m_Player_Magic_1;
        public InputAction @Magic_2 => m_Wrapper.m_Player_Magic_2;
        public InputAction @Magic_3 => m_Wrapper.m_Player_Magic_3;
        public InputAction @Interaction => m_Wrapper.m_Player_Interaction;
        public InputAction @Health => m_Wrapper.m_Player_Health;
        public InputAction @Mana => m_Wrapper.m_Player_Mana;
        public InputAction @Run => m_Wrapper.m_Player_Run;
        public InputAction @Package => m_Wrapper.m_Player_Package;
        public InputActionMap Get() { return m_Wrapper.m_Player; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerActions set) { return set.Get(); }
        public void AddCallbacks(IPlayerActions instance)
        {
            if (instance == null || m_Wrapper.m_PlayerActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_PlayerActionsCallbackInterfaces.Add(instance);
            @MoveUp.started += instance.OnMoveUp;
            @MoveUp.performed += instance.OnMoveUp;
            @MoveUp.canceled += instance.OnMoveUp;
            @MoveDown.started += instance.OnMoveDown;
            @MoveDown.performed += instance.OnMoveDown;
            @MoveDown.canceled += instance.OnMoveDown;
            @MoveLeft.started += instance.OnMoveLeft;
            @MoveLeft.performed += instance.OnMoveLeft;
            @MoveLeft.canceled += instance.OnMoveLeft;
            @MoveRight.started += instance.OnMoveRight;
            @MoveRight.performed += instance.OnMoveRight;
            @MoveRight.canceled += instance.OnMoveRight;
            @LightAttack.started += instance.OnLightAttack;
            @LightAttack.performed += instance.OnLightAttack;
            @LightAttack.canceled += instance.OnLightAttack;
            @RightAttack.started += instance.OnRightAttack;
            @RightAttack.performed += instance.OnRightAttack;
            @RightAttack.canceled += instance.OnRightAttack;
            @Roll.started += instance.OnRoll;
            @Roll.performed += instance.OnRoll;
            @Roll.canceled += instance.OnRoll;
            @Magic_1.started += instance.OnMagic_1;
            @Magic_1.performed += instance.OnMagic_1;
            @Magic_1.canceled += instance.OnMagic_1;
            @Magic_2.started += instance.OnMagic_2;
            @Magic_2.performed += instance.OnMagic_2;
            @Magic_2.canceled += instance.OnMagic_2;
            @Magic_3.started += instance.OnMagic_3;
            @Magic_3.performed += instance.OnMagic_3;
            @Magic_3.canceled += instance.OnMagic_3;
            @Interaction.started += instance.OnInteraction;
            @Interaction.performed += instance.OnInteraction;
            @Interaction.canceled += instance.OnInteraction;
            @Health.started += instance.OnHealth;
            @Health.performed += instance.OnHealth;
            @Health.canceled += instance.OnHealth;
            @Mana.started += instance.OnMana;
            @Mana.performed += instance.OnMana;
            @Mana.canceled += instance.OnMana;
            @Run.started += instance.OnRun;
            @Run.performed += instance.OnRun;
            @Run.canceled += instance.OnRun;
            @Package.started += instance.OnPackage;
            @Package.performed += instance.OnPackage;
            @Package.canceled += instance.OnPackage;
        }

        private void UnregisterCallbacks(IPlayerActions instance)
        {
            @MoveUp.started -= instance.OnMoveUp;
            @MoveUp.performed -= instance.OnMoveUp;
            @MoveUp.canceled -= instance.OnMoveUp;
            @MoveDown.started -= instance.OnMoveDown;
            @MoveDown.performed -= instance.OnMoveDown;
            @MoveDown.canceled -= instance.OnMoveDown;
            @MoveLeft.started -= instance.OnMoveLeft;
            @MoveLeft.performed -= instance.OnMoveLeft;
            @MoveLeft.canceled -= instance.OnMoveLeft;
            @MoveRight.started -= instance.OnMoveRight;
            @MoveRight.performed -= instance.OnMoveRight;
            @MoveRight.canceled -= instance.OnMoveRight;
            @LightAttack.started -= instance.OnLightAttack;
            @LightAttack.performed -= instance.OnLightAttack;
            @LightAttack.canceled -= instance.OnLightAttack;
            @RightAttack.started -= instance.OnRightAttack;
            @RightAttack.performed -= instance.OnRightAttack;
            @RightAttack.canceled -= instance.OnRightAttack;
            @Roll.started -= instance.OnRoll;
            @Roll.performed -= instance.OnRoll;
            @Roll.canceled -= instance.OnRoll;
            @Magic_1.started -= instance.OnMagic_1;
            @Magic_1.performed -= instance.OnMagic_1;
            @Magic_1.canceled -= instance.OnMagic_1;
            @Magic_2.started -= instance.OnMagic_2;
            @Magic_2.performed -= instance.OnMagic_2;
            @Magic_2.canceled -= instance.OnMagic_2;
            @Magic_3.started -= instance.OnMagic_3;
            @Magic_3.performed -= instance.OnMagic_3;
            @Magic_3.canceled -= instance.OnMagic_3;
            @Interaction.started -= instance.OnInteraction;
            @Interaction.performed -= instance.OnInteraction;
            @Interaction.canceled -= instance.OnInteraction;
            @Health.started -= instance.OnHealth;
            @Health.performed -= instance.OnHealth;
            @Health.canceled -= instance.OnHealth;
            @Mana.started -= instance.OnMana;
            @Mana.performed -= instance.OnMana;
            @Mana.canceled -= instance.OnMana;
            @Run.started -= instance.OnRun;
            @Run.performed -= instance.OnRun;
            @Run.canceled -= instance.OnRun;
            @Package.started -= instance.OnPackage;
            @Package.performed -= instance.OnPackage;
            @Package.canceled -= instance.OnPackage;
        }

        public void RemoveCallbacks(IPlayerActions instance)
        {
            if (m_Wrapper.m_PlayerActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IPlayerActions instance)
        {
            foreach (var item in m_Wrapper.m_PlayerActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_PlayerActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public PlayerActions @Player => new PlayerActions(this);
    private int m_KeyBoardMouseSchemeIndex = -1;
    public InputControlScheme KeyBoardMouseScheme
    {
        get
        {
            if (m_KeyBoardMouseSchemeIndex == -1) m_KeyBoardMouseSchemeIndex = asset.FindControlSchemeIndex("KeyBoard/Mouse");
            return asset.controlSchemes[m_KeyBoardMouseSchemeIndex];
        }
    }
    public interface IPlayerActions
    {
        void OnMoveUp(InputAction.CallbackContext context);
        void OnMoveDown(InputAction.CallbackContext context);
        void OnMoveLeft(InputAction.CallbackContext context);
        void OnMoveRight(InputAction.CallbackContext context);
        void OnLightAttack(InputAction.CallbackContext context);
        void OnRightAttack(InputAction.CallbackContext context);
        void OnRoll(InputAction.CallbackContext context);
        void OnMagic_1(InputAction.CallbackContext context);
        void OnMagic_2(InputAction.CallbackContext context);
        void OnMagic_3(InputAction.CallbackContext context);
        void OnInteraction(InputAction.CallbackContext context);
        void OnHealth(InputAction.CallbackContext context);
        void OnMana(InputAction.CallbackContext context);
        void OnRun(InputAction.CallbackContext context);
        void OnPackage(InputAction.CallbackContext context);
    }
}
