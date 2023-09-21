using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Diagnostics;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.IO;
using System.Threading;
using System.Reflection.Metadata;

namespace BeatTheDungeon
{
    //enum for the type of UI being displayed
    enum UIType
    {
        Health,
        Weapon,
        Artifact
    }

    internal class UI
    {
        //bordering rectangle
        private Rectangle rectangle;
        //type of UI
        private UIType uiType;
        //all game textures
        private Texture textures;
        //player
        private Player player;

        public Rectangle Rectangle { get => rectangle; }

        /// <summary>
        /// Constructor that gets the textures, type, and player
        /// </summary>
        /// <param name="textures">all game textures</param>
        /// <param name="uiType">type of UI</param>
        /// <param name="player">player</param>
        public UI(Texture textures, UIType uiType, Player player)
        {
            //sets fields
            this.uiType = uiType;
            this.textures = textures;
            this.player = player;

            //chooses rectangle based on UI type
            switch (uiType)
            {
                case (UIType.Health):
                    rectangle = new Rectangle(300, 500, 200, 50);
                    break;
                case (UIType.Weapon):
                    rectangle = new Rectangle(200, 500, 50, 50);
                    break;
                case (UIType.Artifact):
                    rectangle = new Rectangle(550, 500, 50, 50);
                    break;
            }
        }

        /// <summary>
        /// draws the UI
        /// </summary>
        /// <param name="sb">spritebatch</param>
        /// <param name="offset">offset of the camera</param>
        public void Draw(SpriteBatch sb, Vector2 offset)
        {
            //draws based on the type of UI
            switch (uiType)
            {
                //health UI
                //red bar changes based on current player health
                case (UIType.Health):

                    sb.Draw(textures.HitboxTexture, new Rectangle(rectangle.X + (int)offset.X, rectangle.Y + (int)offset.Y, rectangle.Width, rectangle.Height), Color.Gray);
                    sb.Draw(textures.HitboxTexture, new Rectangle(rectangle.X + (int)offset.X, rectangle.Y + (int)offset.Y, rectangle.Width - (rectangle.Width - ((rectangle.Width * player.Health) / player.MaxHealth)), rectangle.Height), Color.IndianRed);
                    sb.DrawString(textures.Font, $"Health: {player.Health} / {player.MaxHealth}", new Vector2(rectangle.X + (int)offset.X + 25, rectangle.Y + (int)offset.Y + 15), Color.White);

                    sb.Draw(textures.BarUI, new Rectangle(rectangle.X + (int)offset.X - 10, rectangle.Y + (int)offset.Y - 10, rectangle.Width + 20, rectangle.Height + 20), Color.White);
                    break;

                //weapon UI
                //shrinking gray bar displays current weapon cooldown
                case (UIType.Weapon):
                    sb.Draw(textures.HitboxTexture, new Rectangle(rectangle.X + (int)offset.X, rectangle.Y + (int)offset.Y, rectangle.Width, rectangle.Height), Color.DarkGray);
                    sb.Draw(player.Weapon.DisplayIcon, new Rectangle(rectangle.X + (int)offset.X, rectangle.Y + (int)offset.Y, rectangle.Width, rectangle.Height), Color.White);
                    if (player.Weapon.CurrentCooldown != player.Weapon.CooldownDuration)
                        sb.Draw(textures.HitboxTexture, new Rectangle(rectangle.X + (int)offset.X, rectangle.Y + (int)offset.Y, rectangle.Width - (rectangle.Width - (int)((rectangle.Width * player.Weapon.CurrentCooldown) / player.Weapon.CooldownDuration)), rectangle.Height), Color.Gray);

                    sb.Draw(textures.IconUI, new Rectangle(rectangle.X + (int)offset.X - 10, rectangle.Y + (int)offset.Y - 10, rectangle.Width + 20, rectangle.Height + 20), Color.White);
                    break;

                //artifact UI
                case (UIType.Artifact):
                    sb.Draw(textures.HitboxTexture, new Rectangle(rectangle.X + (int)offset.X, rectangle.Y + (int)offset.Y, rectangle.Width, rectangle.Height), Color.DarkGray);
                    if (player.Artifact != null)
                    {
                        sb.Draw(player.Artifact.DisplayIcon, new Rectangle(rectangle.X + (int)offset.X, rectangle.Y + (int)offset.Y, rectangle.Width, rectangle.Height), Color.White);
                        if (player.Artifact.CurrentCooldown != player.Artifact.CooldownDuration)
                            sb.Draw(textures.HitboxTexture, new Rectangle(rectangle.X + (int)offset.X, rectangle.Y + (int)offset.Y, rectangle.Width - (rectangle.Width - (int)((rectangle.Width * player.Artifact.CurrentCooldown) / player.Artifact.CooldownDuration)), rectangle.Height), Color.Gray);
                        if (player.Artifact.InUse)
                            sb.Draw(textures.HitboxTexture, new Rectangle(rectangle.X + (int)offset.X, rectangle.Y + (int)offset.Y, rectangle.Width, rectangle.Height), Color.PaleGoldenrod);
                    }

                    sb.Draw(textures.IconUI, new Rectangle(rectangle.X + (int)offset.X - 10, rectangle.Y + (int)offset.Y - 10, rectangle.Width + 20, rectangle.Height + 20), Color.White);
                    //sb.DrawString(textures.Font, "F", new Vector2(rectangle.X + (int)offset.X + rectangle.Width/4, rectangle.Y + (int)offset.Y + rectangle.Height/4), Color.Black);
                    break;
            }
        }
    }
}