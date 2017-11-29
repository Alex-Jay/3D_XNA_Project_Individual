﻿/*
Function: 		Allows us to draw primitives by explicitly defining the vertex data.
                Used in you I-CA project.
                 
Author: 		NMCG
Version:		1.0
Date Updated:	27/11/17
Bugs:			None
Fixes:			None
*/
namespace GDLibrary
{
    public class PrimitiveObject : DrawnActor3D
    {
        #region Variables
        private IVertexData vertexData;
        #endregion 

        #region Properties
        public IVertexData VertexData
        {
            get
            {
                return this.vertexData;
            }
            set
            {
                this.vertexData = value;
            }
        }
        #endregion

        public PrimitiveObject(string id, ActorType actorType, Transform3D transform, 
            EffectParameters effectParameters, StatusType statusType, IVertexData vertexData) 
            : base(id, actorType, transform, effectParameters, statusType)
        {
            this.vertexData = vertexData;
        }

        public new object Clone()
        {
            return new PrimitiveObject("clone - " + ID, //deep
               this.ActorType, //deep
               (Transform3D)this.Transform.Clone(), //deep
               (EffectParameters)this.EffectParameters.Clone(), //deep
               this.StatusType, //deep
               this.vertexData); //shallow - its ok if objects refer to the same vertices
        }
    }
}
