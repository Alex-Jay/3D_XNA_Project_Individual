﻿/*
Function: 		Stores and organises the cameras available within the game (used single and split screen layouts) 
                WORK IN PROGRESS - at present this class is only a barebones class to be used by the ObjectManager 
Author: 		NMCG
Version:		1.0
Date Updated:	
Bugs:			None
Fixes:			None
*/

using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace GDLibrary
{
    public class CameraManager : GameComponent
    {

        #region Variables
        private Camera3D activeCamera;
        private List<Camera3D> cameraList;
        private int activeCameraIndex = -1;
        #endregion

        #region Properties
        public Camera3D ActiveCamera
        {
            get
            {
                return this.cameraList[this.activeCameraIndex];
            }
        }
        public int ActiveCameraIndex
        {
            get
            {
                return this.activeCameraIndex;
            }
            set
            {
                this.activeCameraIndex = (value >= 0 && value <= this.cameraList.Count) ? value : 0;
            }
        }
        #endregion

        public CameraManager(Game game, int initialSize) : base(game)
        {
            this.cameraList = new List<Camera3D>(initialSize);
        }

        public void Add(Camera3D camera)
        {
            //first time in ensures that we have a default active camera
            if (this.cameraList.Count == 0)
                this.activeCameraIndex = 0;

            this.cameraList.Add(camera);
        }

        public bool Remove(Predicate<Camera3D> predicate)
        {
            Camera3D foundCamera = this.cameraList.Find(predicate);
            if (foundCamera != null)
                return this.cameraList.Remove(foundCamera);

            return false;
        }

        public int RemoveAll(Predicate<Camera3D> predicate)
        {
            return this.cameraList.RemoveAll(predicate);
        }

        public override void Update(GameTime gameTime)
        {
            /* 
             * Update all the cameras in the list.
             * Remember that at the moment only 1 camera is visible so this foreach loop seems counter-intuitive.
             * Assuming each camera in the list had some form of automatic movement (e.g. like a security camera) then what would happen if we only updated the active camera?
             */
            foreach (Camera3D camera in this.cameraList)
                camera.Update(gameTime);

            base.Update(gameTime);
        }
    }
}
