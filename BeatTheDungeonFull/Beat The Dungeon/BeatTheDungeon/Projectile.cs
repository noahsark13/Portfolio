using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace BeatTheDungeon
{
    /// <summary>
    /// Represents a firing projectile.
    /// </summary>
    internal class Projectile : GameObject
    {

        private double speed;
        private double exponentialIncrease;
        private double angle;
        private Vector2 vectorPosition;

        private float scale;
        private float rotation;
        private float timer;

        private int damage;

        private SpriteEffects spriteEffect;

        private Weapon weapon;
        private GameObject sender;

        private Texture2D hitboxTexture;

        /// <summary>
        /// Gets and sets the "timer" value of the individual projectile.
        /// </summary>
        public float Timer { get { return timer; } set { timer = value; } }

        /// <summary>s
        /// Gets and sets damage
        /// </summary>
        public int Damage { get { return damage; } set { damage = value; } }

        /// <summary>
        /// Gets and sets the current SpriteEffect of the projectile.
        /// </summary>
        public SpriteEffects SpriteEffect { get { return spriteEffect; } set { spriteEffect = value; } }

        /// <summary>
        /// Constructor for the projectile object.
        /// </summary>
        /// <param name="hitboxTexture">The texture of the hitbox.</param>
        /// <param name="texture">The actual projectile texture.</param>
        /// <param name="hitbox">The rectangle hitbox that handles collision, and gives initial location.</param>
        /// <param name="angle">The angle to fire the projectile at</param>
        public Projectile(Texture2D hitboxTexture, Texture2D texture, Rectangle hitbox, double angle, double speed, int damage, float timer) : base(texture, hitbox)
        {

            //this.rectangle = rectangle;
            //this.weapon = weapon;

            // Speed and timer will be defined in the parameters of the constructor, however for now it is not.
            this.speed = speed;
            this.angle = angle;
            this.rotation = 0.0f;
            this.timer = timer;
            this.spriteEffect = SpriteEffects.None;
            this.damage = damage;
            exponentialIncrease = 1;

            // May be temp
            this.hitboxTexture = hitboxTexture;


            // Properly determines scale (for vector2 drawing) based on rectangle and texture w & h.
            // Checks for if the width or height of the texture are smaller than another
            if (texture.Width >= texture.Height)
            {
                this.scale = (hitbox.Height / (float)texture.Height);

            }
            // Same as above
            else if (texture.Width < texture.Height)
            {

                this.scale = (hitbox.Width / (float)texture.Width);

            }


            // Vector position based upon rectangle parameter.
            vectorPosition = new Vector2(hitbox.X, hitbox.Y);
        }


        /// <summary>
        /// Handles the actual movement of the projectile. This could be considered
        /// the projectiles "update" method.
        /// </summary>
        public void FireProjectile(WeaponType type)
        {
            if (type == WeaponType.TempestWinds)
            {
                // slow to fast
                vectorPosition.X += (float)(Math.Cos(angle) * speed * exponentialIncrease);
                vectorPosition.Y += (float)(Math.Sin(angle) * speed * exponentialIncrease);
                exponentialIncrease *= 1.1;
            }
            else if (type == WeaponType.ElBombino)
            {
                vectorPosition.X += (float)(Math.Cos(angle) * speed * exponentialIncrease);
                vectorPosition.Y += (float)(Math.Sin(angle) * speed * exponentialIncrease);
                exponentialIncrease *= 1.2;
            }
            else if (type == WeaponType.DivineFist)
            {
                // fast to slow
                vectorPosition.X += (float)(Math.Cos(angle) * speed * exponentialIncrease);
                vectorPosition.Y += (float)(Math.Sin(angle) * speed * exponentialIncrease);
                exponentialIncrease /= 1.1;
            }
            else
            {
                // default fire
                vectorPosition.X += (float)(Math.Cos(angle) * speed);
                vectorPosition.Y += (float)(Math.Sin(angle) * speed);
            }
            // adjusts direction based on the given angle.
            

            // adjusts angle of the texture
            rotation = (float)angle;

            X = (int)vectorPosition.X;
            Y = (int)vectorPosition.Y;

        }

        /// <summary>
        /// Draws the projectile, as well as its hitbox (for now).
        /// </summary>
        /// <param name="sb">SpriteBatch</param>
        public override void Draw(SpriteBatch sb)
        {
            // Draw Hitbox
            /*
            sb.Draw(
                hitboxTexture,
                Rectangle,
                null,
                Color.Red,
                0.0f,
                new Vector2((hitboxTexture.Width) / 2f, (hitboxTexture.Height) / 2f),
                SpriteEffects.None,
                0);
            */

            // Draw Visual Sprite
            sb.Draw(
                Texture,
                vectorPosition,
                null,
                Color.White,
                rotation,
                new Vector2((Texture.Width) / 2f, (Texture.Height) / 2f),
                scale,
                spriteEffect,
                0.0f);

        }



    }
}
