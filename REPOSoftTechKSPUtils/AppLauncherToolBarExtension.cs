/*
 * AppLauncherToolBar.cs
 * (C) Copyright 2016, Jamie Leighton (JPLRepo)
 * REPOSoft Technologies 
 * Kerbal Space Program is Copyright (C) 2013 Squad. See http://kerbalspaceprogram.com/. This
 * project is in no way associated with nor endorsed by Squad.
 *
 *  This file is part of RST Utils. My attempt at creating my own KSP Mod base Architecture.
 *
 *  RST Utils is free software: you can redistribute it and/or modify
 *  it under the terms of the MIT License 
 *
 *  RST Utils is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  
 *
 *  You should have received a copy of the MIT License
 *  along with RST Utils.  If not, see <http://opensource.org/licenses/MIT>.
 *
 */
using KSP.Localization;
using KSP.UI.Screens;
using System;
using ToolbarControl_NS;
using UnityEngine;

namespace RSTUtils
{
    public class AppLauncherToolBar
    {
        public static AppLauncherToolBar Instance { get; private set; }

        private static ApplicationLauncher.AppScenes VisibleinScenes; //What scenes is the applauncher button seen in
        //private bool showHoverText = false; //Whether to show AppLauncher Hover Text or not.

        private bool _gamePaused;
        public Boolean gamePaused
        {
            get { return _gamePaused; }
            private set
            {
                _gamePaused = value;      //Set the private variable
            }
        }

        private bool _hideUI;
        public Boolean hideUI
        {
            get { return _hideUI; }
            private set
            {
                _hideUI = value;      //Set the private variable
            }
        }


        //GuiVisibility
        private bool _Visible;
        public Boolean GuiVisible
        {
            get { return _Visible; }
            set
            {
                _Visible = value;      //Set the private variable
            }
        }

#if false
        public bool ShowHoverText
        {
            get { return showHoverText; }
        }
#endif

        private void GamePaused()
        {
            gamePaused = true;
        }

        private void GameUnPaused()
        {
            gamePaused = false;
        }

        private void onHideUI()
        {
            hideUI = true;
        }

        private void onShowUI()
        {
            hideUI = false;
        }

        /// <summary>
        /// Constructor for AppLauncherToolBar. You need to construct one of these for your Mod Menu/GUI environment.
        /// </summary>
        /// <param name="toolBarName">A string passed into ToolBar indicating the Name of the Mod</param>
        /// <param name="toolBarToolTip">A string passed into ToolBar to use for the Icon ToolTip</param>
        /// <param name="toolBarTexturePath">A string in ToolBar expected format of the TexturePath for the ToolBarIcon</param>
        /// <param name="VisibleinScenes">ApplicationLauncher.AppScenes list - logically OR'd</param>
        /// <param name="appbtnTexON">Texture reference for the AppLauncher ON Icon</param>
        /// <param name="appbtnTexOFF">Texture reference for the AppLauncher OFF Icon</param>
        /// <param name="gameScenes">A list of GameScenes use for ToolBar icon visiblity</param>
        public AppLauncherToolBar(string toolBarName, string toolBarToolTip, string toolBarTexturePath,
            ApplicationLauncher.AppScenes VisibleinScenesValue, UnityEngine.Texture appbtnTexON, UnityEngine.Texture appbtnTexOFF, params GameScenes[] gameScenes)
        {
            if (toolbarControl == null)
            {
                VisibleinScenes = VisibleinScenesValue;
                CreateToolbar();
            }
        }

        public const string MODID = "DeepFreeze";
        public const string MODNAME = "Deep Freeze";
        static ToolbarControl toolbarControl;


        private void CreateToolbar()
        {
            Debug.LogError("DeepFreeze.CreateToolbar 1");
            if (toolbarControl == null)
            {
                Debug.LogError($"DeepFreeze.CreateToolbar 2, VisibleinScenes: {VisibleinScenes.ToString()}");
                GameObject gm = new GameObject(MODID);
                toolbarControl = gm.gameObject.AddComponent<ToolbarControl>();
                toolbarControl.AddToAllToolbars(
                    onAppLaunchToggle,
                    onAppLaunchToggle,
                    VisibleinScenes,
                    MODID,
                    Localizer.Format("#LOC_lTech_16"),
                   "REPOSoftTech/DeepFreeze/Icons/DeepFreezeOn", "REPOSoftTech/DeepFreeze/Icons/DeepFreezeOff",
                    MODNAME
                    );
            }
        }

        public void onAppLaunchToggle()
        {
            GuiVisible = !GuiVisible;
        }

        /// <summary>
        /// This Class is not using MonoBehaviour but has a Start Method that must be called.
        /// Call this in your Start Method for a Mod GUI/Menu Class.
        /// </summary>
        /// <param name="stock">True if we are to use the Stock Applauncher, False to use ToolBar mod</param>
        public void Start(bool stock)
        {
            GameEvents.onGamePause.Add(GamePaused);
            GameEvents.onGameUnpause.Add(GameUnPaused);
            GameEvents.onHideUI.Add(onHideUI);
            GameEvents.onShowUI.Add(onShowUI);
        }

        /// <summary>
        /// This Class is not using MonoBehaviour but has a Destroy Method that must be called.
        /// Call this in your OnDestroy Method for a Mod GUI/Menu Class.
        /// </summary>
        public void Destroy()
        {

            // Stock toolbar
            Utilities.Log_Debug("Removing onGUIAppLauncher callbacks");
            if (GuiVisible) GuiVisible = !GuiVisible;
            GameEvents.onGamePause.Remove(GamePaused);
            GameEvents.onGameUnpause.Remove(GameUnPaused);
            GameEvents.onHideUI.Remove(onHideUI);
            GameEvents.onShowUI.Remove(onShowUI);
        }


        /// <summary>
        /// Sets the Applauncher Icon visible or not. To be extended in future to not require calling from Mod.
        /// Currently it is because I haven't incorporated the mod's Setting for Whether the user wants to use Stock AppLauncher or Toolbar.
        /// </summary>
        /// <param name="visible">True if set to visible, false will turn it off</param>
        public void setAppLSceneVisibility(ApplicationLauncher.AppScenes visibleinScenes)
        {
            Debug.LogError($"DeepFreeze, setAppLSceneVisibility, visibleinScenes: {visibleinScenes.ToString()},  VisibleinScenes: {VisibleinScenes.ToString()}");

            if (VisibleinScenes == visibleinScenes)
                return;
            VisibleinScenes = visibleinScenes;
            Debug.LogError($"DeepFreeze, Trying to recreate button, VisibleinScenes: {VisibleinScenes.ToString()}");
            toolbarControl.OnDestroy();
            UnityEngine.Object.Destroy(toolbarControl);
            toolbarControl = null;
            CreateToolbar();
        }
    }
}
