using Quintessential;
namespace AlchemicalInversions
{
    public class MainClass : QuintessentialMod
    {
        public const string ConglomerationPermission = "AlchemicalInversions:conglomeration";
        public override void Load()
        {
            Logger.Log("Loading Inversions! Yay!");
        }

        public override void LoadPuzzleContent()
        {
            Atoms.Initialize();
            Glyphs.Initialize();
            QApi.AddPuzzlePermission(ConglomerationPermission, "Glyph of Conglomeration", "Alchemical Inversions");
        }
        public override void PostLoad()
        {
            Logger.Log("All Done Loading Inversions! Yay!");
        }

        public override void Unload()
        {
            Logger.Log("No More Inversions! Bye Bye!");
        }
    }
}
