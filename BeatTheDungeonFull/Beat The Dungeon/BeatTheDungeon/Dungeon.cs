using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
// Yu Ma
//2023/03/29

namespace BeatTheDungeon
{
    /// <summary>
    /// create a single dungeon room
    /// there is a method to make every collsion block to a another color
    /// for testing purposes
    /// </summary>
    internal class Dungeon
    {
        // Fields ---------
        private List<Enemy> enemies;
        private List<EnemyShoot> shootEnemies;
        private List<Block> spawnPoint;
        private List<Block> blocks;
        private List<Projectile> projectiles;
        private List<Collectible> collectibles;
        private List<Potion> potions;
        private List<Chest> chests;
        private Texture2D texture;
        private Texture2D dungeonTexture;
        private Point dungeonLocation;
        private Random random;

        private Texture textures;
        private List<Block> doors;
        private bool ifCleared;
        // Properties--------------


        public bool IfEntered { get => ifCleared; set => ifCleared = value; }

        /// <summary>
        /// get and set the dungeon's loctaion
        /// </summary>
        public Point DungeonLocation { get => dungeonLocation; set => dungeonLocation = value; }

        /// <summary>
        /// get the Enemies list
        /// </summary>
        public List<Enemy> Enemies { get => enemies; }
        public List<EnemyShoot> ShootEnemies { get => shootEnemies; }
        /// <summary>
        ///  get the Blocks list
        /// </summary>
        public List<Block> Blocks { get => blocks; }

        /// <summary>
        /// get the projectiles list
        /// </summary>
        public List<Projectile> Projectiles { get => projectiles; }

        /// <summary>
        /// set the dungeon Texture
        /// </summary>
        public Texture2D DungeonTexture { set => dungeonTexture = value; }

        /// <summary>
        /// get the collectibles
        /// </summary>
        public List<Collectible> Collectibles { get => collectibles; }

        /// <summary>
        /// get the list of Chests
        /// </summary>
        public List<Chest> Chests { get => chests; }

        /// <summary>
        /// get the List of Potion
        /// </summary>
        public List<Potion> Potions { get => potions; }

        // Constructors---------
        /// <summary>
        /// create a dungeon
        /// </summary>
        public Dungeon(Texture textures)
        {
            enemies = new List<Enemy>();
            shootEnemies = new List<EnemyShoot>();
            blocks = new List<Block>();
            projectiles = new List<Projectile>();
            collectibles = new List<Collectible>();
            chests = new List<Chest>();
            potions = new List<Potion>();
            spawnPoint = new List<Block>();
            this.textures = textures;
            ifCleared = false;
            random = new Random();
            doors = new List<Block>();
        }

        // Methods-------------

        public bool IfCleared()
        {
            if (enemies.Count == 0 && shootEnemies.Count == 0)
                return true;
            return false;
        }

        public void LoadDoor(ContentManager content)
        {
            foreach(Block b in blocks)
            {
                if(b.Type == BlockType.Door)
                {
                    b.Texture = content.Load<Texture2D>("Door");
                }
            }
        }

        public void SpawnBoss(ContentManager content)
        {
            Enemy enemy = new Enemy(content.Load<Texture2D>("M_2_Zombie_Boss"), new Rectangle(dungeonLocation.X + 900, dungeonLocation.Y + 200,400,400), 3f,
                new Microsoft.Xna.Framework.Vector2(0,0), 300, 390, 2.5f, 15000, 1000f, 15, 8,this);
            enemies.Add(enemy);
        }

