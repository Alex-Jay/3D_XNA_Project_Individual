﻿/*
Function: 		Represents a moveable, collectable and collidable object within the game that can be picked up (e.g. ammo that can fall off a table) 
Author: 		NMCG
Version:		1.0
Date Updated:	13/11/17
Bugs:			None
Fixes:			None
*/
using GDLibrary;
using JigLibX.Collision;
using Microsoft.Xna.Framework.Graphics;

namespace GDLibrary
{
    public class MoveablePickupObject : CollidableObject
    {
        #region Fields
        private PickupParameters pickupParameters;
        #endregion

        #region Properties
        public PickupParameters PickupParameters
        {
            get
            {
                return this.pickupParameters;
            }
            set
            {
                this.pickupParameters = value;
            }
        }
        #endregion

        public MoveablePickupObject(string id, ActorType actorType, Transform3D transform, Effect effect,
            ColorParameters colorParameters, Texture2D texture, Model model, PickupParameters pickupParameters)
            : base(id, actorType, transform, effect, colorParameters, texture, model)
        {
            this.pickupParameters = pickupParameters;

            //register for callback on CDCR
            this.Body.CollisionSkin.callbackFn += CollisionSkin_callbackFn;
        }

        #region Event Handling
        protected virtual bool CollisionSkin_callbackFn(CollisionSkin collider, CollisionSkin collidee)
        {
            HandleCollisions(collider.Owner.ExternalData as CollidableObject, collidee.Owner.ExternalData as CollidableObject);
            return true;
        }
        //how do we want this object to respond to collisions?
        private void HandleCollisions(CollidableObject collidableObjectCollider, CollidableObject collidableObjectCollidee)
        {
            //add your response code here...
        }
        #endregion
    }
}
