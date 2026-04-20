using Quintessential;
using System;
namespace AlchemicalInversions
{
    public class MainClass : QuintessentialMod
    {
        public const string ConglomerationPermission = "AlchemicalInversions:conglomeration";
        public const string RecessionPermission = "AlchemicalInversions:recession";
        public const string TransposalPermission = "AlchemicalInversions:transposal";
        public const string CorrosionPermission = "AlchemicalInversions:corrosion";
        public const string ConcatenationPermission = "AlchemicalInversions:concatenation";
        public override void Load()
        {
            Logger.Log("Loading Inversions! Yay!");
        }

        public override void LoadPuzzleContent()
        {
            Atoms.Initialize();
            Glyphs.Initialize();
            LoadHooks();
            QApi.AddPuzzlePermission(ConglomerationPermission, "Glyph of Conglomeration", "Alchemical Inversions");
            QApi.AddPuzzlePermission(RecessionPermission, "Glyph of Recession", "Alchemical Inversions");
            QApi.AddPuzzlePermission(TransposalPermission, "Glyph of Transposal", "Alchemical Inversions");
            QApi.AddPuzzlePermission(CorrosionPermission, "Glyph of Corrosion", "Alchemical Inversions");
            QApi.AddPuzzlePermission(ConcatenationPermission, "Glyph of Concatenation", "Alchemical Inversions");
        }

        private void LoadHooks()
        {
            On.SolutionEditorScreen.method_50 += sesMethod50;
        }

        private void RemoveHooks()
        {
            On.SolutionEditorScreen.method_50 -= sesMethod50;
        }

        private void sesMethod50(On.SolutionEditorScreen.orig_method_50 orig, SolutionEditorScreen self, float param_5703)
        {
            ChiralityFlip.SolutionEditorScreen_method_50(self);
            orig(self, param_5703);
        }

        public override void PostLoad()
        {
            Logger.Log("All Done Loading Inversions! Yay!");
        }

        public override void Unload()
        {
            RemoveHooks();
            Logger.Log("No More Inversions! Bye Bye!");
        }
    }
}
