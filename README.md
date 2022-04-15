# kitartest

## InitializerManager.cs
It's will start all managers when start the app.

## SettingManager.cs
It's Will load the setting from persistence directory if exists otherwise It's will create a default setting in persistence directory.

## SystemManager.cs
It's Will set max frame rate and enable or disable FPS counter based in settings data.

## LanguageManager.cs
It's Will load the language files to be used by the app.

## AnchorManager.cs
It's Will control the anchors and room creation.
It's will dispatch events about the room creation, valid position of reticle and player in room and the info about reticle and anchor collision.

## SettingUI.cs
Screen that's the user input the settings about room and anchor creation.
When the user click in start button and all values are valid the SettingManager.Instance.SaveAction() will be called and the new settings will be save.

## AnchorInfoUI.cs
Screen with infos about interface.
The screen listener the AnchorManager events for set reticle swap between red (invalid reticle or player position) and green (valid reticle and player position) and show or hide anchor info.
The screen call AnchorManager.Instance.AddAnchor() to add anchor when space key is pressed if reticle and player position is valid.

## HomeController.cs
It's will control the screens load and unload based on EventUtil.Screen.LoadScreen.

## LanguageTool.cs
It's will create the LanguageManagerToken.cs with all language tokens maped to be used by app and will copy the languages to resources.

## AnchorHelper.cs
Anchor class plugged to the anchor prefab with same name to dynamic load.

## CameraHelper.cs
Camera controller that's will be create when AnchorInfoUI started and call AnchorManager.Instance.Enable() and will be destroyed when AnchorInfoUI will be unload and call AnchorManager.Instance.Disable().
