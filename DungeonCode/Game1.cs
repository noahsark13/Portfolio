using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Diagnostics;

namespace BeatTheDungeon
{
    //game states
    enum GameState
    {
        Start,
        Game,
        Pause,
        Win,
        GameOver,
        Controls
    }

    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        //basic single objects
        private Player player;
        private Camera camera;
        private DungeonManager dungeonManager;

        //player texture
        private Texture2D playerTexture;
        private Rectangle playerTextureBounds;

        //list of bullets
        private List<EnemyBullet> bullets;

        //number of enemies left
        int totalEnemies;

        //extra textures
        private Texture2D blockTexture;
        private Texture2D dungeonTexture;
        private Texture2D enemyBullet;

        //all textures used in multiple places
        private Texture assetTextures;

        //font used for debug
        private SpriteFont constantia;

        //UI elements
        private UI healthUI;
        private UI weaponUI;
        private UI artifactUI;

        //camera offset
        private Vector2 offset;

        //current and previous game states
        private GameState gameState;
        private GameState prevGameState;

        //current and previous keyboard inputs
        private KeyboardState currentKB;
        private KeyboardState prevKB;

        //list of projectiles
        private List<Projectile> projectiles;

        //old mouse state
        MouseState currentMouse;
        MouseState oldMouse;

        //check for if game is in debug mode
        bool debugMode;

        //menu textures
        private Texture2D startDefault;
        private Texture2D startHover;
        private Texture2D quitDefault;
        private Texture2D quitHover;
        private Texture2D logo;
        private Texture2D controls;

        //end game textures
        private Texture2D playagain;
        private Texture2D playagain2;
        private Texture2D gameover;
        private Texture2D win;

        //extra UI textures
        private Texture2D arrow;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            //initilizes camera and dungeon objects
            camera = new Camera(_graphics);
            projectiles = new List<Projectile>();
            bullets = new List<EnemyBullet>();
            //sets current and previous game state to Start
            gameState = GameState.Start;
            prevGameState = GameState.Start;

            //sets debug mode to false
            debugMode = false;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // Dungeon textures and object loaded here.
            assetTextures = new Texture(this.Content.Load<Texture2D>("singleBlock(white)"), this.Content.Load<Texture2D>("iconUI"), this.Content.Load<Texture2D>("healthbarUI"),
                this.Content.Load<Texture2D>("chest_empty_open_anim_f0"), this.Content.Load<Texture2D>("chestopencommon"),
                this.Content.Load<Texture2D>("chestrare"), this.Content.Load<Texture2D>("chestopenrare"),
                this.Content.Load<Texture2D>("chestlegendary"), this.Content.Load<Texture2D>("chestopenlegendary"),

                this.Content.Load<Texture2D>("fireballicon"), this.Content.Load<Texture2D>("triplefireballicon"), this.Content.Load<Texture2D>("fireball"),
                this.Content.Load<Texture2D>("slash"),
                this.Content.Load<Texture2D>("bombaicon"), this.Content.Load<Texture2D>("bomba"),
                this.Content.Load<Texture2D>("lighticon"), this.Content.Load<Texture2D>("lightattack"),
                this.Content.Load<Texture2D>("windicon"), this.Content.Load<Texture2D>("windattack"),
                this.Content.Load<Texture2D>("deathicon"), this.Content.Load<Texture2D>("deathattack"),
                this.Content.Load<Texture2D>("swift"), this.Content.Load<Texture2D>("steel"),
                this.Content.Load<Texture2D>("potion"), this.Content.Load<Texture2D>("potiongold"),
                this.Content.Load<SpriteFont>("Constantia"));

            // Load dungeon manager
            blockTexture = this.Content.Load<Texture2D>("singleBlock(white)");
            dungeonManager = new DungeonManager(assetTextures,blockTexture, Content,
            this.Content.Load<Texture2D>("dungeon0_0"), this.Content.Load<Texture2D>("dungeon1_1"), this.Content.Load<Texture2D>("dungeon1_2"),
            this.Content.Load<Texture2D>("dungeon1_3"), this.Content.Load<Texture2D>("dungeon1_4"), this.Content.Load<Texture2D>("dungeon1_5"),
            this.Content.Load<Texture2D>("dungeon1_6"), this.Content.Load<Texture2D>("dungeon1_7"));

