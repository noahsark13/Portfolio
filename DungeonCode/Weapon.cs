using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

//Group 1
//Weapon Item that holds the data for a specific weapon
public enum WeaponType
{
    // Common
    SingleFireBall,
    DivineFist,
    LightPiercer,
    // Rare
    TripleFireBall,
    ElBombino,
    TempestWinds,
    // Legendary
    RapidFireDestructionDeath


}

namespace BeatTheDungeon
{
    /// <summary>
    /// Represents a usable weapon to attack enemies.
    /// </summary>
    internal class Weapon : Item
    {
        //Fields
        private int damage;
        private int size;
        private double speed;
        private int numOfProjectiles;
        private int manaUsed;
        private int range;
        private string name;

        private Texture2D weaponTexture;
        private Texture2D hitboxTexture;

        private WeaponType weapon;

        //Properties
        //damage that the weapon does
        public int Damage { get => damage; }

        //size of the projectiles
        public int Size { get => size; }
        //speed of the projectiles
        public double Speed { get => speed; }
        //number of the projectiles fired
        public int NumOfProjectiles { get => numOfProjectiles; }
        //mana that the gun will use each shot
        public int ManaUsed { get => manaUsed; }
        //distance that the projectiles will go before disappearing
        public int Range { get => range; }
        public Texture2D WeaponTexture { get => weaponTexture; }
        public string Name { get => name; }

        public MouseState oldMouse;


        /// <summary>
        /// Weapon class constructor, sets up the weapon depending on its enum classification.
        /// </summary>
        /// <param name="weapon">The weapon type</param>
        /// <param name="textures">The texture class object</param>
        /// <param name="cooldown">cooldown of the weapon</param>
        public Weapon(WeaponType weapon, Texture textures, int damage, double cooldown) : base(cooldown)
        {
            this.weapon = weapon;
            this.hitboxTexture = textures.HitboxTexture;
            this.damage = damage;
            
            // Sets textures depending on the weapon type
            switch (weapon)
            {
                case WeaponType.SingleFireBall:
                    {
                        weaponTexture = textures.WeaponTextures["fireball"];
                        DisplayIcon = textures.IconTextures["singleFireball"];

                        name = "Fireball";
                        break;
                    }
                case WeaponType.TripleFireBall:
                    {
                        weaponTexture = textures.WeaponTextures["fireball"];
                        DisplayIcon = textures.IconTextures["tripleFireball"];

                        name = "Triple Fireball";
                        break;
                    }
                case WeaponType.DivineFist:
                    {
                        weaponTexture = textures.WeaponTextures["slash"];
                        DisplayIcon = textures.WeaponTextures["slash"];
                        name = "Divine Fist";
                        break;
                    }
                case WeaponType.ElBombino:
                    {
                        weaponTexture = textures.WeaponTextures["bomba"];
                        DisplayIcon = textures.IconTextures["bomba"];
                        name = "El Bombino";
                        break;
                    }
                case WeaponType.LightPiercer:
                    {
                        weaponTexture = textures.WeaponTextures["light"];
                        DisplayIcon = textures.IconTextures["light"];

                        name = "Light Piercer";
                        break;
                    }
                case WeaponType.TempestWinds:
                    {
                        weaponTexture = textures.WeaponTextures["wind"];
                        DisplayIcon = textures.IconTextures["wind"];

                        name = "Tempest Winds";
                        break;
                    }
                case WeaponType.RapidFireDestructionDeath:
                    {
                        weaponTexture = textures.WeaponTextures["death"];
                        DisplayIcon = textures.IconTextures["death"];

                        name = "Rapid Fire";
                        break;
                    }
                default:
                    {
                        weaponTexture = hitboxTexture;
                        DisplayIcon = textures.HitboxTexture;
                        break;
                    }
            }
        }

