using Microsoft.Xna.Framework;

public enum PotionType
{
    Rejuvinating,
    Life,

}

namespace BeatTheDungeon
{
    internal class Potion : GameObject
    {
        private PotionType type;

        public Potion(PotionType type, Texture textures, int x, int y) : base(null, new Rectangle(x, y, 40, 40))
        {
            this.type = type;

            switch (type)
            {
                case PotionType.Rejuvinating:
                    {
                        Texture = textures.IconTextures["redPotion"];
                        break;
                    }
                case PotionType.Life:
                    {
                        Texture = textures.IconTextures["goldPotion"];
                        break;
                    }

            }
        }

        public void UsePotion(Dungeon dungeon, Player player)
        {
            if (this.Collision(player))
            {


                switch (type)
                {
                    case PotionType.Rejuvinating:
                        {
                            if (player.Health < player.MaxHealth)
                            {
                                player.Health += 20;

                                if (player.Health > player.MaxHealth)
                                {
                                    player.Health = player.MaxHealth;
                                }

                            }

                            break;
                        }
                    case PotionType.Life:
                        {
                            player.MaxHealth += 20;
                            break;
                        }

                }

                dungeon.Potions.Remove(this);
            }
        }
    }
}
