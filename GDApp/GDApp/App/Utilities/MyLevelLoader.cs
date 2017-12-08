using System.Collections.Generic;
using GDLibrary;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GDApp
{
    public class MyLevelLoader : LevelLoader
    {
        public MyLevelLoader(Game game, Texture2D texture) 
            : base(game, texture)
        {
        }

        protected override Actor3D GetActorFromPixelColor(Color color, Vector3 position)
        {
            //IVertexData vertexData = null;
            //PrimitiveType primitiveType;
            //int primitiveCount;
            //Transform3D transform3D = null;

            Actor3D actor = null;

            ////if the pixel is red then draw a tall (stretched cube)
            //if (color.Equals(new Color(255, 0, 0)))
            //{
            //    //get the box data from the factory
            //    vertexData = new VertexData<VertexPositionNormalTexture>(VertexFactory.GetVerticesPositionNormalTexturedCube(1,
            //        out primitiveType, out primitiveCount), primitiveType, primitiveCount);

            //    //lets say that red pixels make stretched boxes so
            //    Vector3 scale = new Vector3(1, 5, 1);

            //    //set the position of the stretched box
            //    transform3D = new Transform3D(position, scale);

            //    BasicEffectParameters clonedEffectParameters = (BasicEffectParameters)this.archetypeEffectParameters.Clone();
            //    clonedEffectParameters.Texture = this.TextureDictionary["crate1"];
            //    actor = new PrimitiveObject("1st lit cube",
            //        ActorType.Decorator,
            //        transform3D,
            //        clonedEffectParameters,
            //        StatusType.Drawn | StatusType.Update,
            //        vertexData);
            //}
            //add an else if for each type of object that you want to load...
            return actor;
        }
    }
}
