// GENERATED AUTOMATICALLY FROM 'Assets/Settings/Game Controls/Game Controls.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @GameControls : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @GameControls()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""Game Controls"",
    ""maps"": [
        {
            ""name"": ""Gameplay"",
            ""id"": ""6cd72ab2-9581-45c8-865d-8c559aaa9fb0"",
            ""actions"": [
                {
                    ""name"": ""Movement"",
                    ""type"": ""PassThrough"",
                    ""id"": ""788a117f-82af-4d31-8d93-7ecd93c435eb"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Rewind"",
                    ""type"": ""Button"",
                    ""id"": ""7bda7651-605e-4c5e-822e-7a395d4498b7"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press""
                },
                {
                    ""name"": ""Restart"",
                    ""type"": ""Button"",
                    ""id"": ""0d751531-09b3-492f-938c-44c23c1d0e81"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press""
                },
                {
                    ""name"": ""Click"",
                    ""type"": ""Button"",
                    ""id"": ""afde2e99-0105-4de5-88d3-a6bb5e786936"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press""
                },
                {
                    ""name"": ""Debug Delete Save Data"",
                    ""type"": ""Button"",
                    ""id"": ""1d2a28ac-5fc8-41cb-819d-ead0bc855a53"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press""
                },
                {
                    ""name"": ""Debug Complete Level"",
                    ""type"": ""Button"",
                    ""id"": ""8a5b591b-0d86-433a-986d-c984ef31dfc3"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press""
                },
                {
                    ""name"": ""Debug Key 1"",
                    ""type"": ""Button"",
                    ""id"": ""607ce4e9-7ba6-44b4-9d0c-527befba9380"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press""
                },
                {
                    ""name"": ""Debug Key 2"",
                    ""type"": ""Button"",
                    ""id"": ""a606b6d3-6ac0-4a2b-96e4-44ec3c39c192"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press(behavior=2)""
                },
                {
                    ""name"": ""Debug Key 3"",
                    ""type"": ""Button"",
                    ""id"": ""6911e142-9c31-4e23-a257-64430a40a93e"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press""
                },
                {
                    ""name"": ""Debug Key 4"",
                    ""type"": ""Button"",
                    ""id"": ""e2d5e8eb-87d1-4092-9092-bac8ca02d7f3"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press""
                },
                {
                    ""name"": ""Debug Next Level"",
                    ""type"": ""Button"",
                    ""id"": ""58c59055-1079-482e-a655-0965e526f5f8"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press""
                },
                {
                    ""name"": ""Debug Prev Level"",
                    ""type"": ""Button"",
                    ""id"": ""414e89f8-27ab-455b-9967-dcc804407536"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press""
                },
                {
                    ""name"": ""Quit Game"",
                    ""type"": ""Button"",
                    ""id"": ""1eafc677-770c-4ff9-be0c-d8ef488e700a"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press""
                },
                {
                    ""name"": ""Debug Key Z"",
                    ""type"": ""Button"",
                    ""id"": ""c9bb0c94-92b6-446e-b748-eadfaa322c55"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press""
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""Thumbstick "",
                    ""id"": ""9fdda200-3453-4b63-875a-67f1973398b3"",
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
                    ""id"": ""315519db-09d7-43bf-83a9-01c75192aee4"",
                    ""path"": ""<Gamepad>/leftStick/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""0b697a29-bc20-40ee-9463-4b20bfdad997"",
                    ""path"": ""<Gamepad>/leftStick/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""17b27dee-299c-46f1-9629-ad091812f7ec"",
                    ""path"": ""<Gamepad>/leftStick/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""45170ad6-28c2-4290-954c-5383e11a161b"",
                    ""path"": ""<Gamepad>/leftStick/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""WASD Keys"",
                    ""id"": ""2efbdc44-8e79-462d-b815-61f295973a23"",
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
                    ""id"": ""876b4571-1add-443f-bcdd-37abf826e458"",
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
                    ""id"": ""7678f01c-6e61-42ed-bae6-4c6ae67b3eb6"",
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
                    ""id"": ""53d49481-a061-4037-bfd3-9af82d005927"",
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
                    ""id"": ""51d66e9b-67e9-491f-8595-838ae86ce986"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Arrow Keys"",
                    ""id"": ""ad0f3fa0-776c-4a7f-aecf-9876aa20aa85"",
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
                    ""id"": ""1b71a524-fc73-4a7c-aae7-252663492939"",
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
                    ""id"": ""939ca86c-1cb0-4d3e-8143-56c8af920670"",
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
                    ""id"": ""adba03f0-bf55-4583-ad20-74bf4e6ea718"",
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
                    ""id"": ""6d04812a-0efd-4e7a-b41e-081851d41101"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""D-Pad"",
                    ""id"": ""661bcfca-b027-4ad4-84a7-b2edd1a1b496"",
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
                    ""id"": ""9c1dcb63-7048-4fa0-9ddc-aeb8e89d21f8"",
                    ""path"": ""<Gamepad>/dpad/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""f3cec023-1f49-4014-b516-4878210ed89e"",
                    ""path"": ""<Gamepad>/dpad/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""fbea760a-b650-46d3-9582-89fb76d9578b"",
                    ""path"": ""<Gamepad>/dpad/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""813392ea-73c8-49aa-aa82-9f05b0da2f4b"",
                    ""path"": ""<Gamepad>/dpad/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""7195705e-36b1-4fb2-86c9-e5cf5aec0ef9"",
                    ""path"": ""<Keyboard>/r"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Rewind"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""9ff9f1c5-041f-4f61-a694-6c667ab575fe"",
                    ""path"": ""<Gamepad>/buttonEast"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Rewind"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""9fbf0749-fe0c-4360-87d8-6fc33cb55861"",
                    ""path"": ""<Keyboard>/x"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Restart"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""9d3a211a-0517-4e49-b96e-0cde9e3dceac"",
                    ""path"": ""<Gamepad>/buttonNorth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Restart"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""be93a79b-baf0-46e2-bf4f-e533b4fc5bd0"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Click"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b4b25746-b65b-413e-b01e-833f2653afc3"",
                    ""path"": ""<Keyboard>/delete"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Debug Delete Save Data"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""9084f9ca-7e5f-400b-b0fe-8fe0747f00ab"",
                    ""path"": ""<Keyboard>/c"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Debug Complete Level"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b8f5d0be-3a97-40c8-b431-720fd2816e05"",
                    ""path"": ""<Keyboard>/1"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Debug Key 1"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b86ac861-fde8-458d-a07b-e861a6913238"",
                    ""path"": ""<Keyboard>/2"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Debug Key 2"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c4ef3048-61a5-4f5d-bc1a-d35b05cd827d"",
                    ""path"": ""<Keyboard>/3"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Debug Key 3"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""490b77b6-d16c-4f4a-8c95-701292fd5a8b"",
                    ""path"": ""<Keyboard>/4"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Debug Key 4"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""fc2d8dec-7bfe-4694-9b09-5dfe1342c8eb"",
                    ""path"": ""<Keyboard>/m"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Debug Next Level"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c2822041-f7e9-41a8-92e0-90848a21638e"",
                    ""path"": ""<Keyboard>/n"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Debug Prev Level"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""770c976a-3f03-4d3e-9a83-a1975dce2dfa"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Quit Game"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b7a2751c-1206-4386-80b4-72ad08b7ddc1"",
                    ""path"": ""<Keyboard>/z"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Debug Key Z"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Gamepad"",
            ""bindingGroup"": ""Gamepad"",
            ""devices"": [
                {
                    ""devicePath"": ""<Gamepad>"",
                    ""isOptional"": true,
                    ""isOR"": false
                }
            ]
        },
        {
            ""name"": ""Keyboard"",
            ""bindingGroup"": ""Keyboard"",
            ""devices"": [
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": true,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // Gameplay
        m_Gameplay = asset.FindActionMap("Gameplay", throwIfNotFound: true);
        m_Gameplay_Movement = m_Gameplay.FindAction("Movement", throwIfNotFound: true);
        m_Gameplay_Rewind = m_Gameplay.FindAction("Rewind", throwIfNotFound: true);
        m_Gameplay_Restart = m_Gameplay.FindAction("Restart", throwIfNotFound: true);
        m_Gameplay_Click = m_Gameplay.FindAction("Click", throwIfNotFound: true);
        m_Gameplay_DebugDeleteSaveData = m_Gameplay.FindAction("Debug Delete Save Data", throwIfNotFound: true);
        m_Gameplay_DebugCompleteLevel = m_Gameplay.FindAction("Debug Complete Level", throwIfNotFound: true);
        m_Gameplay_DebugKey1 = m_Gameplay.FindAction("Debug Key 1", throwIfNotFound: true);
        m_Gameplay_DebugKey2 = m_Gameplay.FindAction("Debug Key 2", throwIfNotFound: true);
        m_Gameplay_DebugKey3 = m_Gameplay.FindAction("Debug Key 3", throwIfNotFound: true);
        m_Gameplay_DebugKey4 = m_Gameplay.FindAction("Debug Key 4", throwIfNotFound: true);
        m_Gameplay_DebugNextLevel = m_Gameplay.FindAction("Debug Next Level", throwIfNotFound: true);
        m_Gameplay_DebugPrevLevel = m_Gameplay.FindAction("Debug Prev Level", throwIfNotFound: true);
        m_Gameplay_QuitGame = m_Gameplay.FindAction("Quit Game", throwIfNotFound: true);
        m_Gameplay_DebugKeyZ = m_Gameplay.FindAction("Debug Key Z", throwIfNotFound: true);
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

    // Gameplay
    private readonly InputActionMap m_Gameplay;
    private IGameplayActions m_GameplayActionsCallbackInterface;
    private readonly InputAction m_Gameplay_Movement;
    private readonly InputAction m_Gameplay_Rewind;
    private readonly InputAction m_Gameplay_Restart;
    private readonly InputAction m_Gameplay_Click;
    private readonly InputAction m_Gameplay_DebugDeleteSaveData;
    private readonly InputAction m_Gameplay_DebugCompleteLevel;
    private readonly InputAction m_Gameplay_DebugKey1;
    private readonly InputAction m_Gameplay_DebugKey2;
    private readonly InputAction m_Gameplay_DebugKey3;
    private readonly InputAction m_Gameplay_DebugKey4;
    private readonly InputAction m_Gameplay_DebugNextLevel;
    private readonly InputAction m_Gameplay_DebugPrevLevel;
    private readonly InputAction m_Gameplay_QuitGame;
    private readonly InputAction m_Gameplay_DebugKeyZ;
    public struct GameplayActions
    {
        private @GameControls m_Wrapper;
        public GameplayActions(@GameControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Movement => m_Wrapper.m_Gameplay_Movement;
        public InputAction @Rewind => m_Wrapper.m_Gameplay_Rewind;
        public InputAction @Restart => m_Wrapper.m_Gameplay_Restart;
        public InputAction @Click => m_Wrapper.m_Gameplay_Click;
        public InputAction @DebugDeleteSaveData => m_Wrapper.m_Gameplay_DebugDeleteSaveData;
        public InputAction @DebugCompleteLevel => m_Wrapper.m_Gameplay_DebugCompleteLevel;
        public InputAction @DebugKey1 => m_Wrapper.m_Gameplay_DebugKey1;
        public InputAction @DebugKey2 => m_Wrapper.m_Gameplay_DebugKey2;
        public InputAction @DebugKey3 => m_Wrapper.m_Gameplay_DebugKey3;
        public InputAction @DebugKey4 => m_Wrapper.m_Gameplay_DebugKey4;
        public InputAction @DebugNextLevel => m_Wrapper.m_Gameplay_DebugNextLevel;
        public InputAction @DebugPrevLevel => m_Wrapper.m_Gameplay_DebugPrevLevel;
        public InputAction @QuitGame => m_Wrapper.m_Gameplay_QuitGame;
        public InputAction @DebugKeyZ => m_Wrapper.m_Gameplay_DebugKeyZ;
        public InputActionMap Get() { return m_Wrapper.m_Gameplay; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(GameplayActions set) { return set.Get(); }
        public void SetCallbacks(IGameplayActions instance)
        {
            if (m_Wrapper.m_GameplayActionsCallbackInterface != null)
            {
                @Movement.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnMovement;
                @Movement.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnMovement;
                @Movement.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnMovement;
                @Rewind.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnRewind;
                @Rewind.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnRewind;
                @Rewind.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnRewind;
                @Restart.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnRestart;
                @Restart.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnRestart;
                @Restart.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnRestart;
                @Click.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnClick;
                @Click.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnClick;
                @Click.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnClick;
                @DebugDeleteSaveData.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnDebugDeleteSaveData;
                @DebugDeleteSaveData.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnDebugDeleteSaveData;
                @DebugDeleteSaveData.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnDebugDeleteSaveData;
                @DebugCompleteLevel.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnDebugCompleteLevel;
                @DebugCompleteLevel.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnDebugCompleteLevel;
                @DebugCompleteLevel.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnDebugCompleteLevel;
                @DebugKey1.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnDebugKey1;
                @DebugKey1.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnDebugKey1;
                @DebugKey1.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnDebugKey1;
                @DebugKey2.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnDebugKey2;
                @DebugKey2.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnDebugKey2;
                @DebugKey2.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnDebugKey2;
                @DebugKey3.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnDebugKey3;
                @DebugKey3.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnDebugKey3;
                @DebugKey3.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnDebugKey3;
                @DebugKey4.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnDebugKey4;
                @DebugKey4.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnDebugKey4;
                @DebugKey4.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnDebugKey4;
                @DebugNextLevel.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnDebugNextLevel;
                @DebugNextLevel.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnDebugNextLevel;
                @DebugNextLevel.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnDebugNextLevel;
                @DebugPrevLevel.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnDebugPrevLevel;
                @DebugPrevLevel.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnDebugPrevLevel;
                @DebugPrevLevel.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnDebugPrevLevel;
                @QuitGame.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnQuitGame;
                @QuitGame.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnQuitGame;
                @QuitGame.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnQuitGame;
                @DebugKeyZ.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnDebugKeyZ;
                @DebugKeyZ.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnDebugKeyZ;
                @DebugKeyZ.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnDebugKeyZ;
            }
            m_Wrapper.m_GameplayActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Movement.started += instance.OnMovement;
                @Movement.performed += instance.OnMovement;
                @Movement.canceled += instance.OnMovement;
                @Rewind.started += instance.OnRewind;
                @Rewind.performed += instance.OnRewind;
                @Rewind.canceled += instance.OnRewind;
                @Restart.started += instance.OnRestart;
                @Restart.performed += instance.OnRestart;
                @Restart.canceled += instance.OnRestart;
                @Click.started += instance.OnClick;
                @Click.performed += instance.OnClick;
                @Click.canceled += instance.OnClick;
                @DebugDeleteSaveData.started += instance.OnDebugDeleteSaveData;
                @DebugDeleteSaveData.performed += instance.OnDebugDeleteSaveData;
                @DebugDeleteSaveData.canceled += instance.OnDebugDeleteSaveData;
                @DebugCompleteLevel.started += instance.OnDebugCompleteLevel;
                @DebugCompleteLevel.performed += instance.OnDebugCompleteLevel;
                @DebugCompleteLevel.canceled += instance.OnDebugCompleteLevel;
                @DebugKey1.started += instance.OnDebugKey1;
                @DebugKey1.performed += instance.OnDebugKey1;
                @DebugKey1.canceled += instance.OnDebugKey1;
                @DebugKey2.started += instance.OnDebugKey2;
                @DebugKey2.performed += instance.OnDebugKey2;
                @DebugKey2.canceled += instance.OnDebugKey2;
                @DebugKey3.started += instance.OnDebugKey3;
                @DebugKey3.performed += instance.OnDebugKey3;
                @DebugKey3.canceled += instance.OnDebugKey3;
                @DebugKey4.started += instance.OnDebugKey4;
                @DebugKey4.performed += instance.OnDebugKey4;
                @DebugKey4.canceled += instance.OnDebugKey4;
                @DebugNextLevel.started += instance.OnDebugNextLevel;
                @DebugNextLevel.performed += instance.OnDebugNextLevel;
                @DebugNextLevel.canceled += instance.OnDebugNextLevel;
                @DebugPrevLevel.started += instance.OnDebugPrevLevel;
                @DebugPrevLevel.performed += instance.OnDebugPrevLevel;
                @DebugPrevLevel.canceled += instance.OnDebugPrevLevel;
                @QuitGame.started += instance.OnQuitGame;
                @QuitGame.performed += instance.OnQuitGame;
                @QuitGame.canceled += instance.OnQuitGame;
                @DebugKeyZ.started += instance.OnDebugKeyZ;
                @DebugKeyZ.performed += instance.OnDebugKeyZ;
                @DebugKeyZ.canceled += instance.OnDebugKeyZ;
            }
        }
    }
    public GameplayActions @Gameplay => new GameplayActions(this);
    private int m_GamepadSchemeIndex = -1;
    public InputControlScheme GamepadScheme
    {
        get
        {
            if (m_GamepadSchemeIndex == -1) m_GamepadSchemeIndex = asset.FindControlSchemeIndex("Gamepad");
            return asset.controlSchemes[m_GamepadSchemeIndex];
        }
    }
    private int m_KeyboardSchemeIndex = -1;
    public InputControlScheme KeyboardScheme
    {
        get
        {
            if (m_KeyboardSchemeIndex == -1) m_KeyboardSchemeIndex = asset.FindControlSchemeIndex("Keyboard");
            return asset.controlSchemes[m_KeyboardSchemeIndex];
        }
    }
    public interface IGameplayActions
    {
        void OnMovement(InputAction.CallbackContext context);
        void OnRewind(InputAction.CallbackContext context);
        void OnRestart(InputAction.CallbackContext context);
        void OnClick(InputAction.CallbackContext context);
        void OnDebugDeleteSaveData(InputAction.CallbackContext context);
        void OnDebugCompleteLevel(InputAction.CallbackContext context);
        void OnDebugKey1(InputAction.CallbackContext context);
        void OnDebugKey2(InputAction.CallbackContext context);
        void OnDebugKey3(InputAction.CallbackContext context);
        void OnDebugKey4(InputAction.CallbackContext context);
        void OnDebugNextLevel(InputAction.CallbackContext context);
        void OnDebugPrevLevel(InputAction.CallbackContext context);
        void OnQuitGame(InputAction.CallbackContext context);
        void OnDebugKeyZ(InputAction.CallbackContext context);
    }
}
