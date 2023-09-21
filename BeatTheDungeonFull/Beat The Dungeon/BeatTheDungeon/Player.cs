using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;



namespace BeatTheDungeon
{
    /// <summary>
    /// Represents a player in the dungeon.
    /// </summary>
    internal class Player : GameObject
    {
        // Fields to be used later:
        private int health;
        private int maxHealth;

        private int speed;

        private int debugOffset;

        // ---------

        private KeyboardState currentState;
        private KeyboardState oldState;

        private Vector2 playerPosition;
        private Rectangle playerBounds;
        private Rectangle textureBounds;
        private SpriteEffects playerDirection;
        private float playerScale;

        private bool isTakingDamage;

        private Vector2 playerCenter;

        private Weapon currentWeapon;
        private Artifact currentArtifact;

        Dungeon dungeon;

        //field for animation
        private int currentFrame;
        private double fps;
        private double secondsPerFrame;
        private double timeCounter;
        private int numSpritesInSheet;
        private int widthOfSingleSprite;
        private Vector2 oldPosition;

        /// <summary>
        /// Gets player postion based on the normal origin of the texture.
        /// </summary>
        public Vector2 PlayerPosition { get => playerPosition; }

        /// <summary>
        /// Gets the players center point.
        /// </summary>
        public Vector2 PlayerCenter { get => playerCenter; }

        /// <summary>
        /// Gets the current player direction
        /// </summary>
        public SpriteEffects PlayerDirection { get => playerDirection; }

        /// <summary>
        /// Gets the current player hitbox
        /// </summary>
        public override Rectangle Rectangle { get => playerBounds; }

        /// <summary>
        /// Gets and Sets the speed of the player
        /// </summary>
        public int Speed { get => speed; set => speed = value; }

        /// <summary>
        /// Gets the offset for the player hitbox
        /// </summary>
        public int DebugOffset { get => debugOffset; }

        /// <summary>
        /// Gets and sets the current weapon of the player
        /// </summary>
        public Weapon Weapon { get => currentWeapon; set => currentWeapon = value; }

        /// <summary>
        /// Gets and sets the current artifact of the player
        /// </summary>
        public Artifact Artifact { get => currentArtifact; set => currentArtifact = value; }

        /// <summary>
        /// Gets and sets the current player health
        /// </summary>
        public int Health { get => health; set => health = value; }

        /// <summary>
        /// Gets and sets the current player max health
        /// </summary>
        public int MaxHealth { get => maxHealth; set => maxHealth = value; }

        /// <summary>
        /// Default constructor for the player object.
        /// </summary>
        /// <param name="texture">The texture of the player</param>
        /// <param name="rectangle">Rectangle of the player sprite</param>
        public Player(Texture2D texture, Rectangle rectangle, Rectangle textureBounds, Dungeon dungeon, Weapon currentWeapon, Artifact currentArtifact)
            : base(texture, rectangle)
        {
            //sets the player fields to their default/parameter values
            health = 100;
            maxHealth = 100;
            speed = 5;

            debugOffset = 20;

            playerPosition = new Vector2(rectangle.X, rectangle.Y);
            playerDirection = SpriteEffects.None;
            playerScale = 3.0f;

            playerBounds = new Rectangle(rectangle.X, rectangle.Y + debugOffset, rectangle.Width * 3, (rectangle.Height - 6) * 3);
            this.textureBounds = textureBounds;

            playerCenter = new Vector2(playerPosition.X + (playerScale * Width / 2), playerPosition.Y + (playerScale * Height / 2));

            this.dungeon = dungeon;
            this.currentWeapon = currentWeapon;
            this.currentArtifact = currentArtifact;

            //fields used for animation
            numSpritesInSheet = 9;
            widthOfSingleSprite = texture.Width / numSpritesInSheet;
            currentFrame = 1;
            fps = 10.0;
            secondsPerFrame = 1.0f / fps;
            timeCounter = 0;
        }

