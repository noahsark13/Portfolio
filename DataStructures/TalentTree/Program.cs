// Noah Kasper
// Tree Traversal
// 4/3/2023


namespace TreeTraversal
{
    internal class Program
    {
        static void Main(string[] args)
        {
            TalentTreeNode root = new TalentTreeNode("Single stab", true);

            // 2nd layer
            TalentTreeNode advancedStab = new TalentTreeNode("Advanced stab", true);
            TalentTreeNode deepSlash = new TalentTreeNode("Deep slash", true);

            // 3rd layer

            // left side
            TalentTreeNode thousandCuts = new TalentTreeNode("Thousand cuts", false);
            TalentTreeNode piercingThrust = new TalentTreeNode("Piercing thrust", false);

            // right side
            TalentTreeNode puncturingBlow = new TalentTreeNode("Puncturing blow", false);
            TalentTreeNode serratedWound = new TalentTreeNode("Serrated wound", true);

            root.Left = advancedStab;
            root.Right = deepSlash;

            advancedStab.Left = thousandCuts;
            advancedStab.Right = piercingThrust;

            deepSlash.Left = puncturingBlow;
            deepSlash.Right = serratedWound;

            root.ListAllAbilities();

            Console.WriteLine();

            root.ListKnownAbilities();

            Console.WriteLine();

            root.ListPossibleAbilities();
        }
    }
}