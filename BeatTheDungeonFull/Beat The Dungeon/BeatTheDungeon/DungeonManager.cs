using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;
//Yu Ma
//2023/04/28
namespace BeatTheDungeon
{
    internal class DungeonManager
    {
        //Firld
        private Dungeon dungeon0;
        private Dungeon dungeon1;
        private Dungeon dungeon2;
        private Dungeon dungeon3;
        private Dungeon dungeon4;
        private Dungeon dungeon5;
        private Dungeon dungeon6;
        private Dungeon dungeon7;
        private Dungeon currentDungeon;
        private Dungeon prevDungeon;
        private Dungeon nextDungeon;
        private string currentDungeonName;

        private ContentManager content;
        private bool prevDoor;
        private bool ifBossExist;

        private int currentRoom;

        // Properties
        public Dungeon CurrentDungeon { get => currentDungeon; }

        public Dungeon PreviousDungeon { get => prevDungeon; }

        public Dungeon NextDungeon { get => nextDungeon; }

        public int CurrentRoom { get => currentRoom; }


        //Constructor
        public DungeonManager(Texture texture, Texture2D blockTexture, ContentManager content,
            Texture2D dungeon0Texture, Texture2D dungeon1Texture, Texture2D dungeon2Texture,
            Texture2D dungeon3Texture, Texture2D dungeon4Texture, Texture2D dungeon5Texture,
            Texture2D dungeon6Texture, Texture2D dungeon7Texture)
        {
            dungeon0 = new Dungeon(texture);
            dungeon0.DungeonTexture = dungeon0Texture;
            dungeon0.DungeonLocation = new Point(0, 0);
            dungeon0.LoadEverythingFromFile("Content/dungeon0_0.dungeon", blockTexture);
            dungeon0.LoadDoor(content);
            dungeon1 = new Dungeon(texture);
            dungeon1.DungeonTexture = dungeon1Texture;
            dungeon1.DungeonLocation = new Point(1920, -1920);
            dungeon1.LoadEverythingFromFile("Content/dungeon1_1.dungeon", blockTexture);
            dungeon1.LoadDoor(content);
            dungeon2 = new Dungeon(texture);
            dungeon2.DungeonTexture = dungeon2Texture;
            dungeon2.DungeonLocation = new Point(1920, -5760);
            dungeon2.LoadEverythingFromFile("Content/dungeon1_2.dungeon", blockTexture);
            dungeon2.LoadDoor(content);
            dungeon3 = new Dungeon(texture);
            dungeon3.DungeonTexture = dungeon3Texture;
            dungeon3.DungeonLocation = new Point(3840, -5760);
            dungeon3.LoadEverythingFromFile("Content/dungeon1_3.dungeon", blockTexture);
            dungeon3.LoadDoor(content);
            dungeon4 = new Dungeon(texture);
            dungeon4.DungeonTexture = dungeon4Texture;
            dungeon4.DungeonLocation = new Point(3840, -1920);
            dungeon4.LoadEverythingFromFile("Content/dungeon1_4.dungeon", blockTexture);
            dungeon4.LoadDoor(content);
            dungeon5 = new Dungeon(texture);
            dungeon5.DungeonTexture = dungeon5Texture;
            dungeon5.DungeonLocation = new Point(3840, -3840);
            dungeon5.LoadEverythingFromFile("Content/dungeon1_5.dungeon", blockTexture);
            dungeon5.LoadDoor(content);
            dungeon6 = new Dungeon(texture);
            dungeon6.DungeonTexture = dungeon6Texture;
            dungeon6.DungeonLocation = new Point(1920, -7680);
            dungeon6.LoadEverythingFromFile("Content/dungeon1_6.dungeon", blockTexture);
            dungeon6.LoadDoor(content);
            dungeon7 = new Dungeon(texture);
            dungeon7.DungeonTexture = dungeon7Texture;
            dungeon7.DungeonLocation = new Point( 0, -1920);
            dungeon7.LoadEverythingFromFile("Content/dungeon1_7.dungeon", blockTexture);
            dungeon7.LoadDoor(content);
            //dungeon8 = new Dungeon(texture);
            //dungeon1.DungeonTexture = dungeon8Texture;
            currentDungeonName = "dungeon0";
            currentDungeon = dungeon0;
            prevDungeon = null;

            nextDungeon = dungeon7;
            this.content = content;
            prevDoor = false;

            currentRoom = 0;
        }

        //Methods--------------------------
        /// <summary>
        /// check if the room is clear
        /// </summary>
        public void OpenDoor(bool ifClear)
        {
            if(ifClear)
            {

                // open the door for the player
                foreach (Block b in currentDungeon.Blocks)
                {
                    if (b.Type == BlockType.Door)
                    {
                        b.IfHaveCollision = false;
                        b.Color = Color.Transparent;
                    }
                }
                if (nextDungeon != null)
                {
                    foreach (Block b in nextDungeon.Blocks)
                    {
                        if (b.Type == BlockType.Door)
                        {
                            b.IfHaveCollision = false;
                            b.Color = Color.Transparent;
                        }
                    }
                }
                if (prevDungeon != null)
                {
                    foreach (Block b in prevDungeon.Blocks)
                    {
                        if (b.Type == BlockType.Door)
                        {
                            b.IfHaveCollision = false;
                            b.Color = Color.Transparent;
                        }
                    }
                }
            }
            else
            {
                // open the door for the player
                foreach (Block b in currentDungeon.Blocks)
                {
                    if (b.Type == BlockType.Door)
                    {
                        b.IfHaveCollision = true;
                        b.Color = Color.White;
                    }
                }
                if (prevDungeon != null)
                {
                    foreach (Block b in prevDungeon.Blocks)
                    {
                        if (b.Type == BlockType.Door)
                        {
                            b.IfHaveCollision = true;
                            b.Color = Color.White;
                        }
                    }
                }
                if (nextDungeon != null)
                {
                    foreach (Block b in nextDungeon.Blocks)
                    {
                        if (b.Type == BlockType.Door)
                        {
                            b.IfHaveCollision = true;
                            b.Color = Color.White;
                        }
                    }
                }
            }
        }


