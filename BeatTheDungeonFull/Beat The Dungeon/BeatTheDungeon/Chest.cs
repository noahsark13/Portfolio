using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

/// <summary>
/// Enum for tiers of chests.
/// </summary>
public enum Tier
{
    Common,
    Rare,
    Legendary
}

namespace BeatTheDungeon
{
    /// <summary>
    /// Represents a chest in a dungeon.
    /// </summary>
    internal class Chest : GameObject
    {
        private Collectible collectible;
        private Rectangle detectionRectangle;

        private Texture2D openTexture;
        private Texture2D currentTexture;

        private Random rng;

        private Item item;
        private bool isOpen;

        Texture2D hitbox;

        /// <summary>
        /// Get and sets the collectible within the chest.
        /// </summary>
        public Collectible Collectible { get { return collectible; } set { collectible = value; } }

        /// <summary>
        /// Chest constructor, randomizes the collectible within depending on the given chest tier.
        /// </summary>
        /// <param name="tier">The rarity of the chest</param>
        /// <param name="textures">The textures of the weapons/assets</param>
        /// <param name="x">X location</param>
        /// <param name="y">Y location</param>
        public Chest(Tier tier, Texture textures, int x, int y) : base(null, new Rectangle(x, y, 40, 40))
        {
            rng = new Random();

            this.collectible = null;

            // Chest textures

            Texture = textures.ChestTextures["commonClosed"];
            this.openTexture = textures.ChestTextures["commonOpen"];
            this.currentTexture = textures.ChestTextures["commonClosed"];

            switch (tier)
            {
                case Tier.Common:
                    {
                        Texture = textures.ChestTextures["commonClosed"];
                        this.openTexture = textures.ChestTextures["commonOpen"];
                        this.currentTexture = textures.ChestTextures["commonClosed"];
                        break;
                    }
                case Tier.Rare:
                    {
                        Texture = textures.ChestTextures["rareClosed"];
                        this.openTexture = textures.ChestTextures["rareOpen"];
                        this.currentTexture = textures.ChestTextures["rareClosed"];
                        break;
                    }
                case Tier.Legendary:
                    {
                        Texture = textures.ChestTextures["legendaryClosed"];
                        this.openTexture = textures.ChestTextures["legendaryOpen"];
                        this.currentTexture = textures.ChestTextures["legendaryClosed"];
                        break;
                    }
            }


            // Detection area
            detectionRectangle = new Rectangle(x - 75 + 20, y - 75 + 20, 150, 150);
            hitbox = textures.HitboxTexture;

            this.isOpen = false;

            // Artifact (1/4) chance to drop a artifact
            if (rng.Next(0, 4) == 0)
            {
                // Tiers of chests determines the duration and cooldown of a artifact.
                switch (tier)
                {
                    case Tier.Common:
                        {
                            int randomNum = rng.Next(0, 2);

                            switch (randomNum)
                            {

                                case 0:
                                    {
                                        item = new Artifact(10, ArtifactType.SwiftFoot, textures, 2);
                                        break;
                                    }
                                case 1:
                                    {
                                        item = new Artifact(15, ArtifactType.SteelHeart, textures, 3);
                                        break;

                                    }

                            }
                            break;
                        }
                    case Tier.Rare:
                        {
                            int randomNum = rng.Next(0, 2);

                            switch (randomNum)
                            {

                                case 0:
                                    {
                                        item = new Artifact(12, ArtifactType.SwiftFoot, textures, 4);
                                        break;
                                    }
                                case 1:
                                    {
                                        item = new Artifact(18, ArtifactType.SteelHeart, textures, 6);
                                        break;

                                    }

                            }
                            break;
                        }
                    case Tier.Legendary:
                        {
                            int randomNum = rng.Next(0, 2);

                            switch (randomNum)
                            {

                                case 0:
                                    {
                                        item = new Artifact(15, ArtifactType.SwiftFoot, textures, 8);
                                        break;
                                    }
                                case 1:
                                    {
                                        item = new Artifact(20, ArtifactType.SteelHeart, textures, 10);
                                        break;

                                    }

                            }
                            break;
                        }

                }
            }
            // Weapon
            else
            {
                switch (tier)
                {
                    // Random items for each tier (damage and cooldown are randomly determined)
                    case Tier.Common:
                        {
                            int randomNum = rng.Next(0, 3);

                            switch (randomNum)
                            {
                                case 0:
                                    {
                                        item = new Weapon(WeaponType.SingleFireBall, textures, rng.Next(10, 26), RandomFloat(0.1f, 1));
                                        break;
                                    }
                                case 1:
                                    {
                                        item = new Weapon(WeaponType.DivineFist, textures, rng.Next(25, 51), RandomFloat(0.5f, 1.5f));
                                        break;
                                    }
                                case 2:
                                    {
                                        item = new Weapon(WeaponType.LightPiercer, textures, rng.Next(5, 26), RandomFloat(0.01f, 0.5f));
                                        break;
                                    }

                            }


                            break;
                        }
                    case Tier.Rare:
                        {
                            int randomNum = rng.Next(0, 3);

                            switch (randomNum)
                            {
                                case 0:
                                    {
                                        item = new Weapon(WeaponType.TripleFireBall, textures, rng.Next(25, 41), RandomFloat(0.1f, 1.5f));
                                        break;
                                    }
                                case 1:
                                    {
                                        item = new Weapon(WeaponType.TempestWinds, textures, rng.Next(30, 51), RandomFloat(0.5f, 2));
                                        break;
                                    }
                                case 2:
                                    {
                                        item = new Weapon(WeaponType.ElBombino, textures, rng.Next(45, 81), RandomFloat(1.5f, 3f));
                                        break;
                                    }


                            }

                            break;
                        }
                    case Tier.Legendary:
                        {

                            int randomNum = rng.Next(0, 10);

                            switch (randomNum)
                            {

                                case 9:
                                    {
                                        item = new Weapon(WeaponType.RapidFireDestructionDeath, textures, 1, 0f);
                                        break;
                                    }
                                default:
                                    {
                                        item = new Weapon(WeaponType.SingleFireBall, textures, rng.Next(5, 10), RandomFloat(5f, 10f));
                                        break;

                                    }

                            }
                            break;
                        }
                }
                
            }

            // assigns the collectible within the chest
            collectible = new Collectible(item, new Rectangle(x + 4, y + 40, 30, 30));

        }