        /// <summary>
        /// Run's every time the game updates. Handles player movement, rotation, and block collision. 
        /// </summary>
        /// <param name="gameTime">The time of the game</param>
        public void Update(GameTime gameTime, Dungeon dungeon, DungeonManager dm)
        {
            //sets the mouse state
            MouseState ms = Mouse.GetState();

            //updates the vector value of the center of the player
            playerCenter = new Vector2(playerPosition.X + (playerScale * Width / 2), playerPosition.Y + (playerScale * Height / 2));

            MovePlayer();
            RotatePlayer();


            //handles the cooldowns of the artifact and weapon
            if(currentArtifact != null)
            {
                if (currentArtifact.OnCooldown)
                {
                    currentArtifact.CurrentCooldown -= gameTime.ElapsedGameTime.TotalSeconds;

                    if (currentArtifact.CurrentCooldown <= 0)
                    {
                        currentArtifact.OnCooldown = false;
                        currentArtifact.CurrentCooldown = currentArtifact.CooldownDuration;
                    }
                }
            }

            if (currentWeapon.OnCooldown)
            {
                currentWeapon.CurrentCooldown -= gameTime.ElapsedGameTime.TotalSeconds;

                if (currentWeapon.CurrentCooldown <= 0)
                {
                    currentWeapon.OnCooldown = false;
                    currentWeapon.CurrentCooldown = currentWeapon.CooldownDuration;
                }
            }

            // checks the collisions with various game objects
            foreach (Block b in dungeon.Blocks)
            {
                if (b.IfHaveCollision == true)
                {
                    this.Collision(b);
                }
            }
            if (dm.PreviousDungeon != null)
            {
                foreach (Block b in dm.PreviousDungeon.Blocks)
                {
                    if (b.IfHaveCollision == true)
                    {
                        this.Collision(b);
                    }
                }
            }
            if (dm.NextDungeon != null)
            {
                foreach (Block b in dm.NextDungeon.Blocks)
                {
                    if (b.IfHaveCollision == true)
                    {
                        this.Collision(b);
                    }
                }
            }

            foreach (Enemy e in dungeon.Enemies)
            {
                this.Collision(e);
            }
            foreach (Chest c in dungeon.Chests)
            {
                this.Collision(c);
            }
            for (int i = 0; i < dungeon.Potions.Count; i++)
            {
                dungeon.Potions[i].UsePotion(dungeon, this);
            }
            UpdateAnimation(gameTime);
            oldPosition = playerPosition;
        }

        /// <summary>
        /// WIP: Will most likely implement what we have in main code for
        /// projectile handling.
        /// </summary>
        public void Attack(List<Projectile> projectiles, Dungeon dungeon)
        {

            currentWeapon.UseWeapon(projectiles, this, dungeon);

        }

        /// <summary>
        /// Uses the current artifact of the player
        /// </summary>
        /// <param name="gameTime">gets the current gametime</param>
        public void UseArtifact(GameTime gameTime)
        {
            //current keyboard state
            currentState = Keyboard.GetState();

            //checks for a single keypress and then uses the artifact
            if (SingleKeyPress(Keys.F) && currentArtifact != null && currentArtifact.InUse == false && currentArtifact.CurrentCooldown == currentArtifact.CooldownDuration)
            {
                currentArtifact.Use(this);
            }

            //when the artifact is in use, update its usage timer
            if(currentArtifact != null)
            {
                if (currentArtifact.InUse)
                {
                    currentArtifact.BuffTimer -= gameTime.ElapsedGameTime.TotalSeconds;

                    //once the usage timer has finished, start the artifact's cooldown
                    if (currentArtifact.BuffTimer <= 0)
                    {
                        currentArtifact.BuffTimer = currentArtifact.BuffDuration;
                        currentArtifact.Reset(this);
                        currentArtifact.OnCooldown = true;
                    }
                }
            }

            //old keyboard state is updated
            oldState = currentState;
        }

        /// <summary>
        /// Gets the angle based on the mouse location, and the center point (the player).
        /// </summary>
        /// <returns>The angle in radians.</returns>
        public double GetAngle(int xOffset, int yOffset)
        {
            MouseState ms = Mouse.GetState();

            //subtracts the camera offset from the mouse position
            double mouseX = ms.X + xOffset + (Width / 2 * 3) - 400;
            double mouseY = ms.Y + yOffset + (Height / 2 * 3) - 300;


            return Math.Atan2(mouseY, mouseX);
        }


