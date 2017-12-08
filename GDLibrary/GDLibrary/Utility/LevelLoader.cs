using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace GDLibrary
{
    internal class PrimitiveObjectParameters
    {
        #region Fields
        public PrimitiveObject primitiveObject;
        public Color diffuseColor;
        public float alpha;
        public Texture2D texture;
        public Vector3 rotation;
        public Vector3 scale;
        #endregion

        #region Properties

        #endregion

        public PrimitiveObjectParameters(PrimitiveObject primitiveObject, 
            Color diffuseColor, float alpha, Texture2D texture, Vector3 rotation, Vector3 scale)
        {
   
        }

    }

    public class LevelLoader
    {
        #region Fields
        private Dictionary<Color, PrimitiveObject> colorToPrimitiveObjectDictionary;
        private Texture2D texture;
        #endregion

        #region Properties
        public Dictionary<Color, PrimitiveObject> ColorToPrimitiveObjectDictionary
        {
            get
            {
                return this.colorToPrimitiveObjectDictionary;
            }
        }
        #endregion

        public LevelLoader(Game game, Texture2D texture)
        {
            //this.texture = texture;

            ////reference to dictionary filled with object prototypes to be used when creating new objects
            //this.colorToPrimitiveObjectDictionary = colorToPrimitiveObjectDictionary;
        }






        //reads through each pixel in a PNG texture (ignoring ColorToIgnore in a pixel) and generates an object based on the users GetActorFromPixelColor() method implementation - see MyLevelLoader
        protected virtual List<Actor3D> Process(Vector2 scaleOnXZPlane, float yAxisHeight, Vector3 worldOffset)
        {
            List<Actor3D> list = new List<Actor3D>();
            Color[] colorData = new Color[texture.Height * texture.Width];
            texture.GetData(colorData);

            Color color; 
            Vector3 position;
            Actor3D actor;

            for (int y = 0; y < texture.Height; y++)
            {
                for (int x = 0; x < texture.Width; x++)
                {
                    color = colorData[x + y * texture.Width];

                    if (!colorToPrimitiveObjectDictionary.ContainsKey(color))
                    {
                        //scale allows us to increase the separation between objects in the XZ plane
                        position = new Vector3(x * scaleOnXZPlane.X, yAxisHeight, y * scaleOnXZPlane.Y);

                        //offset allows us to shift the whole set of objects in X, Y, and Z      
                        position += worldOffset;

                        actor = GetActorFromPixelColor(color, position);

                        if (actor != null)
                            list.Add(actor);
                    }
                } //end for x
            } //end for y
            return list;
        }

        //returns a reference to a new actor based on the current pixel color and the if...else logic of this method - see MyLevelLoader::GetActorFromPixelColor()
        protected virtual Actor3D GetActorFromPixelColor(Color color, Vector3 position)
        {
            Actor3D clonedActor = this.ColorToPrimitiveObjectDictionary[color].Clone() as Actor3D;
            clonedActor.Transform.Translation = position;
            return clonedActor;
        }


    }
}