        /// <summary>
        /// Opens the chest if player is in the detection area, adding the collectible to the collectibles list.
        /// </summary>
        /// <param name="dungeon">The dungeon object.</param>
        /// <param name="player">The player.</param>
        public void OpenChest(Dungeon dungeon, Player player)
        {

            if (this.inArea(player) && isOpen == false)
            {
                if (collectible != null)
                {
                    dungeon.Collectibles.Add(collectible);
                    currentTexture = openTexture;
                    this.isOpen = true;

                }

            }
        }

        /// <summary>
        /// Detects if a player is in the area to interact with the chest.
        /// </summary>
        /// <param name="other">The other object to check if is in the area.</param>
        /// <returns>True or false depending on if the object is in the area.</returns>
        public bool inArea(GameObject other)
        {
            if (detectionRectangle.Intersects(other.Rectangle))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Draws the chest.
        /// </summary>
        /// <param name="sb">Spritebatch</param>
        public override void Draw(SpriteBatch sb)
        {


            // detection hitbox
            //sb.Draw(
            //    hitbox,
            //    detectionRectangle,
            //    Color.White
            //    );ddw

            sb.Draw(
                    currentTexture,
                    Rectangle,
                    Color.White);


        }

        /// <summary>
        /// Returns a float between a min and max rounded to the hundredths place
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public float RandomFloat(float min, float max)
        {
            double rand = rng.NextDouble() * (max - min) + min;
            return (float)Math.Round(rand, 2);
        }
    }
}
