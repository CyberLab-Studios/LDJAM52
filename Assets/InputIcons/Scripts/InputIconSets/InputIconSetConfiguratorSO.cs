using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using TMPro;

namespace InputIcons
{
    [CreateAssetMenu(fileName = "Input Icon Set Configurator", menuName = "Input Icon Set/InputIconSetConfigurator", order = 503)]
    public class InputIconSetConfiguratorSO : ScriptableObject
    {

        public static InputIconSetConfiguratorSO instance;
        public static InputIconSetConfiguratorSO Instance
        {
            get
            {
                if (instance != null)
                    return instance;
                else
                {
                    InputIconSetConfiguratorSO iconManager = Resources.Load("InputIcons/InputIconSetConfigurator") as InputIconSetConfiguratorSO;
                    if (iconManager)
                        instance = iconManager;

                    return instance;
                }
            }
            set => instance = value;
        }

        private static InputIconSetBasicSO currentIconSet;
        public delegate void OnIconSetUpdated();
        public static OnIconSetUpdated onIconSetUpdated;

        public InputIconSetBasicSO keyboardIconSet;
        public InputIconSetBasicSO ps3IconSet;
        public InputIconSetBasicSO ps4IconSet;
        public InputIconSetBasicSO ps5IconSet;
        public InputIconSetBasicSO switchIconSet;
        public InputIconSetBasicSO xBoxIconSet;

        public InputIconSetBasicSO overwriteIconSet;

        public InputIconSetBasicSO fallbackGamepadIconSet;

        public DisconnectedSettings disconnectedDeviceSettings;

        private void Awake()
        {
            instance = this;
        }

        [System.Serializable]
        public struct DeviceSet
        {
            public string deviceRawPath;
            public InputIconSetBasicSO iconSetSO;
        }

        [System.Serializable]
        public struct DisconnectedSettings
        {
            public string disconnectedDisplayName;
            public Color disconnectedDisplayColor;
        }

        public static void UpdateCurrentIconSet()
        {
            PlayerInput playerInput = InputIconsManagerSO.GetCurrentPlayerInput();
            currentIconSet = GetIconSet(playerInput);
            onIconSetUpdated?.Invoke();
        }

        public static void SetCurrentIconSet(InputIconSetBasicSO iconSet)
        {
            if (iconSet == null)
                return;

            currentIconSet = iconSet;
        }

        public static InputIconSetBasicSO GetCurrentIconSet()
        {
            if (currentIconSet == null) UpdateCurrentIconSet();


            return currentIconSet;
        }

        public static InputIconSetBasicSO GetIconSet(PlayerInput playerInput)
        {

            if (playerInput == null)
                return Instance.keyboardIconSet;

            if (playerInput.devices.Count == 0)
                return Instance.keyboardIconSet;

            InputDevice activeDevice = playerInput.devices[0];
            //Debug.Log(activeDevice.displayName);
            
            if(activeDevice is Gamepad)
            {
                if (Instance.overwriteIconSet != null) //if overwriteIconSet is not null, this set will be used for all gamepads
                    return Instance.overwriteIconSet;

                if (activeDevice is UnityEngine.InputSystem.XInput.XInputController)
                {
                    return Instance.xBoxIconSet;
                }

#if !UNITY_STANDALONE_LINUX && !UNITY_WEBGL
                if (activeDevice is UnityEngine.InputSystem.DualShock.DualShock3GamepadHID)
                {
                    return Instance.ps3IconSet;
                }

                if (activeDevice is UnityEngine.InputSystem.DualShock.DualShock4GamepadHID)
                {
                    return Instance.ps4IconSet;
                }

                if (activeDevice is UnityEngine.InputSystem.DualShock.DualSenseGamepadHID) //Input System 1.2.0 or higher required (package manager dropdown menu -> see other versions)
                {
                    return Instance.ps5IconSet;
                }

                if (activeDevice is UnityEngine.InputSystem.Switch.SwitchProControllerHID)
                {
                    return Instance.switchIconSet;
                }
#endif

                if (activeDevice is UnityEngine.InputSystem.DualShock.DualShockGamepad)
                {
                    return Instance.ps4IconSet;
                }

                if (activeDevice.name.Contains("DualShock3"))
                    return Instance.ps3IconSet;

                if (activeDevice.name.Contains("DualShock4"))
                    return Instance.ps4IconSet;

                if (activeDevice.name.Contains("DualSense"))
                    return Instance.ps5IconSet;

                if (activeDevice.name.Contains("ProController"))
                    return Instance.switchIconSet;
            }
           

            //in case it is none of the above gamepads, return fallback icons
            if(activeDevice is Gamepad)
            {
                return Instance.fallbackGamepadIconSet;
            }

            return Instance.keyboardIconSet;
        }

        public static InputIconSetBasicSO GetIconSet(string deviceDisplayName)
        {
            if (Instance.keyboardIconSet.deviceDisplayName == deviceDisplayName)
            {
                return Instance.keyboardIconSet;
            }

            if (Instance.ps3IconSet.deviceDisplayName == deviceDisplayName)
            {
                return Instance.ps3IconSet;
            }

            if (Instance.ps4IconSet.deviceDisplayName == deviceDisplayName)
            {
                return Instance.ps4IconSet;
            }

            if (Instance.ps5IconSet.deviceDisplayName == deviceDisplayName)
            {
                return Instance.ps5IconSet;
            }

            if (Instance.switchIconSet.deviceDisplayName == deviceDisplayName)
            {
                return Instance.switchIconSet;
            }

            if (Instance.xBoxIconSet.deviceDisplayName == deviceDisplayName)
            {
                return Instance.xBoxIconSet;
            }

            return null;
        }

        public static string GetCurrentDeviceName()
        {
            return GetCurrentIconSet().deviceDisplayName;
        }

        public static Color GetCurrentDeviceColor()
        {
            return GetCurrentIconSet().deviceDisplayColor;
        }

        public static string GetDisconnectedName()
        {
            return Instance.disconnectedDeviceSettings.disconnectedDisplayName;
        }

        public static Color GetDisconnectedColor()
        {
            return Instance.disconnectedDeviceSettings.disconnectedDisplayColor;
        }


    }
}