using Microsoft.Xna.Framework.Graphics;
using System;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace BeatTheDungeon
{
    internal class EnemyBullet : GameObject
    {
        private Vector2 direction;
        private float speed;
        private Rectangle bulletHitbox;
        private Texture2D bulletTexture;
        protected Vector2 bulletPosition;
        public float Speed { get => speed; set => speed = value; }
        public override Rectangle Rectangle
        {
            get => bulletHitbox;
        }

        public EnemyBullet(Texture2D texture, Vector2 position, Vector2 target, float speed) 
            : base(texture, new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height))
        {
            direction = target - position;
            direction.Normalize();
            this.speed = speed;
            bulletTexture = texture;
            bulletPosition = position;
            bulletHitbox = new Rectangle((int)position.X, (int)position.Y, 10, 10);
        }
        /// <summary>
        /// update the bullet pisition
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            // Move the bullet in the direction it was fired
            bulletPosition += direction * speed;
            bulletHitbox.X = (int)bulletPosition.X;
            bulletHitbox.Y = (int)bulletPosition.Y;

        }
        /// <summary>
        /// check the collision
        /// </summary>
        /// <param name="otherObject"></param>
        /// <returns></returns>
        public override bool Collision(GameObject otherObject)
        {
            return base.Collision(otherObject);
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(bulletTexture, bulletHitbox, Color.White);
        }
    }
}