            //player creation
            playerTexture = this.Content.Load<Texture2D>("C_Knight_f");
            playerTextureBounds = new Rectangle(0, 0, playerTexture.Width / 9, playerTexture.Height);
            player = new Player(playerTexture, new Rectangle(400, 400, playerTextureBounds.Width, playerTextureBounds.Height), playerTextureBounds, dungeonManager.CurrentDungeon, new Weapon(WeaponType.SingleFireBall, assetTextures, 10, 1), null);
      
            Random random = new Random();

            //enemy bullet
            enemyBullet = this.Content.Load<Texture2D>("fireball");

            //font
            constantia = this.Content.Load<SpriteFont>("Constantia");

            // Load dungeon
            dungeonTexture = this.Content.Load<Texture2D>("Dungeon0_0");

            //intialize the UI elements
            healthUI = new UI(assetTextures, UIType.Health, player);
            weaponUI = new UI(assetTextures, UIType.Weapon, player);
            artifactUI = new UI(assetTextures, UIType.Artifact, player);
            arrow = this.Content.Load<Texture2D>("arrow");

            //load the start menu texturess
            startDefault = this.Content.Load<Texture2D>("start");
            startHover = this.Content.Load<Texture2D>("start2");
            quitDefault = this.Content.Load<Texture2D>("quit");
            quitHover = this.Content.Load<Texture2D>("quit2");
            logo = this.Content.Load<Texture2D>("btd_logo");
            controls = this.Content.Load<Texture2D>("controls");

