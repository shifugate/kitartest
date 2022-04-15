using KitAR.Helper.UI;
using KitAR.Manager.Language.Token;
using KitAR.Manager.Setting;
using KitAR.Manager.System;
using KitAR.Util;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace KitAR.UI._Screen.Setting
{
    public class SettingUI : MonoBehaviour
    {

        [SerializeField]
        private Toggle fpsToggle;
        [SerializeField]
        private InputFieldHelper sizeInputField;
        [SerializeField]
        private InputFieldHelper anchorReachInputField;

        private void Awake()
        {
            AddListener();
            SetProperties();
        }

        private void OnDestroy()
        {
            RemoveListener();
        }

        private void AddListener()
        {
            EventUtil.Setting.SaveComplete += SaveComplete;
        }

        private void RemoveListener()
        {
            EventUtil.Setting.SaveComplete -= SaveComplete;
        }

        private void SetProperties()
        {
            sizeInputField.SetLabel(LanguageManagerToken.common.size_token
                .Replace("%s1", SettingManager.Instance.DataCurrent.room_size.ToString())
                .Replace("%s2", SettingManager.Instance.DataCurrent.room_max_size.ToString()));

            anchorReachInputField.SetLabel(LanguageManagerToken.common.anchor_reach_token
                .Replace("%s1", SettingManager.Instance.DataCurrent.anchor_reach.ToString())
                .Replace("%s2", SettingManager.Instance.DataCurrent.anchor_max_reach.ToString()));

            fpsToggle.isOn = SettingManager.Instance.DataCurrent.show_fps;
            sizeInputField.text = SettingManager.Instance.DataCurrent.room_size.ToString();
            anchorReachInputField.text = SettingManager.Instance.DataCurrent.anchor_reach.ToString();
        }

        public void FPSToggleAction(bool enable)
        {
            SystemManager.Instance?.FPSEnableAction(enable);
        }

        private void SaveComplete()
        {
            EventUtil.Screen.LoadScreen?.Invoke(ContentUtil.Constant.Screen.Anchor);
        }

        private bool ValidForm()
        {
            bool valid = true;
            int size;
            int anchorReach;

            sizeInputField.SetError(null);
            anchorReachInputField.SetError(null);

            if (!int.TryParse(sizeInputField.text, out size))
            {
                sizeInputField.SetError(LanguageManagerToken.common.invalid_size_token
                    .Replace("%s1", SettingManager.Instance.DataDefault.room_size.ToString())
                    .Replace("%s2", SettingManager.Instance.DataCurrent.room_max_size.ToString()));

                valid = false;
            }

            if (size < SettingManager.Instance.DataDefault.room_size
                || size > SettingManager.Instance.DataDefault.room_max_size)
            {
                sizeInputField.SetError(LanguageManagerToken.common.invalid_size_token
                    .Replace("%s1", SettingManager.Instance.DataDefault.room_size.ToString())
                    .Replace("%s2", SettingManager.Instance.DataCurrent.room_max_size.ToString()));

                valid = false;
            }

            if (!int.TryParse(anchorReachInputField.text, out anchorReach))
            {
                anchorReachInputField.SetError(LanguageManagerToken.common.invalid_anchor_reach_token
                    .Replace("%s1", SettingManager.Instance.DataDefault.anchor_reach.ToString())
                    .Replace("%s2", SettingManager.Instance.DataCurrent.anchor_max_reach.ToString()));

                valid = false;
            }

            if (anchorReach < SettingManager.Instance.DataDefault.anchor_reach
                || anchorReach > SettingManager.Instance.DataDefault.anchor_max_reach)
            {
                anchorReachInputField.SetError(LanguageManagerToken.common.invalid_anchor_reach_token
                    .Replace("%s1", SettingManager.Instance.DataDefault.anchor_reach.ToString())
                    .Replace("%s2", SettingManager.Instance.DataCurrent.anchor_max_reach.ToString()));

                valid = false;
            }

            return valid;
        }

        public void StartAction()
        {
            if (!ValidForm())
                return;

            SettingManager.Instance.DataCurrent.show_fps = fpsToggle.isOn;
            SettingManager.Instance.DataCurrent.room_size = int.Parse(sizeInputField.text);
            SettingManager.Instance.DataCurrent.anchor_reach = int.Parse(anchorReachInputField.text);
            SettingManager.Instance.SaveAction();
        }
    }
}
