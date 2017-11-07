﻿/*
Function: 		Provide mouse input functions
Author: 		NMCG
Version:		1.0
Date Updated:	17/8/17
Bugs:			None
Fixes:			None
*/

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;

namespace GDLibrary
{
    public class MouseManager : GameComponent
    {
        #region Fields
        private MouseState newState, oldState;
        #endregion

        #region Properties
        public Microsoft.Xna.Framework.Rectangle Bounds
        {
            get
            {
                return new Microsoft.Xna.Framework.Rectangle(this.newState.X, this.newState.Y, 1, 1);
            }
        }
        public Vector2 Position
        {
            get
            {
                return new Vector2(this.newState.X, this.newState.Y);
            }
        }
        #endregion

        public MouseManager(Game game, bool isVisible)
            : base(game)
        {
            game.IsMouseVisible = isVisible;
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            // TODO: Add your initialization code here

            base.Initialize();
        }
     
        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            //store the old state
            this.oldState = newState;

            //get the new state
            this.newState = Mouse.GetState();

            base.Update(gameTime);
        }

        public bool HasMoved(float mouseSensitivity)
        {
            float deltaPositionLength = new Vector2(newState.X - oldState.X, 
                newState.Y - oldState.Y).Length();

            return (deltaPositionLength > mouseSensitivity) ? true : false;
        }

        public bool IsLeftButtonClickedOnce()
        {
            return ((newState.LeftButton.Equals(ButtonState.Pressed)) && (!oldState.LeftButton.Equals(ButtonState.Pressed)));
        }

        public bool IsLeftButtonClicked()
        {
            return (newState.LeftButton.Equals(ButtonState.Pressed));
        }

        public bool IsRightButtonClickedOnce()
        {
            return ((newState.RightButton.Equals(ButtonState.Pressed)) && (!oldState.RightButton.Equals(ButtonState.Pressed)));
        }

        public bool isRightButtonClicked()
        {
            return (newState.RightButton.Equals(ButtonState.Pressed));
        }

        ////Calculates the mouse pointer distance (in X and Y) from a user-defined position
        //public Vector2 GetDeltaFromPosition(Vector2 position, Camera3D activeCamera)
        //{
        //    Vector2 delta;
        //    if (this.Position != position) //e.g. not the centre
        //    {
                
        //        if (activeCamera.View.Up.Y == -1)
        //        {
        //            delta.X = 0;
        //            delta.Y = 0;
        //        }
        //        else
        //        {
        //            delta.X = this.Position.X - position.X;
        //            delta.Y = this.Position.Y - position.Y;
        //        }
        //        SetPosition(position);
        //        return delta;
        //    }
        //    return Vector2.Zero;
        //}

        //Calculates the mouse pointer distance from the screen centre
        public Vector2 GetDeltaFromCentre(Vector2 screenCentre)
        {
            return new Vector2(this.newState.X - screenCentre.X, this.newState.Y - screenCentre.Y);
        }

        //has the mouse state changed since the last update?
        public bool IsStateChanged()
        {
            return (this.newState.Equals(oldState)) ? false : true;
        }

        //did the mouse move above the limits of precision from centre position
        public bool IsStateChangedOutsidePrecision(float mousePrecision)
        {
            return ((Math.Abs(newState.X - oldState.X) > mousePrecision) || (Math.Abs(newState.Y - oldState.Y) > mousePrecision));
        }


        //how much has the scroll wheel been moved since the last update?
        public int GetDeltaFromScrollWheel()
        {
            if (IsStateChanged()) //if state changed then return difference
                return newState.ScrollWheelValue - oldState.ScrollWheelValue;

            return 0;
        }

        public void SetPosition(Vector2 position)
        {
            Mouse.SetPosition((int)position.X, (int)position.Y);
        }

        //tests if mouse on the screen vertical screen edge
        public CompassDirectionType GetCompassDirection(float padding, Rectangle screenRectangle)
        {
            CompassDirectionType compassDirectionType = CompassDirectionType.Centre;
            //left
            if (this.newState.X <= screenRectangle.Left)
            {
                compassDirectionType = CompassDirectionType.West;
            }
            else if (this.newState.X > screenRectangle.Right)
            {
                compassDirectionType = CompassDirectionType.East;
            }

            if (this.newState.Y <= screenRectangle.Top)
            {
                compassDirectionType |= CompassDirectionType.North;
            }
            else if (this.newState.Y > screenRectangle.Bottom)
            {
                compassDirectionType |= CompassDirectionType.South;
            }

            return compassDirectionType;
        }
    }
}