        /// <summary>
        /// sommon mobs
        /// </summary>
        /// <param name="content"></param>
        public void Spawnmob(ContentManager content, Microsoft.Xna.Framework.Vector2 playerLocation, DungeonManager dm)
        {
            foreach (Block b in blocks)
            {
                if (b.Type == BlockType.Spown)
                {
                    spawnPoint.Add(b);

                    // Big guy
                    for (int i = 0; i < 1; i++)
                    {
                        int x = b.Rectangle.X + random.Next(0, 30);
                        int y = b.Rectangle.Y + random.Next(0, 30);
                        float speed = (float)(random.NextDouble() * (2.5f - 1.5f) + 1);
                        Texture2D bigEnemyTexture = content.Load<Texture2D>("M_3_Zombie_Big");

                        Rectangle enemyRectangle = new Rectangle(x, y, bigEnemyTexture.Width, bigEnemyTexture.Height);
                        Enemy enemy = new Enemy(bigEnemyTexture, enemyRectangle, speed, new Microsoft.Xna.Framework.Vector2(0, 0),
                            50, 75, 0.5f,
                            75 + (20 * dm.CurrentRoom), 500f, 8, 8, this);
                        enemies.Add(enemy);
                    }

                    // Small dude
                    if (dm.CurrentRoom > 3)
                    {
                        for (int i = 0; i < 2; i++)
                        {
                            int x = b.Rectangle.X + random.Next(0, 40);
                            int y = b.Rectangle.Y + random.Next(0, 40); 
                            float speed = (float)(random.NextDouble() * (4f - 2f) + 1);
                            Texture2D zombieEnemy = content.Load<Texture2D>("EnemyZombie");

                            Rectangle zombieRectangle = new Rectangle(x, y, zombieEnemy.Width, zombieEnemy.Height);
                            Enemy zombie = new Enemy(zombieEnemy, zombieRectangle, speed, new Microsoft.Xna.Framework.Vector2(0, 0),
                                50, 75, 1.5f,
                                20 + (10 * dm.CurrentRoom), 600f, 2, 8, this);
                            enemies.Add(zombie);
                        }
                    }
                    
                    // Green shlime
                    if (dm.CurrentRoom > 5)
                    {
                        for (int i = 0; i < 1; i++)
                        {
                            int x = b.Rectangle.X + random.Next(0, 50);
                            int y = b.Rectangle.Y + random.Next(0, 50);
                            float speed = (float)(random.NextDouble() * (10f - 5f) + 1);
                            Texture2D theGreenGuy = content.Load<Texture2D>("M_4_Zombie_Big");

                            Rectangle greenGuyRectangle = new Rectangle(x, y, theGreenGuy.Width, theGreenGuy.Height);
                            Enemy greenGuy = new Enemy(theGreenGuy, greenGuyRectangle, speed, new Microsoft.Xna.Framework.Vector2(0, 0),
                                50, 50, 0.5f,
                                10 + (5 * dm.CurrentRoom), 700f, 10, 4, this);
                            enemies.Add(greenGuy);
                        }
                    }
                    

                    for (int i = 0; i < 1; i++)
                    {
                        int x = b.Rectangle.X + random.Next(0, 100);
                        int y = b.Rectangle.Y + random.Next(0, 100);
                        float speed = (float)(random.NextDouble() * (2.8f - 1.5f) + 1);
                        Microsoft.Xna.Framework.Vector2 enemyShootPosition = new Microsoft.Xna.Framework.Vector2(x, y);
                        Texture2D textureOfShootEnemy = content.Load<Texture2D>("M_5_Zombie_Big");
                        Texture2D enemyBullet = content.Load<Texture2D>("EnemyBullet");
                        EnemyShoot enemyShoot = new EnemyShoot(textureOfShootEnemy, enemyBullet, enemyShootPosition, playerLocation, this);
                        shootEnemies.Add(enemyShoot);
                    }
                }
            }
        }
        /// <summary>
        /// Add a game object to the dungeon
        /// </summary>
        /// <param name="gameObject"> some game object</param>
        public void Add(GameObject gameObject)
        {
            //adds each gameobject to its respective list
            if (gameObject is Enemy)
            {
                enemies.Add((Enemy)gameObject);
            }
            if (gameObject is EnemyShoot)
            {
                shootEnemies.Add((EnemyShoot)gameObject);
            }
            if (gameObject is Block)
            {
                blocks.Add((Block)gameObject);
            }
            {
                projectiles.Add((Projectile)gameObject);
            }
        }


