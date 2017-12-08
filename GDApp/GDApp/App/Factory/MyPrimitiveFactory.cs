using System;
using GDLibrary;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace GDApp
{
    public class MyPrimitiveFactory : PrimitiveFactory
    {
        #region Fields
        //re-used local vars inside methods
        private PrimitiveType primitiveType;
        private int primitiveCount;
        private IVertexData vertexData;
        #endregion

        public PrimitiveObject GetArchetypePrimitiveObject(GraphicsDevice graphics, ShapeType shapeType, EffectParameters effectParameters)
        {
            PrimitiveObject primitiveObject = null;

            //add a Get...() method for each of the UNIQUE shapes that you have in your game
            switch (shapeType)
            {
                case ShapeType.WireframeLine:
                    primitiveObject = (this.PrimitiveDictionary.ContainsKey(shapeType)) ? this.PrimitiveDictionary[shapeType] : GetLine(graphics, shapeType, effectParameters);
                    break;

                case ShapeType.WireframeOrigin:
                    primitiveObject = (this.PrimitiveDictionary.ContainsKey(shapeType)) ? this.PrimitiveDictionary[shapeType] : GetOrigin(graphics, shapeType, effectParameters);
                    break;

                case ShapeType.ColoredQuad:
                    primitiveObject = (this.PrimitiveDictionary.ContainsKey(shapeType)) ? this.PrimitiveDictionary[shapeType] : GetColoredQuad(graphics, shapeType, effectParameters);
                    break;

                case ShapeType.TexturedQuad:
                    primitiveObject = (this.PrimitiveDictionary.ContainsKey(shapeType)) ? this.PrimitiveDictionary[shapeType] : GetTexturedQuad(graphics, shapeType, effectParameters);
                    break;

                case ShapeType.TexturedCube:
                    primitiveObject = (this.PrimitiveDictionary.ContainsKey(shapeType)) ? this.PrimitiveDictionary[shapeType] : GetTexturedCube(graphics, shapeType, effectParameters);
                    break;

                default:
                    break;

            }

            return primitiveObject;
        }

        private PrimitiveObject GetColoredQuad(GraphicsDevice graphics, ShapeType shapeType, EffectParameters effectParameters)
        {
            //vertex colors for the quad
            Color[] vertexColorArray = { Color.Red, Color.Green, Color.Blue, Color.Orange };

            //get the vertices
            VertexPositionColor[] vertices = PrimitiveUtility.GetColoredQuad(vertexColorArray, out primitiveType, out primitiveCount);

            //create the buffered data
            vertexData = new BufferedVertexData<VertexPositionColor>(graphics, vertices, primitiveType, primitiveCount);

            //instanciate the object and return a reference
            return GetPrimitiveObjectFromVertexData(vertexData, shapeType, effectParameters);
        }

        private PrimitiveObject GetLine(GraphicsDevice graphics, ShapeType shapeType, EffectParameters effectParameters)
        {
            //get the vertices
            VertexPositionColor[] vertices = PrimitiveUtility.GetWireframLine(out primitiveType, out primitiveCount);

            //create the buffered data
            vertexData = new BufferedVertexData<VertexPositionColor>(graphics, vertices, primitiveType, primitiveCount);

            //instanciate the object and return a reference
            return GetPrimitiveObjectFromVertexData(vertexData, shapeType, effectParameters);
        }

        private PrimitiveObject GetTexturedQuad(GraphicsDevice graphics, ShapeType shapeType, EffectParameters effectParameters)
        {
            //get the vertices
            VertexPositionColorTexture[] vertices = PrimitiveUtility.GetTexturedQuad(out primitiveType, out primitiveCount);

            //create the buffered data
            vertexData = new BufferedVertexData<VertexPositionColorTexture>(graphics, vertices, primitiveType, primitiveCount);

            //instanciate the object and return a reference
            return GetPrimitiveObjectFromVertexData(vertexData, shapeType, effectParameters);
        }

        private PrimitiveObject GetTexturedCube(GraphicsDevice graphics, ShapeType shapeType, EffectParameters effectParameters)
        {
            //get the vertices
            VertexPositionColorTexture[] vertices = PrimitiveUtility.GetTexturedCube(out primitiveType, out primitiveCount);

            /* 
             * Remember we saw in PrimitiveUtility.GetTexturedCube() that we were using too many vertices? 
             * We can use IndexedBufferedVertexData to now reduce the 36 vertices to 12.
             * 
             * Notice however that we are STILL using TriangleList as our PrimitiveType. The optimal solution is to use an IndexedBufferedVertexData object where you define the indices and vertices
             * and also choose TriangleStrip or LineStrip as your PrimitiveType.
             */

            vertexData = new IndexedBufferedVertexData<VertexPositionColorTexture>(graphics, vertices, primitiveType, primitiveCount);

            //instanciate the object and return a reference
            return GetPrimitiveObjectFromVertexData(vertexData, shapeType, effectParameters);
        }

        private PrimitiveObject GetOrigin(GraphicsDevice graphics, ShapeType shapeType, EffectParameters effectParameters)
        {
            //get the vertices
            VertexPositionColor[] vertices = PrimitiveUtility.GetWireframeOrigin(out primitiveType, out primitiveCount);

            //create the buffered data
            vertexData = new BufferedVertexData<VertexPositionColor>(graphics, vertices, primitiveType, primitiveCount);

            //instanciate the object and return a reference
            return GetPrimitiveObjectFromVertexData(vertexData, shapeType, effectParameters);
        }
    }
}