        /// <summary>
        /// Handles player movement.
        /// </summary>
        public void MovePlayer()
        {
            KeyboardState kb = Keyboard.GetState();

            //moves the player based on its speed and keyboard input
            if (kb.IsKeyDown(Keys.W))
            {
                playerPosition.Y -= speed;

            }
            if (kb.IsKeyDown(Keys.S))
            {
                playerPosition.Y += speed;

            }
            if (kb.IsKeyDown(Keys.A))
            {
                playerPosition.X -= speed;
            }
            if (kb.IsKeyDown(Keys.D))
            {
                playerPosition.X += speed;
            }

            // Adjusts player hitbox accordingly.
            playerBounds.X = (int)playerPosition.X;
            playerBounds.Y = (int)playerPosition.Y + debugOffset;
        }

        /// <summary>
        /// Swaps the direction the player is looking depending on if the mouse is on the left
        /// or right side of the screen.
        /// </summary>
        public void RotatePlayer()
        {
            MouseState ms = Mouse.GetState();

            //subtracts the camera offset from the mouse position
            int mouseStateX = ms.X - ((int)-playerPosition.X - Width / 2 * 3) - 400;

            if (mouseStateX > playerCenter.X)
            {
                playerDirection = SpriteEffects.None;
            }
            else if (mouseStateX < playerCenter.X)
            {
                playerDirection = SpriteEffects.FlipHorizontally;
            }
        }

        /// <summary>
        /// Handles player collision with other gameObjects.
        /// </summary>
        /// <param name="other">A object the player is colliding with</param>
        /// <returns>True if the player and the object are colliding, false otherwise.</returns>
        public override bool Collision(GameObject other)
        {
            if (base.Collision(other) && other is Block)
            {
                Rectangle overlap = Rectangle.Intersect(playerBounds, other.Rectangle);

                //if the height is greater than or equal to the width, move the player on the X axis opposite to its collision
                if (overlap.Height >= overlap.Width)
                {
                    if (playerBounds.X > other.Rectangle.X)
                    {
                        playerPosition.X += overlap.Width;
                    }
                    if (playerBounds.X < other.Rectangle.X)
                    {
                        playerPosition.X -= overlap.Width;
                    }
                }

                //if the width is greater than or equal to the height, move the player on the Y axis opposite to its collision
                //also note that if the width is small enough do not count the collision. this stops random y collisions when hitting walls.
                if (overlap.Width > overlap.Height && overlap.Width > 5)
                {
                    if (playerBounds.Y > other.Rectangle.Y)
                    {
                        playerPosition.Y += overlap.Height;
                    }
                    if (playerBounds.Y < other.Rectangle.Y)
                    {
                        playerPosition.Y -= overlap.Height;
                    }
                }

                //sets the position to that of the playerRect
                playerBounds.X = (int)playerPosition.X;
                playerBounds.Y = (int)playerPosition.Y + debugOffset;
            }

            return base.Collision(other);
        }

