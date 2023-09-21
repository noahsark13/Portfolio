using Microsoft.Xna.Framework.Graphics;

//Group 1
//Class that handles the items (such as weapons or artifacts)

namespace BeatTheDungeon
{
    internal abstract class Item
    {
        //Fields
        private double cooldownDuration;
        private double currentCooldown;
        private bool onCooldown;

        private Texture2D displayIcon;

        //Properties
        //cooldown for using the item
        public double CooldownDuration { get => cooldownDuration; }

        public double CurrentCooldown { get => currentCooldown; set => currentCooldown = value; }

        public Texture2D DisplayIcon { get => displayIcon; set => displayIcon = value; }

        public bool OnCooldown { get => onCooldown; set => onCooldown = value; }

        /// <summary>
        /// basic constructor that is used to make a basic item
        /// </summary>
        /// <param name="cooldown">cooldown for using the item</param>
        public Item(double cooldownDuration)
        {
            this.cooldownDuration = cooldownDuration;
            this.currentCooldown = cooldownDuration;
            onCooldown = false;


        }
    }
}
