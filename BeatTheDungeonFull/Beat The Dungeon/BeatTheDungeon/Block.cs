using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

//Group 1
//Stores a block that will be used for obstacles/walls

namespace BeatTheDungeon
{
    /// <summary>
    /// store 5 type of block
    /// </summary>
    enum BlockType
    {
        None,
        Block,
        Spown,
        Fire,
        Chest,
        Door
    }

    /// <summary>
    /// create a block object
    /// </summary>
    internal class Block : GameObject
    {
        //Fields---------
        private Texture2D texture;
        private Rectangle rectangle;
        private Color color;
        private BlockType type;
        private bool ifHaveCollision;

        /// <summary>
        /// set the texture of the block
        /// </summary>
        public Texture2D Texture { get => texture; set => texture = value; }

        /// <summary>
        /// get the rectangle
        /// </summary>
        public Rectangle Rectangle { get => rectangle; }

        /// <summary>
        /// get and set the color (red for detection purposes
        /// </summary>
        public Color Color { get => color; set => color = value; }

        /// <summary>
        /// get and set the block type
        /// </summary>
        public BlockType Type { get => type; set => type = value; }

        /// <summary>
        /// get and set if a block have collision
        /// </summary>
        public bool IfHaveCollision { get => ifHaveCollision; set => ifHaveCollision = value; }



        /// <summary>
        /// create a block in gungeon
        /// make the block for texting purpose
        /// </summary>
        /// <param name="texture"> the texture of block</param>
        /// <param name="rectangle"> the shape of the block</param>
        public Block(Texture2D texture, Rectangle rectangle, BlockType type) : base(texture, rectangle)
        {
            this.texture = texture;
            this.rectangle = rectangle;
            this.color = Color.Transparent;
            this.type = type;
            if (type == BlockType.Spown ||
                type == BlockType.Fire ||
                type == BlockType.Chest)
            {
                ifHaveCollision = false;
            }
            else
            {
                ifHaveCollision = true;
            }
        }
    }

}