        /// <summary>
        /// Updates the entire dungeon
        /// </summary>
        /// <param name="playerLocation"></param>
        public void Update(Vector2 playerLocation)
        {

            if (currentDungeon.Enemies.Count == 0 && currentDungeon.ShootEnemies.Count == 0 && prevDoor == false)
            {
                OpenDoor(true);
                prevDoor = true;
            }
            if (currentDungeon.IfEntered == false)
            {
                currentRoom++;

                currentDungeon.Spawnmob(content, playerLocation, this);
                currentDungeon.IfEntered = true;
                OpenDoor(false);
                prevDoor = false;
            }
            
            if(!new Rectangle(currentDungeon.DungeonLocation.X, currentDungeon.DungeonLocation.Y,1920, 1920).Contains(playerLocation))
            {
                if (new Rectangle(0 + 64, 0 + 64, 1920, 1920).Contains(playerLocation))
                {
                    currentDungeonName = "dungeon0";
                    if(prevDoor == true)
                    {
                        OpenDoor(false);
                        prevDoor = false;
                    }
                }
                else if (new Rectangle(1920 + 64, -1920 + 64, 1920 - 64, 1920 - 64).Contains(playerLocation))
                {
                    currentDungeonName = "dungeon1";
                    if (prevDoor == true)
                    {
                        OpenDoor(false);
                        prevDoor = false;
                    }
                }
                else if (new Rectangle(1920 + 64, -5760 + 64, 1920 - 64 - 64, 1920 - 64 - 64).Contains(playerLocation))
                {   
                    currentDungeonName = "dungeon2";
                    if (prevDoor == true)
                    {
                        OpenDoor(false);
                        prevDoor = false;
                    }
                }
                else if (new Rectangle(3840 + 64, -5760 + 64, 1920 - 64 - 64, 1920 - 64 - 64 - 64).Contains(playerLocation))
                {
                    currentDungeonName = "dungeon3";
                    if (prevDoor == true)
                    {
                        OpenDoor(false);
                        prevDoor = false;
                    }
                }
                else if (new Rectangle(3840 + 64, -1920 + 64, 1920 - 64, 1920 - 64).Contains(playerLocation))
                {
                    currentDungeonName = "dungeon4";
                    if (prevDoor == true)
                    {
                        OpenDoor(false);
                        prevDoor = false;
                    }
                }
                else if (new Rectangle(3840 + 64, -3840 + 64, 1920 - 64 - 64 , 1920 - 64 - 64 - 64).Contains(playerLocation))
                {
                    currentDungeonName = "dungeon5";
                    if (prevDoor == true)
                    {
                        OpenDoor(false);
                        prevDoor = false;
                    }
                }
                else if (new Rectangle(1920 + 64, -7680 + 64, 1920 - 64 - 64,  1920 - 64 -64 - 64).Contains(playerLocation))
                {
                    currentDungeonName = "dungeon6";
                    if (prevDoor == true)
                    {
                        OpenDoor(false);
                        prevDoor = false;
                    }
                }
                else if (new Rectangle(0 + 64, -1920 + 64, 1920 - 64 -64, 1920 - 64 -64 - 64).Contains(playerLocation))
                {
                    currentDungeonName = "dungeon7";
                    if (prevDoor == true)
                    {
                        OpenDoor(false);
                    }
                }
            }
            
            switch (currentDungeonName)
            {
                case "dungeon0":
                    nextDungeon = dungeon7;
                    currentDungeon = dungeon0;
                    prevDungeon = null;
                    break;
                case "dungeon1":
                    nextDungeon = dungeon4;
                    currentDungeon = dungeon1;
                    prevDungeon = dungeon7;
                    break;
                case "dungeon2":
                    nextDungeon = dungeon6;
                    currentDungeon = dungeon2;
                    prevDungeon = dungeon3;
                    break;
                case "dungeon3":
                    nextDungeon = dungeon2;
                    currentDungeon = dungeon3;
                    prevDungeon = dungeon5;
                    break;
                case "dungeon4":
                    nextDungeon = dungeon5;
                    currentDungeon = dungeon4;
                    prevDungeon = dungeon1;
                    break;
                case "dungeon5":
                    nextDungeon = dungeon3;
                    currentDungeon = dungeon5;
                    prevDungeon = dungeon4;
                    break;
                case "dungeon6":
                    nextDungeon = null;
                    currentDungeon = dungeon6;
                    prevDungeon = dungeon2;
                    if(!ifBossExist)
                    {
                        currentDungeon.SpawnBoss(content);
                        ifBossExist = true;
                    }
                    break;
                case "dungeon7":
                    nextDungeon = dungeon1;
                    currentDungeon = dungeon7;
                    prevDungeon = dungeon0;
                    break;
                default:
                    // there is a bug
                    foreach (Block b in currentDungeon.Blocks)
                    {
                        b.Color = Color.White;
                    }
                    break;
            }

            
        }

        public bool ifWin()
        {
            if(currentDungeonName == "dungeon6" && currentDungeon.IfCleared())
                return true;
            return false;
        }

        public void Draw(SpriteBatch sb)
        {
            if(currentDungeon != null)
            {
                currentDungeon.Draw(sb);
            }
            if (nextDungeon != null)
            {
                nextDungeon.Draw(sb);
            }
            if (prevDungeon != null)
            {
                prevDungeon.Draw(sb);
            }
        }

    }
}
