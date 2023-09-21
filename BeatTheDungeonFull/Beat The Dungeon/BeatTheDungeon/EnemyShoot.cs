using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace BeatTheDungeon
{
    internal class EnemyShoot : GameObject
    {
      
        private Texture2D enemyTexture;
        private Texture2D bulletTexture;
        private int health;
        private int currentFrame;
        private double fps;
        private double secondsPerFrame;
        private double timeCounter;
     
        private bool isDead;
        private bool isTakingDamage;
        private int numSpritesInSheet;
        private int widthOfSingleSprite;

        private Vector2 playerPosition;
        private Vector2 enemyPosition;
        private Rectangle enemyHitBox;

        private SpriteEffects enemyDirection;

        public override Rectangle Rectangle { get => enemyHitBox;}
        public Vector2 EnemyPosition { get => enemyPosition; set => enemyPosition = value; }

        private float speed;
        private Player player;
        private List<EnemyBullet> bullets;
        private float fireInterval; // Time between each shot
        private float timeSinceLastShot;

        private Dungeon dungeon;


        List<EnemyBullet> bulletsToRemove = new List<EnemyBullet>();

        // Constructors
        public EnemyShoot(Texture2D texture,Texture2D bulletTexture, Vector2 position, Vector2 playerPos, Dungeon dungeon)
            : base(texture, new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height))
        {
            playerPosition = playerPos;
            enemyTexture = texture;
            this.bulletTexture = bulletTexture;
            health = 10;
            numSpritesInSheet = 4;
            widthOfSingleSprite = texture.Width / numSpritesInSheet;
            currentFrame = 1;
            fps = 10.0;
            secondsPerFrame = 1.0f / fps;
            timeCounter = 0;
            isDead = false;
            isTakingDamage = false;
            EnemyPosition = position;
            enemyHitBox = new Rectangle((int)enemyPosition.X + 10, (int)enemyPosition.Y + 10, 24, 24);
            enemyDirection = SpriteEffects.None;
            bullets = new List<EnemyBullet>();
            this.speed = 3;
            this.fireInterval = 0.8f;
            timeSinceLastShot = 0;
            this.dungeon = dungeon;
        }

        // Methods
        public override void Update(GameTime gameTime, Player player)
        {
            playerPosition = player.PlayerCenter;
            Vector2 direction = playerPosition - enemyPosition;
            direction.Normalize();
            // Check if it's time to fire a bullet
            timeSinceLastShot += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (timeSinceLastShot >= fireInterval)
            {
                FireBullet();
                timeSinceLastShot = 0;
            }
            ///check coliision
            foreach (EnemyBullet bullet in bullets)
            {
               //with player
                if (bullet.Collision(player))
                {
                    player.PlayerTakeDamageFromBullets(bullet);
                    bulletsToRemove.Add(bullet);
                }
                //check collision with block
                foreach (Block b in dungeon.Blocks)
                {
                    if(b.Type == BlockType.Block && bullet.Collision(b))
                    {
                        bulletsToRemove.Add(bullet);
                    }
                }

                bullet.Update(gameTime);
            }
            foreach (EnemyBullet b in bulletsToRemove)
            {
                bullets.Remove(b);  
            }

            //flip enemy
            if (direction.X <= 0)
            {
                //if x direction less than 0 flip
                enemyDirection = SpriteEffects.FlipHorizontally;
            }
            else
            {
                //if not don't flip
                enemyDirection = SpriteEffects.None;
            }
            isTakingDamage = false;         
            UpdateAnimation(gameTime);
        }
        /// <summary>
        /// create bullet and add to the list
        /// </summary>
        private void FireBullet()
        {
            // Create a new bullet and add it to the list of bullets fired by the enemy
            EnemyBullet bullet = new EnemyBullet(bulletTexture, EnemyPosition, playerPosition, 6);
            bullets.Add(bullet);
        }
        /// <summary>
        /// the animation of enemy
        /// </summary>
        /// <param name="gameTime"></param>
        private void UpdateAnimation(GameTime gameTime)
        {
            //total time 
            timeCounter += gameTime.ElapsedGameTime.TotalSeconds;

            if (timeCounter >= secondsPerFrame)
            {
                //get to next frome according to the time
                currentFrame++;
                //let the sprite loop 
                if (currentFrame >= numSpritesInSheet)
                    currentFrame = 1;


                timeCounter -= secondsPerFrame;
            }

        }
        /// <summary>
        /// shoot enemy take damage
        /// </summary>
        /// <param name="damage"></param>
        public void ShooteEnemyTakeDamage(int damage)
        {
            health -= damage;
            isTakingDamage = true;
            if (health <= 0)
            {
                isDead = true;
            }
        }
        /// <summary>
        /// the shoot enemy is dead method rerturn id dead or not
        /// </summary>
        /// <returns></returns>
        public bool ShootEnemyIsDead()
        {
            return isDead;
        }
        /// <summary>
        /// draw enemy and if enemy get hit turn red at that second 
        /// also draw bullet
        /// </summary>
        /// <param name="sb"></param>
        public override void Draw(SpriteBatch sb)
        {
            
            if (isTakingDamage)
            {
                sb.Draw(
                Texture,
                enemyPosition,
                new Rectangle(widthOfSingleSprite * currentFrame, 0, widthOfSingleSprite, Texture.Height),
                Color.Red,
                0.0f,
                Vector2.Zero,
                0.3f,
                enemyDirection,
                0.0f);
            }
            else
            {
                sb.Draw(
                Texture,
                enemyPosition,
                new Rectangle(widthOfSingleSprite * currentFrame, 0, widthOfSingleSprite, Texture.Height),
                Color.White,
                0.0f,
                Vector2.Zero,
                0.3f,
                enemyDirection,
                0.0f);
            }
            foreach (EnemyBullet bullet in bullets)
            {
                bullet.Draw(sb);
            }
        }
    }
}
