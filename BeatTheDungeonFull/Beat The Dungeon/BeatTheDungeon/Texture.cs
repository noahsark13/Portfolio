using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace BeatTheDungeon
{
    /// <summary>
    /// Holds various textures used throughout the dungeon.
    /// </summary>
    internal class Texture
    {
        private Dictionary<string, Texture2D> weaponTextures;
        private Dictionary<string, Texture2D> chestTextures;
        private Dictionary<string, Texture2D> enemyTextures;
        private Dictionary<string, Texture2D> iconTextures;

        private Texture2D hitboxTexture;
        private Texture2D iconUI;
        private Texture2D barUI;

        private SpriteFont font;

        /// <summary>
        /// Gets all the weapon textures.
        /// </summary>
        public Dictionary<string, Texture2D> WeaponTextures { get { return weaponTextures; } }

        /// <summary>
        /// Gets all the chest textures
        /// </summary>
        public Dictionary<string, Texture2D> ChestTextures { get { return chestTextures; } }

        public Dictionary<string, Texture2D> EnemyTextures { get { return enemyTextures; } }

        public Dictionary<string, Texture2D> IconTextures { get { return iconTextures; } }

        /// <summary>
        /// Gets the hitbox texture used throughout the game debugging.
        /// </summary>
        public Texture2D HitboxTexture { get { return hitboxTexture; } }

        public Texture2D IconUI { get { return iconUI; } }

        public Texture2D BarUI { get { return barUI; } }

        /// <summary>
        /// simple get for the game's font
        /// </summary>
        public SpriteFont Font { get { return font; } }

        /// <summary>
        /// Texture class constructor.
        /// </summary>
        /// <param name="hitboxTexture">The texture of the hitbox</param>
        /// <param name="commonChestClosed">Texture of a common closed chest</param>
        /// <param name="commonChestOpen">Texture of a common open chest</param>
        /// <param name="fireballTexture">The fireball weapon texture</param>
        /// <param name="font">font of our game</param>
        public Texture(Texture2D hitboxTexture, Texture2D iconUI, Texture2D barUI,
            Texture2D commonChestClosed, Texture2D commonChestOpen,
            Texture2D rareChestClosed, Texture2D rareChestOpen,
            Texture2D legendaryChestClosed, Texture2D legendaryChestOpen,

            Texture2D fireballIcon, Texture2D tripleFireballIcon, Texture2D fireballTexture,
            Texture2D slashTexture,
            Texture2D bombaIcon, Texture2D bombaTexture,
            Texture2D lightIcon, Texture2D lightTexture,
            Texture2D windIcon, Texture2D windTexture,
            Texture2D deathIcon, Texture2D deathTexture,

            Texture2D swiftIcon, Texture2D steelIcon,
            Texture2D redPotion, Texture2D goldPotion,
            SpriteFont font)
        {
            this.hitboxTexture = hitboxTexture;
            this.iconUI = iconUI;
            this.barUI = barUI;

            // Chest stuff
            chestTextures = new Dictionary<string, Texture2D>();
            chestTextures.Add("commonClosed", commonChestClosed);
            chestTextures.Add("commonOpen", commonChestOpen);
            chestTextures.Add("rareClosed", rareChestClosed);
            chestTextures.Add("rareOpen", rareChestOpen);
            chestTextures.Add("legendaryClosed", legendaryChestClosed);
            chestTextures.Add("legendaryOpen", legendaryChestOpen);


            // Weapon stuff
            weaponTextures = new Dictionary<string, Texture2D>();
            weaponTextures.Add("fireball", fireballTexture);
            weaponTextures.Add("slash", slashTexture);
            weaponTextures.Add("bomba", bombaTexture);
            weaponTextures.Add("light", lightTexture);
            weaponTextures.Add("wind", windTexture);
            weaponTextures.Add("death", deathTexture);

            // Collectible Icons
            iconTextures = new Dictionary<string, Texture2D>();
            iconTextures.Add("singleFireball", fireballIcon);
            iconTextures.Add("tripleFireball", tripleFireballIcon);
            iconTextures.Add("bomba", bombaIcon);
            iconTextures.Add("light", lightIcon);
            iconTextures.Add("wind", windIcon);
            iconTextures.Add("death", deathIcon);

            iconTextures.Add("swift", swiftIcon);
            iconTextures.Add("steel", steelIcon);

            iconTextures.Add("redPotion", redPotion);
            iconTextures.Add("goldPotion", goldPotion);

            this.font = font;
        }
    }
}