        /// <summary>
        /// controls the damage the player takes
        /// </summary>
        /// <param name="enemy">enemy the player collides with</param>
        public void PlayerTakeDamage(Enemy enemy)
        {
            //enemies do no damage if the player uses the Steel Heart artifact
            if (currentArtifact != null && currentArtifact.ArtifactType == ArtifactType.SteelHeart && currentArtifact.InUse)
            {
                enemy.Speed = -100;
            }
            else
            {
                //player takes damage
                health -= enemy.Damage;
                enemy.Speed = -100;
                isTakingDamage = true;
            }
        }
        public void PlayerTakeDamageFromBullets(EnemyBullet bullet)
        {
            //enemies do no damage if the player uses the Steel Heart artifact
            if (currentArtifact != null && currentArtifact.ArtifactType == ArtifactType.SteelHeart && currentArtifact.InUse)
            {
                health -= 0;
            }
            else
            {
                //player takes damage
                health -= 1;
                isTakingDamage = true;
            }
        }
        public void PlayerTakeDamageFromProjectile(int damage)
        {
            health -= damage;
            isTakingDamage = true;
        }
        /// <summary>
        /// player animation class
        /// </summary>
        /// <param name="gameTime"></param>
        private void UpdateAnimation(GameTime gameTime)
        {
            //total time 

            if(playerPosition != oldPosition)
            {
                timeCounter += gameTime.ElapsedGameTime.TotalSeconds;

                if (timeCounter >= secondsPerFrame)
                {
                    //get to next frome according to the time
                    currentFrame++;
                    //let the sprite loop 
                    if (currentFrame >= 8)
                        currentFrame = 1;


                    timeCounter -= secondsPerFrame;
                }
            }
            else
            {
                currentFrame = 1;
                timeCounter = 0;
            }

        }
        /// <summary>
        /// Interacts with objects within the dungeon. Specifically chests and collectibles.
        /// </summary>
        /// <param name="dungeon">The dungeon with the containing assets</param>
        public void Interact(Dungeon dungeon)
        {
            currentState = Keyboard.GetState();

            // Checks for key press
            if (SingleKeyPress(Keys.E))
            {
                // Checks for collectible interaction
                foreach (Collectible c in dungeon.Collectibles)
                {
                    if (c.inArea(this))
                    {
                        if (c.Item is Weapon)
                        {
                            Weapon old = this.Weapon;
                            this.Weapon = (Weapon)c.Item;
                            c.Item = old;
                            c.Texture = old.DisplayIcon;
                        }
                        else if (c.Item is Artifact && (currentArtifact != null && currentArtifact.InUse == false))
                        {
                            Artifact old = this.Artifact;
                            this.Artifact = (Artifact)c.Item;
                            c.Item = old;
                            c.Texture = old.DisplayIcon;
                        }
                        else if(c.Item is Artifact && currentArtifact == null)
                        {
                            this.Artifact = (Artifact)c.Item;
                            dungeon.Collectibles.Remove(c);
                            break;
                        }
                    }
                }

                // Checks for chest interaction
                foreach (Chest c in dungeon.Chests)
                {
                    c.OpenChest(dungeon, this);

                }


            }

            // gets set in useartifact method
            //oldState = currentState;

        }


        /// <summary>
        /// Draws the player based upon data in the player object.
        /// </summary>
        /// <param name="sb">SpriteBatch</param>
        public override void Draw(SpriteBatch sb)
        {
            //draws the player red if they are taking damage
            if (isTakingDamage)
            {
                sb.Draw(
                Texture,
                playerPosition,
                new Rectangle(widthOfSingleSprite * currentFrame, 0, widthOfSingleSprite, Texture.Height),
                Color.Red,
                0.0f,
                Vector2.Zero,
                playerScale,
                playerDirection,
                0.0f);

                isTakingDamage = false;
            }
            //draws the player blue if they are using the steel heart artifact
            else if (currentArtifact != null && currentArtifact.ArtifactType == ArtifactType.SteelHeart && currentArtifact.InUse)
            {
                sb.Draw(
                Texture,
                playerPosition,
                new Rectangle(widthOfSingleSprite * currentFrame, 0, widthOfSingleSprite, Texture.Height),
                Color.Blue,
                0.0f,
                Vector2.Zero,
                playerScale,
                playerDirection,
                0.0f);

                isTakingDamage = false;
            }
            //normal draw for the player
            else
            {
                sb.Draw(
                Texture,
                playerPosition,
                new Rectangle(widthOfSingleSprite * currentFrame, 0, widthOfSingleSprite, Texture.Height),
                Color.White,
                0.0f,
                Vector2.Zero,
                playerScale,
                playerDirection,
                0.0f);
            }
        }

        /// <summary>
        /// Detects a single key press (not holding down).
        /// </summary>
        /// <param name="key">The key to press once.</param>
        /// <returns>True if the key is pressed once, false otherwise.</returns>
        public bool SingleKeyPress(Keys key)
        {

            return currentState.IsKeyDown(key) && oldState.IsKeyUp(key);
        }


    }
}