            //load the end game textures
            playagain = this.Content.Load<Texture2D>("playagain");
            playagain2 = this.Content.Load<Texture2D>("playagain2");
            gameover = this.Content.Load<Texture2D>("gameover");
            win = this.Content.Load<Texture2D>("win");
        }


        protected override void Update(GameTime gameTime)
        {

            //+1 for count the boss
            totalEnemies = dungeonManager.CurrentDungeon.Enemies.Count(e => !e.IsDead()) + 1;

            //sets previous game state and keyboard state to the current one
            prevGameState = gameState;
            prevKB = currentKB;

            //sets current keyboardstate
            currentKB = Keyboard.GetState();

            //sets the mousestate
            MouseState ms = Mouse.GetState();

            //key pressed is Enter
            if (SingleKeyPress(Keys.Enter))
            {
                //Start -> Game
                if (gameState == GameState.Controls)
                {
                    gameState = GameState.Game;
                }
                //Pause -> Game
                else if (gameState == GameState.Pause)
                {
                    gameState = GameState.Game;
                }
            }

            //key pressed is Escape
            if (SingleKeyPress(Keys.Escape))
            {
                //Game -> Pause
                if (gameState == GameState.Game)
                {
                    gameState = GameState.Pause;
                }
                //Pause -> Start
                else if (gameState == GameState.Pause)
                {
                    Initialize();
                    LoadContent();
                }
                //Start -> Exit Game
                else if (gameState == GameState.Controls)
                {
                    gameState = GameState.Start;
                }
            }

            //key pressed is Q, toggle debug mode
            if (SingleKeyPress(Keys.Q))
            {
                if (!debugMode)
                    debugMode = true;
                else if (debugMode)
                    debugMode = false;
            }

            //updates the dungeon manager
            dungeonManager.Update(player.PlayerPosition);

            //runs game updates when gamestate is Game
            if (gameState == GameState.Game)
            {
                //runs updates for player and enemies
                player.Update(gameTime,dungeonManager.CurrentDungeon, dungeonManager);
                foreach (Enemy enemy in dungeonManager.CurrentDungeon.Enemies)
                {
                    enemy.Update(gameTime, player);
                }
                foreach (EnemyShoot shootEnemy in dungeonManager.CurrentDungeon.ShootEnemies)
                {
                    shootEnemy.Update(gameTime, player);
                }
                //make enemy dead if the enemy is dead
                for (int i = 0; i < dungeonManager.CurrentDungeon.Enemies.Count; i++)
                {
                    if (dungeonManager.CurrentDungeon.Enemies[i].IsDead())
                    {
                        dungeonManager.CurrentDungeon.Enemies.RemoveAt(i);
                        i--;
                    }
                }
                for (int j = 0; j < dungeonManager.CurrentDungeon.ShootEnemies.Count; j++)
                {
                    if (dungeonManager.CurrentDungeon.ShootEnemies[j].ShootEnemyIsDead())
                    {
                        dungeonManager.CurrentDungeon.ShootEnemies.RemoveAt(j);
                        j--;
                    }
                }
                //determines enemy bullet collisions
                foreach (EnemyBullet bullet in bullets)
                {
                    bullet.Update(gameTime);
                    if (bullet.Collision(player))
                    {
                        player.Health--;
                        bullets.Remove(bullet);
                        break; // Only one bullet can hit the player at a time
                    }

                }
                //determines player bullet collisions
                foreach (Projectile p in projectiles)
                {
                    foreach (Enemy enemy in dungeonManager.CurrentDungeon.Enemies)
                    {
                        if (enemy.Collision(p))
                        {
                            enemy.enemyTakeDamage(p.Damage, assetTextures);
                        }
                    }
                    foreach (EnemyShoot shoorEnemy in dungeonManager.CurrentDungeon.ShootEnemies)
                    {
                        if (shoorEnemy.Collision(p))
                        {
                            shoorEnemy.ShooteEnemyTakeDamage(p.Damage);
                        }
                    }
                }

                //determines enemy collisions
                foreach (Enemy enemy in dungeonManager.CurrentDungeon.Enemies)
                {
                    if (enemy.Collision(player))
                    {
                        player.PlayerTakeDamage(enemy);
                    }
                }

                //camera updates matrix
                camera.Follow(player);

                //various player updates
                player.Attack(projectiles, dungeonManager.CurrentDungeon);
                if(dungeonManager.CurrentDungeon.IfCleared())
                {
                    player.Interact(dungeonManager.CurrentDungeon);
                }
                player.UseArtifact(gameTime);
            }

            //win and gameover conditions
            if (player.Health <= 0)
            {
                gameState = GameState.GameOver;
            }
            if (dungeonManager.ifWin() == true)
            {
                gameState = GameState.Win;
            }

            oldMouse = ms;

            //sets the offset for the camera
            offset = new Vector2(-((int)-player.PlayerPosition.X - player.Width / 2 * 3) - 400, -((int)-player.PlayerPosition.Y - player.Height / 2 * 3) - 300);

            base.Update(gameTime);
        }

        /// checks if a key has already been pressed
        /// </summary>
        /// <param name="key">key to be checked</param>
        /// <returns>boolean based on if the key has already been pressed</returns>
        public bool SingleKeyPress(Keys key)
        {
            return currentKB.IsKeyDown(key) && prevKB.IsKeyUp(key);
        }

        protected override void Draw(GameTime gameTime)
        {
            oldMouse = currentMouse;
            currentMouse = Mouse.GetState();

            if (gameState == GameState.Game)
            {
                //if the gamestate is Game, begin draw with change by camera matrix
                _spriteBatch.Begin(transformMatrix: camera.Transform);
            }
            else
            {
                //no camera matrix change in menus
                _spriteBatch.Begin();
            }

            //draws game if the game state is Game
            if (gameState == GameState.Game)
            {
                //draws all aspects of the dungeon
                GraphicsDevice.Clear(new Color(37, 19, 26));
                dungeonManager.Draw(_spriteBatch);
                foreach (Enemy enemy in dungeonManager.CurrentDungeon.Enemies)
                {
                    enemy.Draw(_spriteBatch);
                }
                foreach (EnemyShoot enemy in dungeonManager.CurrentDungeon.ShootEnemies)
                {
                    enemy.Draw(_spriteBatch);
                }
                player.Draw(_spriteBatch);

                //draws debug sprites
                if (debugMode)
                {
                    foreach (Enemy enemy in dungeonManager.CurrentDungeon.Enemies)
                    {
                        _spriteBatch.Draw(blockTexture, enemy.EnemyPosition, new Rectangle(0, 0, enemy.Rectangle.Width, enemy.Rectangle.Height), Color.Red);
                    }
                    {
                        dungeonManager.CurrentDungeon.DungeonCollsionTexting(debugMode);
                    }
                    foreach (EnemyShoot b in dungeonManager.CurrentDungeon.ShootEnemies)
                    {
                        _spriteBatch.Draw(blockTexture, b.EnemyPosition, new Rectangle(0, 0, b.Rectangle.Width, b.Rectangle.Height), Color.Red);
                    }
                    {
                        dungeonManager.CurrentDungeon.DungeonCollsionTexting(debugMode);
                    }
                   
                    _spriteBatch.Draw(blockTexture, new Rectangle((int)player.PlayerPosition.X, (int)player.PlayerPosition.Y + player.DebugOffset, player.Rectangle.Width, player.Rectangle.Height), Color.Green);
                }
                else
                {
                    dungeonManager.CurrentDungeon.DungeonCollsionTexting(false);
                }

                //draws all projectiles
                foreach (Projectile p in projectiles)
                {
                    p.Draw(_spriteBatch);
                    if (debugMode)
                        _spriteBatch.Draw(
                        blockTexture,
                        p.Rectangle,
                        null,
                        Color.Orange,
                        0.0f,
                        new Vector2((blockTexture.Width) / 2f, (blockTexture.Height) / 2f),
                        SpriteEffects.None,
                        0);
                }

                _spriteBatch.DrawString(constantia, "Press 'Esc' to Pause", new System.Numerics.Vector2(50 - ((int)-player.PlayerPosition.X - player.Width / 2 * 3) - 400, 100 - ((int)-player.PlayerPosition.Y - player.Height / 2 * 3) - 300), Color.White);

                DrawUI();

                foreach (Chest c in dungeonManager.CurrentDungeon.Chests)
                {
                    if (c.inArea(player) && (dungeonManager.CurrentDungeon.IfCleared()))
                    {
                        _spriteBatch.DrawString(assetTextures.Font, "Press E to Open", new Vector2(c.Rectangle.X - c.Width, c.Rectangle.Y - c.Height / 2), Color.White);
                    }
                }

                //checks if the current room is cleared
                if(dungeonManager.CurrentDungeon.IfCleared())
                {
                    //boolean used to determine the second door (exit)
                    bool secondDoor = false;
                    foreach (Block b in dungeonManager.CurrentDungeon.Blocks)
                    {
                        if (b.Type == BlockType.Door)
                        {
                            if(secondDoor == true)
                            {
                                //arrows points in the cardinal direction of the exit door
                                int blockX = b.X + b.Width;
                                int blockY = b.Y + b.Height;
                                if (Math.Abs(player.PlayerCenter.X - blockX) > Math.Abs(player.PlayerCenter.Y - blockY))
                                {
                                    if (player.PlayerCenter.X - blockX < 0 && Math.Abs(player.PlayerCenter.X - blockX) > 150)
                                        _spriteBatch.Draw(arrow, new Vector2((int)player.PlayerCenter.X + arrow.Width / 2 + 50, (int)player.PlayerCenter.Y - arrow.Height / 8 + 10), new Rectangle(0, 0, arrow.Width, arrow.Height), Color.White, (float)Math.PI / 2, new Vector2(arrow.Width / 2, arrow.Height / 2), new Vector2(0.5f, 0.5f), SpriteEffects.None, 0f);
                                    if (player.PlayerCenter.X - blockX > 0 && Math.Abs(player.PlayerCenter.X - blockX) > 150)
                                        _spriteBatch.Draw(arrow, new Vector2((int)player.PlayerCenter.X - arrow.Width / 2 - 50, (int)player.PlayerCenter.Y - arrow.Height / 8 + 10), new Rectangle(0, 0, arrow.Width, arrow.Height), Color.White, -(float)Math.PI / 2, new Vector2(arrow.Width / 2, arrow.Height / 2), new Vector2(0.5f, 0.5f), SpriteEffects.None, 0f);
                                }
                                else
                                {
                                    if (player.PlayerCenter.Y - blockY < 0 && Math.Abs(player.PlayerCenter.Y - blockY) > 150)
                                        _spriteBatch.Draw(arrow, new Vector2((int)player.PlayerCenter.X - arrow.Width / 8 + 10, (int)player.PlayerCenter.Y + arrow.Height / 2 + 50), new Rectangle(0, 0, arrow.Width, arrow.Height), Color.White, (float)Math.PI, new Vector2(arrow.Width / 2, arrow.Height / 2), new Vector2(0.5f, 0.5f), SpriteEffects.None, 0f);
                                    if (player.PlayerCenter.Y - blockY > 0 && Math.Abs(player.PlayerCenter.Y - blockY) > 150)
                                        _spriteBatch.Draw(arrow, new Vector2((int)player.PlayerCenter.X - arrow.Width / 8 + 10, (int)player.PlayerCenter.Y - arrow.Height / 2 - 50), new Rectangle(0, 0, arrow.Width, arrow.Height), Color.White, 0, new Vector2(arrow.Width / 2, arrow.Height / 2), new Vector2(0.5f, 0.5f), SpriteEffects.None, 0f);
                                }
                                break;
                            }
                            else
                            {
                                secondDoor = true;
                            }
                        }
                    }
                }
            }

            //draws the menus based on the game state
            if (gameState == GameState.Start)
            {
                GraphicsDevice.Clear(Color.Black);
                MainMenu();
            }
            if (gameState == GameState.Controls)
            {
                GraphicsDevice.Clear(Color.Black);
                _spriteBatch.Draw(controls, new Rectangle(0, 0, controls.Width, controls.Height), Color.White);
            }
            if (gameState == GameState.Pause)
            {
                GraphicsDevice.Clear(Color.Gray);
                _spriteBatch.DrawString(constantia, "SUPER AWESOME AND COOL PAUSE MENU\n\nPress 'Enter' to Resume\nPress 'Esc' to Return to Start", new System.Numerics.Vector2(100, 100), Color.White);
            }
            if (gameState == GameState.GameOver)
            {
                GraphicsDevice.Clear(Color.Black);
                if (GameOverMenu())
                {
                    return;
                }
            }
            if (gameState == GameState.Win)
            {
                GraphicsDevice.Clear(Color.Black);
                if (WinMenu())
                {
                    return;
                }
            }

            _spriteBatch.End();
            base.Draw(gameTime);
        }

        /// <summary>
        /// draws the UI elements
        /// </summary>
        public void DrawUI()
        {
            //draws the UI elements
            healthUI.Draw(_spriteBatch, offset);
            weaponUI.Draw(_spriteBatch, offset);
            artifactUI.Draw(_spriteBatch, offset);



            //mousestate
            MouseState ms = Mouse.GetState();

            //subtracts the camera offset from the mouse position
            Vector2 mouseOffset = new Vector2(ms.X + offset.X, ms.Y + offset.Y);

            //makes a new rectangle based on the weapon UI
            Rectangle weaponRect = new Rectangle((int)(weaponUI.Rectangle.X + offset.X), (int)(weaponUI.Rectangle.Y + offset.Y - 80), weaponUI.Rectangle.Width, weaponUI.Rectangle.Height);

            //checks if the weaponUI contains the mouse
            if (weaponRect.Contains(mouseOffset))
            {
                //draws the stats of the weapon
                Rectangle statsRect = new Rectangle((int)offset.X + 25, (int)offset.Y + 450, 150, 100);
                _spriteBatch.Draw(assetTextures.HitboxTexture, statsRect, Color.DimGray);

                _spriteBatch.DrawString(constantia, $"{player.Weapon.Name}:", new Vector2(statsRect.X + 10, statsRect.Y + 10), Color.White, 0f, new Vector2(0, 0), .9f, SpriteEffects.None, 0f); ;
                _spriteBatch.DrawString(constantia, $"Damage: {player.Weapon.Damage}", new Vector2(statsRect.X + 10, statsRect.Y + 40), Color.White, 0f, new Vector2(0, 0), .75f, SpriteEffects.None, 0f);
                _spriteBatch.DrawString(constantia, $"Cooldown: {Math.Round(player.Weapon.CooldownDuration, 2)} sec", new Vector2(statsRect.X + 10, statsRect.Y + 70), Color.White, 0f, new Vector2(0, 0), .75f, SpriteEffects.None, 0f);
            }

            //makes a new rectangle based on the artifact UI
            Rectangle artifactRect = new Rectangle((int)(artifactUI.Rectangle.X + offset.X), (int)(artifactUI.Rectangle.Y + offset.Y - 80), artifactUI.Rectangle.Width, artifactUI.Rectangle.Height);

            //checks if the artifactUI contains the mouse
            if (player.Artifact != null && artifactRect.Contains(mouseOffset))
            {
                //draws the stats of the artifact
                Rectangle statsRect = new Rectangle((int)offset.X + 625, (int)offset.Y + 450, 150, 100);
                _spriteBatch.Draw(assetTextures.HitboxTexture, statsRect, Color.DimGray);

                _spriteBatch.DrawString(constantia, $"{player.Artifact.Name}:", new Vector2(statsRect.X + 10, statsRect.Y + 10), Color.White, 0f, new Vector2(0, 0), .9f, SpriteEffects.None, 0f); ;
                _spriteBatch.DrawString(constantia, $"Ability: {player.Artifact.Description}", new Vector2(statsRect.X + 10, statsRect.Y + 40), Color.White, 0f, new Vector2(0, 0), .75f, SpriteEffects.None, 0f);
                _spriteBatch.DrawString(constantia, $"Cooldown: {Math.Round(player.Artifact.CooldownDuration, 2)} sec", new Vector2(statsRect.X + 10, statsRect.Y + 70), Color.White, 0f, new Vector2(0, 0), .75f, SpriteEffects.None, 0f);
            }
        }

        /// <summary>
        /// draws the main menu
        /// </summary>
        public void MainMenu()
        {
            MouseState ms = Mouse.GetState();

            //start button
            float startScale = 1.2f;
            Rectangle startRect = new Rectangle(_graphics.PreferredBackBufferWidth / 2 - (int)(startDefault.Width * startScale / 2), 250, (int)(startDefault.Width * startScale), (int)(startDefault.Height * startScale));
            if (startRect.Contains(ms.Position))
            {
                _spriteBatch.Draw(startHover, new Vector2(startRect.X, startRect.Y), new Rectangle(0, 0, (int)(startRect.Width / startScale), (int)(startRect.Height / startScale)), Color.White, 0f, new Vector2(0, 0), startScale, SpriteEffects.None, 0f);

                if (SingleClick())
                {
                    gameState = GameState.Controls;
                }
            }
            else
                _spriteBatch.Draw(startDefault, new Vector2(startRect.X, startRect.Y), new Rectangle(0, 0, (int)(startRect.Width / startScale), (int)(startRect.Height / startScale)), Color.White, 0f, new Vector2(0, 0), startScale, SpriteEffects.None, 0f);

            //quit button
            Rectangle quitRect = new Rectangle(_graphics.PreferredBackBufferWidth / 2 - quitDefault.Width / 2, 350, quitDefault.Width, quitDefault.Height);
            if (quitRect.Contains(ms.Position))
            {
                _spriteBatch.Draw(quitHover, quitRect, Color.White);

                if (SingleClick())
                {
                    Exit();
                }
            }
            else
                _spriteBatch.Draw(quitDefault, quitRect, Color.White);

            //logo
            _spriteBatch.Draw(logo, new Vector2(_graphics.PreferredBackBufferWidth / 2 - logo.Width / 2 * startScale, -150), new Rectangle(0, 0, logo.Width, logo.Height), Color.White, 0f, new Vector2(0, 0), startScale, SpriteEffects.None, 0f);
        }

        /// <summary>
        /// draws the gameover menu
        /// </summary>
        /// <returns>returns whether or not the play wants to play again</returns>
        public bool GameOverMenu()
        {
            MouseState ms = Mouse.GetState();

            //play again button scaling
            float playagainScale = 1.2f;

            //gameover
            _spriteBatch.Draw(gameover, new Vector2(_graphics.PreferredBackBufferWidth / 2 - logo.Width / 2 * playagainScale, -150), new Rectangle(0, 0, logo.Width, logo.Height), Color.White, 0f, new Vector2(0, 0), playagainScale, SpriteEffects.None, 0f);

            //play again button
            Rectangle playagainRect = new Rectangle(_graphics.PreferredBackBufferWidth / 2 - (int)(playagain.Width * playagainScale / 2), 250, (int)(playagain.Width * playagainScale), (int)(playagain.Height * playagainScale));
            if (playagainRect.Contains(ms.Position))
            {
                _spriteBatch.Draw(playagain2, new Vector2(playagainRect.X, playagainRect.Y), new Rectangle(0, 0, (int)(playagainRect.Width / playagainScale), (int)(playagainRect.Height / playagainScale)), Color.White, 0f, new Vector2(0, 0), playagainScale, SpriteEffects.None, 0f);

                if (SingleClick())
                {
                    _spriteBatch.End();
                    Initialize();
                    LoadContent();
                    return true;
                }
            }
            else
                _spriteBatch.Draw(playagain, new Vector2(playagainRect.X, playagainRect.Y), new Rectangle(0, 0, (int)(playagainRect.Width / playagainScale), (int)(playagainRect.Height / playagainScale)), Color.White, 0f, new Vector2(0, 0), playagainScale, SpriteEffects.None, 0f);

            //quit button
            Rectangle quitRect = new Rectangle(_graphics.PreferredBackBufferWidth / 2 - playagain2.Width / 2, 350, playagain2.Width, playagain2.Height);
            if (quitRect.Contains(ms.Position))
            {
                _spriteBatch.Draw(quitHover, quitRect, Color.White);

                if (SingleClick())
                {
                    Exit();
                }
            }
            else
                _spriteBatch.Draw(quitDefault, quitRect, Color.White);

            return false;
        }

        /// <summary>
        /// draws the win menu
        /// </summary>
        /// <returns>returns whether or not the play wants to play again</returns>
        public bool WinMenu()
        {
            MouseState ms = Mouse.GetState();

            //play again button scaling
            float playagainScale = 1.2f;

            //gameover
            _spriteBatch.Draw(win, new Vector2(_graphics.PreferredBackBufferWidth / 2 - logo.Width / 2 * playagainScale, -150), new Rectangle(0, 0, logo.Width, logo.Height), Color.White, 0f, new Vector2(0, 0), playagainScale, SpriteEffects.None, 0f);

            //play again button
            Rectangle playagainRect = new Rectangle(_graphics.PreferredBackBufferWidth / 2 - (int)(playagain.Width * playagainScale / 2), 250, (int)(playagain.Width * playagainScale), (int)(playagain.Height * playagainScale));
            if (playagainRect.Contains(ms.Position))
            {
                _spriteBatch.Draw(playagain2, new Vector2(playagainRect.X, playagainRect.Y), new Rectangle(0, 0, (int)(playagainRect.Width / playagainScale), (int)(playagainRect.Height / playagainScale)), Color.White, 0f, new Vector2(0, 0), playagainScale, SpriteEffects.None, 0f);

                if (SingleClick())
                {
                    _spriteBatch.End();
                    Initialize();
                    LoadContent();
                    return true;
                }
            }
            else
                _spriteBatch.Draw(playagain, new Vector2(playagainRect.X, playagainRect.Y), new Rectangle(0, 0, (int)(playagainRect.Width / playagainScale), (int)(playagainRect.Height / playagainScale)), Color.White, 0f, new Vector2(0, 0), playagainScale, SpriteEffects.None, 0f);

            //quit button
            Rectangle quitRect = new Rectangle(_graphics.PreferredBackBufferWidth / 2 - playagain2.Width / 2, 350, playagain2.Width, playagain2.Height);
            if (quitRect.Contains(ms.Position))
            {
                _spriteBatch.Draw(quitHover, quitRect, Color.White);

                if (SingleClick())
                {
                    Exit();
                }
            }
            else
                _spriteBatch.Draw(quitDefault, quitRect, Color.White);

            return false;
        }

        /// checks if a single click has happened
        /// </summary>
        /// <param name="key">key to be checked</param>
        /// <returns>boolean based on if the key has already been pressed</returns>
        public bool SingleClick()
        {
            return currentMouse.LeftButton == ButtonState.Pressed && oldMouse.LeftButton != ButtonState.Pressed;
        }
    }
}