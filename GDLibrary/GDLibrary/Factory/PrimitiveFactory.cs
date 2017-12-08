using System.Collections.Generic;

namespace GDLibrary
{

    public class PrimitiveFactory
    {

        #region Fields
        private Dictionary<ShapeType, PrimitiveObject> primitiveDictionary;
        #endregion

        #region Properties
        public Dictionary<ShapeType, PrimitiveObject> PrimitiveDictionary
        {
            get
            {
                return this.primitiveDictionary;
            }
        }
        #endregion


        public PrimitiveFactory()
        {
            this.primitiveDictionary = new Dictionary<ShapeType, PrimitiveObject>();
        }

        protected PrimitiveObject GetPrimitiveObjectFromVertexData(IVertexData vertexData, ShapeType shapeType, EffectParameters effectParameters)
        {
            //instanicate the object
            PrimitiveObject primitiveObject = new PrimitiveObject("Archetype - " + shapeType.ToString(), ActorType.NotYetAssigned, Transform3D.Zero, effectParameters.Clone() as EffectParameters, StatusType.Update | StatusType.Drawn, vertexData);

            //add to the dictionary for re-use in any subsequent call to this method
            primitiveDictionary.Add(shapeType, primitiveObject);

            //return a reference to our new object
            return primitiveObject;
        }
    }
}
