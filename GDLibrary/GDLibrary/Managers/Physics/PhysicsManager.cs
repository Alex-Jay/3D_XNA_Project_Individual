﻿/*
Function: 		Enables CDCR through JibLibX by integrating forces applied to each collidable object within the scene
Author: 		NMCG
Version:		1.0
Date Updated:	27/10/17
Bugs:			
Fixes:			None
*/

using JigLibX.Collision;
using JigLibX.Physics;
using Microsoft.Xna.Framework;
using System;

namespace GDLibrary
{
    public class PhysicsManager : PausableGameComponent
    {
        #region Fields
        private PhysicsSystem physicSystem;
        private PhysicsController physCont;
        private float timeStep = 0;
        #endregion

        #region Properties
        public PhysicsSystem PhysicsSystem
        {
            get
            {
                return physicSystem;
            }
        }
        public PhysicsController PhysicsController
        {
            get
            {
                return physCont;
            }
        }
        #endregion

        public PhysicsManager(Game game, EventDispatcher eventDispatcher, StatusType statusType)
            : base(game, eventDispatcher, statusType)
        {
            this.physicSystem = new PhysicsSystem();

            //add cd/cr system
            this.physicSystem.CollisionSystem = new CollisionSystemSAP();
            this.physicSystem.EnableFreezing = true;
            this.physicSystem.SolverType = PhysicsSystem.Solver.Normal;
            this.physicSystem.CollisionSystem.UseSweepTests = true;
            //affect accuracy and the overhead == time required
            this.physicSystem.NumCollisionIterations = 8; //8
            this.physicSystem.NumContactIterations = 8; //8
            this.physicSystem.NumPenetrationRelaxtionTimesteps = 12; //15

            #region SETTING_COLLISION_ACCURACY
            //affect accuracy of the collision detection
            this.physicSystem.AllowedPenetration = 0.000025f;
            this.physicSystem.CollisionTollerance = 0.00005f;
            #endregion

            this.physCont = new PhysicsController();
            this.physicSystem.AddController(physCont);

        }

        #region Event Handling
        protected override void RegisterForEventHandling(EventDispatcher eventDispatcher)
        {
            eventDispatcher.RemoveActorChanged += EventDispatcher_RemoveActorChanged;
            base.RegisterForEventHandling(eventDispatcher);
        }

        private void EventDispatcher_RemoveActorChanged(EventData eventData)
        {
            if (eventData.EventType == EventActionType.OnRemoveActor)
            {
                //using the "sender" property of the event to pass reference to object to be removed - use "as" to access Body since sender is defined as a raw object.
                CollidableObject collidableObject = eventData.Sender as CollidableObject;
                //what would happen if we did not remove the physics body? would the CD/CR skin remain?
                this.PhysicsSystem.RemoveBody(collidableObject.Body);
            }
        }

        //See MenuManager::EventDispatcher_MenuChanged to see how it does the reverse i.e. they are mutually exclusive
        protected override void EventDispatcher_MenuChanged(EventData eventData)
        {
            //did the event come from the main menu and is it a start game event
            if (eventData.EventType == EventActionType.OnStart)
            {
                //turn on update and draw i.e. hide the menu
                this.StatusType = StatusType.Update | StatusType.Drawn;
            }
            //did the event come from the main menu and is it a start game event
            else if (eventData.EventType == EventActionType.OnPause)
            {
                //turn off update and draw i.e. show the menu since the game is paused
                this.StatusType = StatusType.Off;
            }
        }
        #endregion

        protected override void ApplyUpdate(GameTime gameTime)
        {
            timeStep = (float)gameTime.ElapsedGameTime.Ticks / TimeSpan.TicksPerSecond;
            //if the time between updates indicates a FPS of close to 60 fps or less then update CD/CR engine
            if (timeStep < 1.0f / 60.0f)
                physicSystem.Integrate(timeStep);
            else
                //else fix at 60 updates per second
                physicSystem.Integrate(1.0f / 60.0f);
        }

        //to do - dispose
    }
}
