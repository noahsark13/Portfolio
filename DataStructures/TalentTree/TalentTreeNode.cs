using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TreeTraversal
{
    internal class TalentTreeNode
    {
        private string name;
        private bool hasLearned;
        private TalentTreeNode left;
        private TalentTreeNode right;

        /// <summary>
        /// Gets and sets the left node which follows the current node.
        /// </summary>
        public TalentTreeNode Left { get { return left; } set { left = value; } }

        /// <summary>
        /// Gets and sets the right node which follows the current node.
        /// </summary>
        public TalentTreeNode Right { get { return right; } set { right = value; } }

        /// <summary>
        /// Tree Node constructor, set's the left and right value to null.
        /// </summary>
        /// <param name="name">The name of the ability</param>
        /// <param name="hasLearned">A boolean value which determines if the skill has already been learned</param>
        public TalentTreeNode(string name, bool hasLearned)
        {
            this.name = name;
            this.hasLearned = hasLearned;
            left = null;
            right = null;
        }

        /// <summary>
        /// Prints all the abilities in the tree, using the basic tree traversal.
        /// </summary>
        public void ListAllAbilities()
        {
            // left - current - right pattern

            if (this.Left != null)
            {
                Left.ListAllAbilities();
            }

            Console.WriteLine(name);

            if (this.Right != null)
            {
                Right.ListAllAbilities();
            }
        }

        /// <summary>
        /// Prints only the abilities which have been learned.
        /// </summary>
        public void ListKnownAbilities()
        {
            // Checks if ability is learned, then prints it, then continues to traverse checking if the following nodes are learned.
            if (this.hasLearned)
            {
                Console.WriteLine(name);

                if (Left != null)
                {
                    Left.ListKnownAbilities();
                }

                if (Right != null)
                {
                    Right.ListKnownAbilities();
                }

            }
        }

        /// <summary>
        /// Prints only abilities which can currently be learned; meaning abilities which are currently not learned but come
        /// directly after a ability which has been learned.
        /// </summary>
        public void ListPossibleAbilities()
        {
            // Checks if current node has been learned already
            if (this.hasLearned)
            {

                if (Left != null)
                {
                    Left.ListPossibleAbilities();
                }

                if (Right != null)
                {
                    Right.ListPossibleAbilities();
                }

            }
            // Finds that the next node after a learned node is not learned, and thus prints the name.
            else
            {
                Console.WriteLine(name);
            }
            


        }
    }
}
