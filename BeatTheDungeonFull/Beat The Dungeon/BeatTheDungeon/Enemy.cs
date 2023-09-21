using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
//using System.Drawing;

namespace BeatTheDungeon
{
    internal class Enemy : GameObject
    {
        private int health;
        private Vector2 playerPosition;
        private Vector2 enemyPosition;
        private float speed;
        private float defaultSpeed;
        private SpriteEffects enemyDirection;
        private int numSpritesInSheet;
        private int widthOfSingleSprite;
        private Texture2D texture;
        private int currentFrame;
        private double fps;
        private double secondsPerFrame;
        private double timeCounter;
        private Rectangle enemyHitBox;
        private bool isDead;
        private bool isTakingDamage;
        private Vector2 lastPosition;
        private float enemySize;
        private int damage;
        private List<Projectile> projectiles;
        private double shotTimer;
        private double shotCooldown;

        private Projectile projectile;
        // add by Yu Ma
        private int spawnTime;
        private Random random;
        private float trackDistance;

        private Dungeon dungeon;

        /// <summary>
        /// Gets and sets the enemy position.
        /// </summary>
        public Vector2 EnemyPosition { get => enemyPosition; set => enemyPosition = value; }
        public Vector2 ProjectilePosition { get; set; }
        public override Rectangle Rectangle { get => enemyHitBox; }
        public Vector2 LastPosition { get => lastPosition; }
        public float Speed { get => speed; set => speed = value; }
        public int Damage { get; set; }

        /// <summary>
        /// Enemy constructor.
        /// </summary>
        /// <param name="enemyTexture">Texture of the enemy</param>
        /// <param name="enemyRectangle">Rectangle of the enemy</param>
        /// <param name="speed">Enemy speed</param>
        /// <param name="playerPos">The players current position</param>
        public Enemy(Texture2D enemyTexture, Rectangle enemyRectangle, float speed, Vector2 playerPos,
            int widthOfHitBox, int heightOfHitBox, float enemySize,
            int health, float trackDistance, int damage,int numOfSpritSheet, Dungeon dungeon)
            : base(enemyTexture, enemyRectangle)
        {
            Damage = damage;
            this.health = health;
            this.speed = speed;
            this.trackDistance = trackDistance;
            defaultSpeed = speed;
            playerPosition = playerPos;
            EnemyPosition = new Vector2(enemyRectangle.X, enemyRectangle.Y);
            ProjectilePosition = new Vector2(enemyRectangle.X, enemyRectangle.Y);
            enemyHitBox = new Rectangle(enemyRectangle.X, enemyRectangle.Y, widthOfHitBox, heightOfHitBox);
            enemyDirection = SpriteEffects.None;
            texture = enemyTexture;
            numSpritesInSheet = numOfSpritSheet;
            widthOfSingleSprite = texture.Width / numSpritesInSheet;
            currentFrame = 1;
            fps = 10.0;
            secondsPerFrame = 1.0f / fps;
            timeCounter = 0;
            isDead = false;
            isTakingDamage = false;
            lastPosition = new Vector2(0, 0);
            this.enemySize = enemySize;
            random = new Random();
            this.trackDistance = trackDistance;
            projectiles = new List<Projectile>();
            this.dungeon = dungeon;

        }

        /// <summary>
        /// Moves the enemy towards the player.
        /// </summary>
        /// <param name="gameTime">GameTime</param>
        /// <param name="player">The enemy player.</param>
        public override void Update(GameTime gameTime, Player player)
        {
            //get distance between player and enemy
            float distance = Vector2.Distance(playerPosition, enemyPosition);
            //get the direction 
            Vector2 direction = playerPosition - enemyPosition;
            //make direction stay same value
            direction.Normalize();
            //need a fsm to make the enemy can flip 
            lastPosition = enemyPosition;
            playerPosition = player.PlayerCenter;
            float absoluteDistance = Math.Abs(distance);
            if (absoluteDistance < trackDistance)
            {
                //track enemy and make enemy can flip according to the player position

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
                

                enemyPosition += direction * speed;

            }
            enemyHitBox.X = (int)enemyPosition.X;
            enemyHitBox.Y = (int)enemyPosition.Y;
            speed = defaultSpeed;



            isTakingDamage = false;
            UpdateAnimation(gameTime);

            foreach (Block b in dungeon.Blocks)
            {
                this.Collision(b);
            }
        }
        /// <summary>
        /// Updates the animation of the enemy.
        /// </summary>
        /// <param name="gameTime">GameTime</param>
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
        /// check the collison with enemy
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public override bool Collision(GameObject otherObject)
        {
            if (base.Collision(otherObject) && otherObject is Block)
            {
                Block other = (Block)otherObject;

                if (other.Type == BlockType.Block)
                {

                    Rectangle overlap = Rectangle.Intersect(enemyHitBox, other.Rectangle);

                    //if the height is greater than or equal to the width, move the player on the X axis opposite to its collision
                    if (overlap.Height >= overlap.Width)
                    {
                        if (enemyHitBox.X > other.Rectangle.X)
                        {
                            enemyPosition.X += overlap.Width;
                        }
                        if (enemyHitBox.X < other.Rectangle.X)
                        {
                            enemyPosition.X -= overlap.Width;
                        }
                    }

                    //if the width is greater than or equal to the height, move the player on the Y axis opposite to its collision
                    //also note that if the width is small enough do not count the collision. this stops random y collisions when hitting walls.
                    if (overlap.Width > overlap.Height && overlap.Width > 5)
                    {
                        if (enemyHitBox.Y > other.Rectangle.Y)
                        {
                            enemyPosition.Y += overlap.Height;
                        }
                        if (enemyHitBox.Y < other.Rectangle.Y)
                        {
                            enemyPosition.Y -= overlap.Height;
                        }
                    }

                    //sets the position to that of the playerRect
                    enemyHitBox.X = (int)enemyPosition.X;
                    enemyHitBox.Y = (int)enemyPosition.Y;
                }
            }

            return base.Collision(otherObject);
        }
        /// <summary>
        /// enemy take damage and drop item
        /// </summary>
        /// <param name="damage"></param>
        /// <param name="textures"></param>
        public void enemyTakeDamage(int damage, Texture textures)
        {
            health -= damage;
            isTakingDamage = true;
            // if enemy dead randomly drop life or rejuvinating
            if (health <= 0)
            {
                Random rng = new Random();

                int num = rng.Next(0, 9);

                if (num == 0)
                {
                    Potion p;
                    int numTwo = rng.Next(0, 5);

                    switch (numTwo)
                    {
                        case 0:
                            {
                                p = new Potion(PotionType.Life, textures, (int)EnemyPosition.X, (int)EnemyPosition.Y);
                                break;
                            }
                        default:
                            {
                                p = new Potion(PotionType.Rejuvinating, textures, (int)EnemyPosition.X, (int)EnemyPosition.Y);
                                break;
                            }
                    }

                    dungeon.Potions.Add(p);

                }


                isDead = true;
            }
        }
        public bool IsDead()
        {
            return isDead;
        }
        /// <summary>
        /// Draws the enemy.
        /// </summary>
        /// <param name="sb">SpriteBatch</param>
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
                enemySize,
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
                enemySize,
                enemyDirection,
                0.0f);
            }
        }
    }
}