        /// <summary>
        /// Load the dungeon form file
        /// </summary>
        /// <param name="file"> the file of dungeon</param>
        /// <param name="blockTexture"> the texture of blocks</param>
        public void LoadEverythingFromFile(string file, Texture2D blockTexture)
        {
            // print an error if we can't find the map in Visual studio output window
            if (!File.Exists(file))
            {
                System.Diagnostics.Debug.WriteLine($"Error: can't find file '{file}' in the output diectory");
                return;
            }

            // load content
            StreamReader input = new StreamReader(file);

            string line = null!;
            int xPosition = dungeonLocation.X!;
            int yPosition = dungeonLocation.Y!;
            int width = 64!;
            int height = 64!;

            while ((line = input.ReadLine()!) != null)
            {
                string[] words = line.Split(' ');
                //for (int j = 0; j < words.Length; j++)
                //{
                //    Console.WriteLine(words[j]);
                //}
                foreach (string word in words)
                {
                    try
                    {
                        if (word == "0")
                        {
                            xPosition += 64;
                        }
                        else if (word == "1")
                        {
                            Block b1 = new Block(blockTexture, new Rectangle(xPosition, yPosition, width, height), BlockType.Block);
                            blocks.Add(b1);
                            xPosition += 64;
                        }
                        else if (word == "2")
                        {
                            Block b1 = new Block(blockTexture, new Rectangle(xPosition, yPosition, width, height), BlockType.Spown);
                            blocks.Add(b1);
                            xPosition += 64;
                        }
                        else if (word == "3")
                        {
                            Block b1 = new Block(blockTexture, new Rectangle(xPosition, yPosition, width, height), BlockType.Fire);
                            blocks.Add(b1);
                            xPosition += 64;
                        }
                        else if (word == "4")
                        {
                            Random rng = new Random();

                            // Randomize rarity of chest spawn

                            if (rng.Next(0, 2) == 0)
                            {
                                Chest c1;

                                int ranNum = rng.Next(0, 10);

                                if (ranNum >= 0 && ranNum < 6)
                                {
                                    c1 = new Chest(Tier.Common, textures, xPosition + (width / 2) - 20, yPosition + (height / 2) - 20);
                                }
                                else if (ranNum >= 6 && ranNum < 9)
                                {
                                    c1 = new Chest(Tier.Rare, textures, xPosition + (width / 2) - 20, yPosition + (height / 2) - 20);
                                    
                                }
                                else
                                {
                                    c1 = new Chest(Tier.Legendary, textures, xPosition + (width / 2) - 20, yPosition + (height / 2) - 20);

                                }


                                chests.Add(c1);
                            }

                       

                            // potion test
                            //Potion p1 = new Potion(PotionType.Life, textures, xPosition + (width/2) - 20, yPosition + (height / 2) - 20);
                            //potions.Add(p1);

                            //Block b1 = new Block(blockTexture, new Rectangle(xPosition, yPosition, width, height), BlockType.Chest);
                            //blocks.Add(b1);
                            xPosition += 64;
                        }
                        else if (word == "5")
                        {
                            Block b1 = new Block(blockTexture, new Rectangle(xPosition, yPosition, width, height), BlockType.Door);
                            b1.Color = Color.White;
                            blocks.Add(b1);
                            xPosition += 64;
                        }
                    }
                    catch
                    {
                        System.Diagnostics.Debug.WriteLine($"Error: can't load file '{file}' ");
                    }
                }
                yPosition += 64;
                xPosition = dungeonLocation.X;
            }
            input.Close();
        }

        /// <summary>
        /// make the blocks collision box red
        /// </summary>
        public void DungeonCollsionTexting(bool onOrOff)
        {

            
            if (onOrOff)
            {
                foreach (Block b in blocks)
                {
                    if (b.Type == BlockType.Block)
                        b.Color = Color.Black;
                    else if (b.Type == BlockType.Spown)
                        b.Color = Color.Lime;
                    else if (b.Type == BlockType.Fire)
                        b.Color = Color.Firebrick;
                    else if (b.Type == BlockType.Chest)
                        b.Color = Color.Gold;
                    else if (b.Type == BlockType.Door)
                        b.Color = Color.SaddleBrown;
                }
            }
            else
            {
                foreach (Block b in blocks)
                {
                    if(b.Type != BlockType.Door)
                        b.Color = Color.Transparent;
                }
            }
        }

        /// <summary>
        /// draw the dungeon
        /// </summary>
        /// <param name="sb"></param>
        public void Draw(SpriteBatch sb)
        {
            //draws the dungeon
            sb.Draw(dungeonTexture, new Rectangle(dungeonLocation.X, dungeonLocation.Y, 1920, 1920), Color.White);

            //loops through all the lists and draws the gameobjects
            foreach (Block b in blocks)
            {
                sb.Draw(b.Texture, b.Rectangle, b.Color);

            }
            foreach (Projectile p in projectiles)
            {
                sb.Draw(p.Texture, p.Rectangle, Color.White);
            }
            if (IfCleared())
            {
                foreach (Chest c in chests)
                {
                    c.Draw(sb);
                }
            }
            foreach (Potion p in potions)
            {
                p.Draw(sb);
            }
            foreach (Collectible c in collectibles)
            {
                c.Draw(sb);
            }
        }
    }
}
