using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeatTheDungeon
{
    internal class EnemyProjectile
    {
        private Texture2D texture;
        private Rectangle rectangle;
        private Vector2 direction;
        private float speed;
        private int damage;

        public Rectangle Rectangle { get { return rectangle; } }
        public int Damage { get { return damage; } }

        public EnemyProjectile(Texture2D texture, Rectangle rectangle, float speed, int damage, Vector2 direction)
        {
            this.texture = texture;
            this.rectangle = rectangle;
            this.speed = speed;
            this.damage = damage;
            this.direction = direction;
            this.direction.Normalize();
        }

        public void Update(GameTime gameTime)
        {
            rectangle.X += (int)(direction.X * speed * (float)gameTime.ElapsedGameTime.TotalSeconds);
            rectangle.Y += (int)(direction.Y * speed * (float)gameTime.ElapsedGameTime.TotalSeconds);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, rectangle, Color.White);
        }
    }
}