        /// <summary>
        /// Uses the weapon.
        /// </summary>
        /// <param name="projectiles">The projectiles list</param>
        /// <param name="player">The player using the weapon</param>
        /// <param name="dungeon">The dungeon which holds the projectiles</param>
        public void UseWeapon(List<Projectile> projectiles, Player player, Dungeon dungeon)
        {

            MouseState ms = Mouse.GetState();

            // The special weapon
            if (ms.LeftButton == ButtonState.Pressed && weapon == WeaponType.RapidFireDestructionDeath)
            {
                Projectile p = new Projectile(hitboxTexture, weaponTexture, new Rectangle((int)player.PlayerCenter.X, (int)player.PlayerCenter.Y + 20, 20, 20), player.GetAngle(-20, 20), 8.0, damage, 3f);

                projectiles.Add(p);
            }
            else if (ms.LeftButton == ButtonState.Pressed && oldMouse.LeftButton != ButtonState.Pressed && OnCooldown == false)
            {
                // Different weapon settings depending on the weapon.
                switch (weapon)
                {
                    case WeaponType.SingleFireBall:
                        {
                            // Single Click

                            // Projectile created on click: Width & Height defines the size of the projectile. Optimal to use a equal width and height
                            // as the hitbox for unequal sprites.

                            Projectile p = new Projectile(hitboxTexture, weaponTexture, new Rectangle((int)player.PlayerCenter.X, (int)player.PlayerCenter.Y + 20, 20, 20), player.GetAngle(-20, 20), 8.0, damage, 10f);

                            projectiles.Add(p);


                            break;

                        }

                    case WeaponType.TripleFireBall:
                        {
                            // Shoots 3 fireballs
                            Projectile p = new Projectile(hitboxTexture, weaponTexture, new Rectangle((int)player.PlayerCenter.X, (int)player.PlayerCenter.Y + 20, 20, 20), player.GetAngle(-20, 20), 8.0, damage, 6f);
                            Projectile q = new Projectile(hitboxTexture, weaponTexture, new Rectangle((int)player.PlayerCenter.X, (int)player.PlayerCenter.Y + 20, 20, 20), player.GetAngle(-20, 20) + 0.523599f, 8.0, damage, 6f);
                            Projectile r = new Projectile(hitboxTexture, weaponTexture, new Rectangle((int)player.PlayerCenter.X, (int)player.PlayerCenter.Y + 20, 20, 20), player.GetAngle(-20, 20) - 0.523599, 8.0, damage, 6f);
                            projectiles.Add(p);
                            projectiles.Add(q);
                            projectiles.Add(r);

                            break;
                        }
                    case WeaponType.DivineFist:
                        {

                            Projectile p = new Projectile(hitboxTexture, weaponTexture, new Rectangle((int)player.PlayerCenter.X, (int)player.PlayerCenter.Y + 20, 80, 80), player.GetAngle(0, 0), 14, damage, 2);

                            // Changes sprite effect depending on the direction of your mouse.
                            if (player.PlayerDirection == SpriteEffects.FlipHorizontally)
                            {

                                p.SpriteEffect = SpriteEffects.FlipVertically;
                            }
                            else
                            {
                                p.SpriteEffect = SpriteEffects.None;
                            }


                            projectiles.Add(p);



                            break;
                        }
                    case WeaponType.ElBombino:
                        {
                            Projectile p = new Projectile(hitboxTexture, weaponTexture, new Rectangle((int)player.PlayerCenter.X, (int)player.PlayerCenter.Y + 20, 50, 50), player.GetAngle(-20, 20), 0.01, damage, 30f);

                            projectiles.Add(p);

                            break;
                        }
                    case WeaponType.LightPiercer:
                        {
                            Projectile p = new Projectile(hitboxTexture, weaponTexture, new Rectangle((int)player.PlayerCenter.X, (int)player.PlayerCenter.Y + 20, 20, 20), player.GetAngle(-20, 20), 12, damage, 10f);
                            Projectile q = new Projectile(hitboxTexture, weaponTexture, new Rectangle((int)player.PlayerCenter.X, (int)player.PlayerCenter.Y + 20, 20, 20), player.GetAngle(-20, 20) + 3.14, 12, damage, 10f);

                            projectiles.Add(p);
                            projectiles.Add(q);

                            break;
                        }
                    case WeaponType.TempestWinds:
                        {
                            for (int i = 1; i <= 12; i++)
                            {
                                Projectile p = new Projectile(hitboxTexture, weaponTexture, new Rectangle((int)player.PlayerCenter.X, (int)player.PlayerCenter.Y + 20, 20, 20), player.GetAngle(-20, 20) + (0.523599 * i), 1, damage, 3.5f);
                                projectiles.Add(p);
                            }

                            break;
                        }

                }

                this.OnCooldown = true;
            }

            // Updates projectile position, and current timer
            for (int x = 0; x < projectiles.Count; x++)
            {
                projectiles[x].FireProjectile(weapon);
                projectiles[x].Timer -= 0.1f;


                // Removes when timer is 0.
                if (projectiles[x].Timer <= 0)
                {
                    projectiles.Remove(projectiles[x]);
                }
                // Otherwise will remove when collision occurs
                else
                {
                    foreach (Block b in dungeon.Blocks)
                    {
                        if (projectiles[x].Collision(b))
                        {
                            if (b.Type == BlockType.Fire || b.Type == BlockType.Spown)
                            {
                                // will have method later in design
                            }
                            else
                            {
                                projectiles.Remove(projectiles[x]);
                            }
                            break;
                        }
                    }
                }



            }

            oldMouse = ms;

        }
    }
}