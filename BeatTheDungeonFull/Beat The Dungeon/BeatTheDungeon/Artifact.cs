using Microsoft.Xna.Framework;

//Group 1
//Artifact Item that holds the data for a specific artifact
//types of artifact
public enum ArtifactType
{
    SteelHeart,
    SwiftFoot
}

namespace BeatTheDungeon
{

    internal class Artifact : Item
    {
        //Fields
        private ArtifactType artifactType;
        private bool inUse;

        private double buffDuration;
        private double buffTimer;

        private string name;
        private string description;



        //Properties
        /// <summary>
        /// gets artifact type
        /// </summary>
        public ArtifactType ArtifactType { get => artifactType; }
        /// <summary>
        /// gets and sets if the artifact is in use
        /// </summary>
        public bool InUse { get => inUse; set => inUse = value; }
        /// <summary>
        /// gets and sets the duration of the buff 
        /// </summary>
        public double BuffDuration { get => buffDuration; set => buffDuration = value; }
        /// <summary>
        /// gets and sets how long the artifact lasts for
        /// </summary>
        public double BuffTimer { get => buffTimer; set => buffTimer = value; }
        /// <summary>
        /// gets and sets the name
        /// </summary>
        public string Name { get => name; }
        /// <summary>
        /// gets and sets the description
        /// </summary>
        public string Description { get => description; }

        /// <summary>
        /// basic constructor that sets up all the necessary data for the artifact
        /// </summary>
        /// <param name="cooldown">cooldown for using the artifact</param>
        /// <param name="artifactType">type of artifact</param>
        /// <param name="textures">texture</param>
        /// <param name="buffDuration">how long the artifact lasts</param>
        public Artifact(int cooldown, ArtifactType artifactType, Texture textures, double buffDuration) : base(cooldown)
        {
            this.artifactType = artifactType;
            this.inUse = false;
            this.buffDuration = buffDuration;
            this.buffTimer = buffDuration;

            switch (artifactType)
            {
                case (ArtifactType.SwiftFoot):
                    {
                        DisplayIcon = textures.IconTextures["swift"];
                        name = "Swift Foot";
                        description = "Dash";
                        break;
                    }
                case (ArtifactType.SteelHeart):
                    {
                        DisplayIcon = textures.IconTextures["steel"];
                        name = "Steel Heart";
                        description = "Invincibility";
                        break;
                    }
            }
        }



        //checks the type of the artifact and then runs the code associated with it

        /// <summary>
        /// Uses the artifact
        /// </summary>
        /// <param name="player"></param>
        public void Use(Player player)
        {
            switch (artifactType)
            {
                case ArtifactType.SwiftFoot:
                    {

                        player.Speed = 8;

                        break;
                    }
                case ArtifactType.SteelHeart:
                    {
                        player.Speed = 2;
                        break;
                    }
            }

            inUse = true;
        }

        /// <summary>
        /// Resets the values altered by the artifact
        /// </summary>
        /// <param name="player"></param>
        public void Reset(Player player)
        {
            inUse = false;

            switch (artifactType)
            {
                case ArtifactType.SwiftFoot:
                    {
                        player.Speed = 5;

                        break;
                    }
                case ArtifactType.SteelHeart:
                    {
                        player.Speed = 5;
                        break;
                    }
            }
        }
    }
}
