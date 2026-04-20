using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quintessential;
using PartType = class_139;
using Texture = class_256;

namespace AlchemicalInversions
{
    public static class Glyphs
    {
        public static PartType Conglomeration;
        public static HexIndex ConglomerationInput1 = new HexIndex(0, 0);
        public static HexIndex ConglomerationInput2 = new HexIndex(1, 0);
        public static HexIndex ConglomerationOutput = new HexIndex(0, 1);

        public static PartType Recession;
        public static HexIndex RecessionOutput1 = new HexIndex(0, 0);
        public static HexIndex RecessionInput1 = new HexIndex(1, 0);
        public static HexIndex RecessionInput2 = new HexIndex(2, 0);
        public static HexIndex RecessionOutput2 = new HexIndex(3, 0);

        public static PartType Transposal;
        public static HexIndex TransposalInput = new HexIndex(0, 0);
        public static HexIndex TransposalBowl = new HexIndex(1, 0);

        public static PartType Corrosion;
        public static HexIndex CorrosionInput1 = new HexIndex(-1, 0);
        public static HexIndex CorrosionOutput = new HexIndex(0, 0);
        public static HexIndex CorrosionInput2 = new HexIndex(1, 0);

        public static PartType Concatenation;
        public static HexIndex ConcatenationConduit1 = new HexIndex(0, 0);
        public static HexIndex ConcatenationConduit2 = new HexIndex(1, 0);
        public static HexIndex ConcatenationYttriumInput = new HexIndex(2, 0);
        public static HexIndex ConcatenationOutput = new HexIndex(-1, 1);

        public static PartType FlippyConcatenation;
        public static HexIndex FlippyConcatenationConduit1 = new HexIndex(1, 0);
        public static HexIndex FlippyConcatenationConduit2 = new HexIndex(2, 0);
        public static HexIndex FlippyConcatenationYttriumInput = new HexIndex(0, 0);
        public static HexIndex FlippyConcatenationOutput = new HexIndex(2, 1);



        public static class Textures
        {
            public static Texture ConglomerationAnimismusSymbol = Brimstone.API.GetTexture("textures/parts/Directrix777/AlchemicalInversions/Conglomeration/conglomeration_animismus_symbol");
            public static Texture MetalSymbol = Brimstone.API.GetTexture("textures/parts/Directrix777/AlchemicalInversions/Recession/recession_metal_symbol");
            public static Texture TransposalTenebrivexSymbol = Brimstone.API.GetTexture("textures/parts/Directrix777/AlchemicalInversions/Transposal/transposal_tenebrivex_symbol");
            public static Texture CorrosionBase = Brimstone.API.GetTexture("textures/parts/Directrix777/AlchemicalInversions/Corrosion/corrosion_base");
            public static Texture ConcatenationYttriumSymbol = Brimstone.API.GetTexture("textures/parts/Directrix777/AlchemicalInversions/Concatenation/concatenation_yttrium_symbol");

        }
        private static int GetMetallicity(AtomType atomtype)
        {
            if (!API.metals.Contains(atomtype))
            {
                Logger.Log("You tried to get the metallicity of a non-metal, wtf???");
                return -777;
            }
            else
            {
                for (int i = 0; i < API.metals.Length; i++)
                {
                    if (API.metals[i] == atomtype)
                    {
                        return i - 6;
                    }
                }
                Logger.Log("Somehow the metal isn't a metal anymore, dog, idk what to tell you.");
                return -777;
            }
        }

        private static void ConcatenationBehave(Sim sim, bool first, PartSimState pss, Part part, SolutionEditorBase seb, HexIndex c1, HexIndex c2, HexIndex c3, HexIndex cOutput)
        {
            if (first)
            {
                if (sim.FindAtomRelative(part, cOutput).method_1085())
                {
                    //output blocked
                    return;
                }
                if (!sim.FindAtomRelative(part, c1).method_99(out AtomReference i1) || !sim.FindAtomRelative(part, c2).method_99(out AtomReference i2) || !sim.FindAtomRelative(part, c3).method_99(out AtomReference i3))
                {
                    //not enough atoms on inputs
                    return;
                }
                if (!i1.field_2281 || i1.field_2282 || !i2.field_2281 || i2.field_2282 || i3.field_2281 || i3.field_2282)
                {
                    //any input atom is held or Yttrium is a molecule or metals are NOT molecule
                    return;
                }
                if (!API.metals.Contains(i1.field_2280) || !API.metals.Contains(i2.field_2280) || i3.field_2280 != Atoms.Yttrium)
                {
                    //at least one atom type is wrong
                    return;
                }
                Molecule bondedPair = i1.field_2277;
                IReadOnlyList<class_277> bondList = bondedPair.method_1101();
                if (bondedPair != i2.field_2277 || bondedPair.method_1100().Count != 2/*not two atoms in molecule*/ || bondList.Count != 1/*not just one bond*/)
                {
                    return;
                }
                class_277 bond = bondList[0];
                if (bond.field_2186 != enum_126.Standard)
                {
                    return;
                }
                if (!((bond.field_2187 == i1.field_2278 && bond.field_2188 == i2.field_2278) || (bond.field_2187 == i2.field_2278 && bond.field_2188 == i1.field_2278)))
                {
                    return;
                }
                int totalMetallicity = GetMetallicity(i1.field_2280) + GetMetallicity(i2.field_2280);
                if (totalMetallicity >= -6 && totalMetallicity <= 6)
                {
                    //total metallicity is within acceptable range, ready to fire!
                    sim.field_3823.Remove(bondedPair);//remove molecule from simulation!
                    Brimstone.API.RemoveAtom(i3);//deletes atoms
                    Brimstone.API.DrawFallingAtom(seb, i3);
                    Brimstone.API.AddSmallCollider(sim, part, cOutput);//collision of atom entering
                    pss.field_2743 = true; //sets it to active
                    pss.field_2744 = new AtomType[1] { API.metals[totalMetallicity + 6] };
                    pss.field_2732.Add(bondedPair);
                }
            }
            else if (pss.field_2743)//if going to fire
            {
                Brimstone.API.AddAtom(sim, part, cOutput, pss.field_2744[0]);//make atom!
                pss.field_2732.Clear();
            }
        }

        public static void ConcatenationDraw(Part part, Vector2 pos, SolutionEditorBase editor, class_195 renderer, HexIndex c1, HexIndex c2, HexIndex c3, HexIndex cOutput)//how the glyphs look
        {
            PartSimState pss = editor.method_507().method_481(part);
            float time = editor.method_504();
            class_236 uco = editor.method_1989(part, pos);
            foreach (var h in new HexIndex[] { c1, c2 })
            {
                renderer.method_528(class_238.field_1989.field_90.field_200, h, Vector2.Zero);//renders conduit rings
                renderer.method_529(Textures.MetalSymbol, h, Vector2.Zero);
            }
            renderer.method_526(class_238.field_1989.field_90.field_196, c1, new Vector2(0, 0), new Vector2(-23, 23), 0);//renders conduit bond
            renderer.method_528(class_238.field_1989.field_90.field_255.field_293, c3, Vector2.Zero);//renders holes
            renderer.method_529(Textures.ConcatenationYttriumSymbol, c3, Vector2.Zero);//renders symbol
            renderer.method_528(class_238.field_1989.field_90.field_228.field_272, cOutput, Vector2.Zero);//renders void under iris

            int irisFrame = 15;
            bool afterIrisOpens = false;
            Molecule risingAtom = null;
            Vector2 risingOffset = uco.field_1984 + class_187.field_1742.method_492(cOutput).Rotated(uco.field_1985);
            if (pss.field_2743)
            {
                irisFrame = class_162.method_404((int)(class_162.method_411(1, -1, time) * 16), 0, 15);
                afterIrisOpens = time > 0.5;
                risingAtom = Molecule.method_1121(pss.field_2744[0]);
                if (!afterIrisOpens)
                {
                    Editor.method_925(risingAtom, risingOffset, new HexIndex(0, 0), 0, 1, time, 1, false, null);
                }
            }
            renderer.method_529(class_238.field_1989.field_90.field_246[irisFrame], cOutput, Vector2.Zero);//renders current iris frame
            renderer.method_528(class_238.field_1989.field_90.field_228.field_271, cOutput, Vector2.Zero);//renders rim above iris
            if (pss.field_2743 && afterIrisOpens)
            {
                Editor.method_925(risingAtom, risingOffset, new HexIndex(0, 0), 0, 1, time, 1, false, null);
            }
            foreach (Molecule m in pss.field_2732)
            {
                Editor.method_925(m, pos, new HexIndex(0, 0), 0f, 1f, 1f - time, 1, false, null);
                Editor.method_925(m, pos, new HexIndex(0, 0), 0f, 1f, 1f - time, 1, false, null);
            }
        }
        public static void Initialize()
        {
            API.Initialize();
            Conglomeration = Brimstone.API.CreateSimpleGlyph(
                ID: "alchemical-inversions-conglomeration",
                name: "Glyph of Conglomeration",
                description: "The glyph of Conglomeration consumes one Vitae and one Mors atom to create an atom of Tenebrivex.",
                cost: 20,
                glow: class_238.field_1989.field_97.field_386,
                stroke: class_238.field_1989.field_97.field_387,
                icon: class_238.field_1989.field_90.field_245.field_319, //Placeholder, delete once this glyph has a proper texture
                                                                         //icon: Brimstone.API.GetTexture("textures/parts/Directrix777/AlchemicalInversions/Conglomeration/conglomeration_icon"),
                hoveredIcon: Brimstone.API.GetTexture("textures/parts/Directrix777/AlchemicalInversions/Conglomeration/conglomeration_hover"),
                usedHexes: new HexIndex[]
                {
                    ConglomerationInput1,
                    ConglomerationInput2,
                    ConglomerationOutput
                },
                customPermission: MainClass.ConglomerationPermission
                );
            QApi.AddPartTypeToPanel(Conglomeration, false);
            QApi.AddPartType(Conglomeration, static (part, pos, editor, renderer) =>
            {
                PartSimState pss = editor.method_507().method_481(part);
                float time = editor.method_504();
                class_236 uco = editor.method_1989(part, pos);
                Vector2 offset = new Vector2(41, 48);
                renderer.method_523(class_238.field_1989.field_90.field_257.field_359, new Vector2(-1, -1), offset, 0);//renders base of glyph
                foreach (var h in new HexIndex[] { ConglomerationInput1, ConglomerationInput2 })
                {
                    renderer.method_528(class_238.field_1989.field_90.field_255.field_293, h, Vector2.Zero);//renders holes
                    renderer.method_529(Textures.ConglomerationAnimismusSymbol, h, Vector2.Zero);//renders holes
                }
                renderer.method_528(class_238.field_1989.field_90.field_228.field_272, ConglomerationOutput, Vector2.Zero);//renders void under iris
                int irisFrame = 15;
                bool afterIrisOpens = false;
                Molecule risingAtom = null;
                Vector2 risingOffset = uco.field_1984 + class_187.field_1742.method_492(ConglomerationOutput).Rotated(uco.field_1985);
                if (pss.field_2743)
                {
                    irisFrame = class_162.method_404((int)(class_162.method_411(1, -1, time) * 16), 0, 15);
                    afterIrisOpens = time > 0.5;
                    risingAtom = Molecule.method_1121(pss.field_2744[0]);
                    if (!afterIrisOpens)
                    {
                        Editor.method_925(risingAtom, risingOffset, new HexIndex(0, 0), 0, 1, time, 1, false, null);
                    }
                }
                renderer.method_529(class_238.field_1989.field_90.field_246[irisFrame], ConglomerationOutput, Vector2.Zero);//renders current iris frame
                renderer.method_528(class_238.field_1989.field_90.field_228.field_271, ConglomerationOutput, Vector2.Zero);//renders rim above iris
                if (pss.field_2743 && afterIrisOpens)
                {
                    Editor.method_925(risingAtom, risingOffset, new HexIndex(0, 0), 0, 1, time, 1, false, null);
                }
            }
            );

            Recession = Brimstone.API.CreateSimpleGlyph(
                ID: "alchemical-inversions-recession",
                name: "Glyph of Recession",
                description: "The glyph of Recession takes in two metals and redistributes their metalicity, rendering them as close to equivalent as possible.",
                cost: 20,
                glow: class_238.field_1989.field_97.field_374,
                stroke: class_238.field_1989.field_97.field_375,
                icon: class_238.field_1989.field_90.field_245.field_319, //Placeholder, delete once this glyph has a proper texture
                                                                         //icon: Brimstone.API.GetTexture("textures/parts/Directrix777/AlchemicalInversions/recession"),
                hoveredIcon: Brimstone.API.GetTexture("textures/parts/Directrix777/AlchemicalInversions/conglomeration_hover"),
                usedHexes: new HexIndex[]
                {
                    RecessionOutput1,
                    RecessionInput1,
                    RecessionInput2,
                    RecessionOutput2
                },
                customPermission: MainClass.RecessionPermission
                );
            QApi.AddPartTypeToPanel(Recession, false);
            QApi.AddPartType(Recession, static (part, pos, editor, renderer) =>//how the glyphs look
            {
                PartSimState pss = editor.method_507().method_481(part);
                float time = editor.method_504();
                class_236 uco = editor.method_1989(part, pos);
                Vector2 offset = new Vector2(41, 48);
                renderer.method_523(class_238.field_1989.field_90.field_255.field_288, new Vector2(-1, -1), offset, 0);//renders base of projection glyph
                renderer.method_523(class_238.field_1989.field_90.field_255.field_288, new Vector2(-1, -1), offset + new Vector2(246, 0), (float)Math.PI);//renders base of projection glyph
                foreach (var h in new HexIndex[] { RecessionInput1, RecessionInput2 })
                {
                    renderer.method_528(class_238.field_1989.field_90.field_255.field_293, h, Vector2.Zero);//renders holes
                    renderer.method_529(Textures.MetalSymbol, h, Vector2.Zero);
                }
                renderer.method_528(class_238.field_1989.field_90.field_228.field_272, RecessionOutput1, Vector2.Zero);//renders void under iris
                renderer.method_528(class_238.field_1989.field_90.field_228.field_272, RecessionOutput2, Vector2.Zero);//renders void under iris

                int irisFrame = 15;
                bool afterIrisOpens = false;
                Molecule risingAtom1 = null;
                Vector2 risingOffset1 = uco.field_1984 + class_187.field_1742.method_492(RecessionOutput1).Rotated(uco.field_1985);
                Molecule risingAtom2 = null;
                Vector2 risingOffset2 = uco.field_1984 + class_187.field_1742.method_492(RecessionOutput2).Rotated(uco.field_1985);
                if (pss.field_2743)
                {
                    irisFrame = class_162.method_404((int)(class_162.method_411(1, -1, time) * 16), 0, 15);
                    afterIrisOpens = time > 0.5;
                    risingAtom1 = Molecule.method_1121(pss.field_2744[0]);
                    risingAtom2 = Molecule.method_1121(pss.field_2744[1]);
                    if (!afterIrisOpens)
                    {
                        Editor.method_925(risingAtom1, risingOffset1, new HexIndex(0, 0), 0, 1, time, 1, false, null);
                        Editor.method_925(risingAtom2, risingOffset2, new HexIndex(0, 0), 0, 1, time, 1, false, null);
                    }
                }
                renderer.method_529(Brimstone.API.GetAnimation("textures/parts/Directrix777/AlchemicalInversions/Recession/recession_iris.array", "iris_up", 16)[irisFrame], RecessionOutput1, Vector2.Zero);//renders current iris frame
                renderer.method_528(class_238.field_1989.field_90.field_228.field_271, RecessionOutput1, Vector2.Zero);//renders rim above iris
                renderer.method_529(class_238.field_1989.field_90.field_246[irisFrame], RecessionOutput2, Vector2.Zero);
                renderer.method_528(class_238.field_1989.field_90.field_228.field_271, RecessionOutput2, Vector2.Zero);
                if (pss.field_2743 && afterIrisOpens)
                {
                    Editor.method_925(risingAtom1, risingOffset1, new HexIndex(0, 0), 0, 1, time, 1, false, null);
                    Editor.method_925(risingAtom2, risingOffset2, new HexIndex(0, 0), 0, 1, time, 1, false, null);
                }
            }
            );

            Transposal = Brimstone.API.CreateSimpleGlyph(
                ID: "alchemical-inversions-transposal",
                name: "Glyph of Transposal",
                description: "The glyph of Transposal consumes an atom of Tenebrivex to invert a compatible atom on its other end.",
                cost: 20,
                glow: class_238.field_1989.field_97.field_374,
                stroke: class_238.field_1989.field_97.field_375,
                icon: class_238.field_1989.field_90.field_245.field_319, //Placeholder, delete once this glyph has a proper texture
                                                                         //icon: Brimstone.API.GetTexture("textures/parts/Directrix777/AlchemicalInversions/recession"),
                hoveredIcon: Brimstone.API.GetTexture("textures/parts/Directrix777/AlchemicalInversions/Conglomeration/conglomeration_hover"),
                usedHexes: new HexIndex[]
                {
                    TransposalInput,
                    TransposalBowl
                },
                customPermission: MainClass.TransposalPermission
                );
            QApi.AddPartTypeToPanel(Transposal, false);
            QApi.AddPartType(Transposal, static (part, pos, editor, renderer) =>//how the glyphs look
            {
                PartSimState pss = editor.method_507().method_481(part);
                class_236 uco = editor.method_1989(part, pos);
                Vector2 offset = new Vector2(41, 48);
                renderer.method_523(class_238.field_1989.field_90.field_255.field_288, new Vector2(-1, -1), offset, 0);//renders base of projection glyph
                renderer.method_528(class_238.field_1989.field_90.field_255.field_293, TransposalInput, Vector2.Zero);//renders hole
                renderer.method_529(Textures.TransposalTenebrivexSymbol, TransposalInput, Vector2.Zero);//renders 
                renderer.method_528(class_238.field_1989.field_90.field_170, TransposalBowl, Vector2.Zero); //renders bowl
            }
            );

            Corrosion = Brimstone.API.CreateSimpleGlyph(
                ID: "alchemical-inversions-corrosion",
                name: "Glyph of Corrosion",
                description: "The glyph of Corrosion takes in a base metal and an antimetal, outputting a single atom with their combined metallicity.",
                cost: 20,
                glow: Brimstone.API.GetTexture("textures/parts/Directrix777/AlchemicalInversions/Corrosion/corrosion_glow"),
                stroke: Brimstone.API.GetTexture("textures/parts/Directrix777/AlchemicalInversions/Corrosion/corrosion_stroke"),
                icon: class_238.field_1989.field_90.field_245.field_319, //Placeholder, delete once this glyph has a proper texture
                                                                         //icon: Brimstone.API.GetTexture("textures/parts/Directrix777/AlchemicalInversions/recession"),
                hoveredIcon: Brimstone.API.GetTexture("textures/parts/Directrix777/AlchemicalInversions/conglomeration_hover"),
                usedHexes: new HexIndex[]
                {
                    CorrosionInput1,
                    CorrosionOutput,
                    CorrosionInput2,
                },
                customPermission: MainClass.CorrosionPermission
                );
            QApi.AddPartTypeToPanel(Corrosion, false);
            QApi.AddPartType(Corrosion, static (part, pos, editor, renderer) =>//how the glyphs look
            {
                PartSimState pss = editor.method_507().method_481(part);
                float time = editor.method_504();
                class_236 uco = editor.method_1989(part, pos);
                Vector2 offset = new Vector2(41, 48);
                renderer.method_523(Textures.CorrosionBase, new Vector2(-1, -1), offset + new Vector2(86, 3.75f), 0);//renders base of projection glyph
                foreach (var h in new HexIndex[] { CorrosionInput1, CorrosionInput2 })
                {
                    renderer.method_528(class_238.field_1989.field_90.field_255.field_293, h, Vector2.Zero);//renders holes
                    renderer.method_529(Textures.MetalSymbol, h, Vector2.Zero);
                }
                renderer.method_528(class_238.field_1989.field_90.field_228.field_272, CorrosionOutput, Vector2.Zero);//renders void under iris

                int irisFrame = 15;
                bool afterIrisOpens = false;
                Molecule risingAtom = null;
                Vector2 risingOffset = uco.field_1984 + class_187.field_1742.method_492(CorrosionOutput).Rotated(uco.field_1985);
                if (pss.field_2743)
                {
                    irisFrame = class_162.method_404((int)(class_162.method_411(1, -1, time) * 16), 0, 15);
                    afterIrisOpens = time > 0.5;
                    risingAtom = Molecule.method_1121(pss.field_2744[0]);
                    if (!afterIrisOpens)
                    {
                        Editor.method_925(risingAtom, risingOffset, new HexIndex(0, 0), 0, 1, time, 1, false, null);
                    }
                }
                renderer.method_528(class_238.field_1989.field_90.field_228.field_271, CorrosionOutput, Vector2.Zero);//renders rim above iris
                renderer.method_529(class_238.field_1989.field_90.field_246[irisFrame], CorrosionOutput, Vector2.Zero);
                if (pss.field_2743 && afterIrisOpens)
                {
                    Editor.method_925(risingAtom, risingOffset, new HexIndex(0, 0), 0, 1, time, 1, false, null);
                }
            }
            );

            Concatenation = Brimstone.API.CreateSimpleGlyph(
                ID: "alchemical-inversions-concatenation",
                name: "Glyph of Concatenation",
                description: "The glyph of Concatenation takes a Yttrium atom, and a bonded metal pair, and adds the two bonded metals together, outputting their sum.",
                cost: 20,
                glow: class_238.field_1989.field_97.field_374,
                stroke: class_238.field_1989.field_97.field_375,
                icon: class_238.field_1989.field_90.field_245.field_319, //Placeholder, delete once this glyph has a proper texture
                                                                         //icon: Brimstone.API.GetTexture("textures/parts/Directrix777/AlchemicalInversions/recession"),
                hoveredIcon: Brimstone.API.GetTexture("textures/parts/Directrix777/AlchemicalInversions/concatenation_hover"),
                usedHexes: new HexIndex[]
                {
                    ConcatenationConduit1,
                    ConcatenationConduit2,
                    ConcatenationYttriumInput,
                    ConcatenationOutput
                },
                customPermission: MainClass.ConcatenationPermission
                );
            QApi.AddPartTypeToPanel(Concatenation, false);
            QApi.AddPartType(Concatenation, static (part, pos, editor, renderer) =>
            {
                Vector2 offset = new Vector2(41, 48);
                renderer.method_523(class_238.field_1989.field_90.field_255.field_288, new Vector2(-1, -1), offset, ((float)Math.PI * 2) / 3);//renders base of projection glyph
                renderer.method_523(class_238.field_1989.field_90.field_255.field_288, new Vector2(-1, -1), offset + new Vector2(-82, 0), 0);//renders base of projection glyph
                ConcatenationDraw(part, pos, editor, renderer, ConcatenationConduit1, ConcatenationConduit2, ConcatenationYttriumInput, ConcatenationOutput);
            }
            );

            FlippyConcatenation = Brimstone.API.CreateSimpleGlyph(
                ID: "alchemical-inversions-flippy-concatenation",
                name: "Glyph of Concatenation",
                description: "The glyph of Concatenation takes a Yttrium atom, and a bonded metal pair, and adds the two bonded metals together, outputting their sum.",
                cost: 20,
                glow: class_238.field_1989.field_97.field_374,
                stroke: class_238.field_1989.field_97.field_375,
                icon: class_238.field_1989.field_73, //Placeholder, delete once this glyph has a proper texture
                hoveredIcon: class_238.field_1989.field_73,
                usedHexes: new HexIndex[]
                {
                    FlippyConcatenationConduit1,
                    FlippyConcatenationConduit2,
                    FlippyConcatenationYttriumInput,
                    FlippyConcatenationOutput
                },
                customPermission: MainClass.ConcatenationPermission
                );
            QApi.AddPartType(FlippyConcatenation, static (part, pos, editor, renderer) =>
            {
                Vector2 offset = new Vector2(41, 48);
                renderer.method_523(class_238.field_1989.field_90.field_255.field_288, new Vector2(-1, -1), offset + new Vector2(-82, 142), ((float)Math.PI) / 3);//renders base of projection glyph
                renderer.method_523(class_238.field_1989.field_90.field_255.field_288, new Vector2(-1, -1), offset, 0);//renders base of projection glyph
                ConcatenationDraw(part, pos, editor, renderer, FlippyConcatenationConduit1, FlippyConcatenationConduit2, FlippyConcatenationYttriumInput, FlippyConcatenationOutput);
            }
            );

            QApi.RunAfterCycle(static (sim, first) =>//What the glyphs do
            {
                SolutionEditorBase seb = sim.field_3818; //Sim is what controls atoms, runs game. SEB keeps track of editor
                Dictionary<Part, PartSimState> pss = sim.field_3821; //pss is what the glyphs are doing
                List<Part> parts = seb.method_502().field_3919; //list of placed parts
                foreach (var part in parts)
                {
                    PartType type = part.method_1159();
                    if (type == Conglomeration)
                    {
                        if (first)
                        {
                            if (sim.FindAtomRelative(part, ConglomerationOutput).method_1085()) //FindAtomRelative returns an atom, if it finds one at the index. 1085 returns whether the atom has an element as bool.
                            {
                                //output blocked
                                continue;
                            }
                            if (!sim.FindAtomRelative(part, ConglomerationInput1).method_99(out AtomReference i1) || !sim.FindAtomRelative(part, ConglomerationInput2).method_99(out AtomReference i2))
                            {
                                //not enough atoms on inputs
                                continue;
                            }
                            if (i1.field_2281 || i1.field_2282 || i2.field_2281 || i2.field_2282) //81 checks for molecule, 82 checks for if that atom is being held
                            {
                                //atom is being grabbed, or has bonds.
                                continue;
                            }
                            if ((i1.field_2280 == Brimstone.API.VanillaAtoms.vitae && i2.field_2280 == Brimstone.API.VanillaAtoms.mors) || (i2.field_2280 == Brimstone.API.VanillaAtoms.vitae && i1.field_2280 == Brimstone.API.VanillaAtoms.mors))
                            {
                                //atoms are correct! yay!
                                Brimstone.API.RemoveAtom(i1);
                                Brimstone.API.RemoveAtom(i2);//deletes atoms
                                Brimstone.API.DrawFallingAtom(seb, i1);
                                Brimstone.API.DrawFallingAtom(seb, i2);//make atoms fall into respective holes
                                Brimstone.API.AddSmallCollider(sim, part, ConglomerationOutput);//collision of atom entering
                                pss[part].field_2743 = true; //sets it to active
                                pss[part].field_2744 = new AtomType[1] { Atoms.Tenebrivex }; //set atom(s) that will be made, array size is set, each index can be pulled from separately
                            }
                        }
                        else if (pss[part].field_2743)//if going to fire
                        {
                            Brimstone.API.AddAtom(sim, part, ConglomerationOutput, pss[part].field_2744[0]);//make atom!
                        }
                    }
                    else if (type == Recession)
                    {
                        if (first)
                        {
                            if (sim.FindAtomRelative(part, RecessionOutput1).method_1085() || sim.FindAtomRelative(part, RecessionOutput2).method_1085()) //FindAtomRelative returns an atom, if it finds one at the index. 1085 returns whether the atom has an element as bool.
                            {
                                //output blocked
                                continue;
                            }
                            if (!sim.FindAtomRelative(part, RecessionInput1).method_99(out AtomReference i1) || !sim.FindAtomRelative(part, RecessionInput2).method_99(out AtomReference i2))
                            {
                                //not enough atoms on inputs
                                continue;
                            }
                            if (i1.field_2281 || i1.field_2282 || i2.field_2281 || i2.field_2282) //81 checks for molecule, 82 checks for if that atom is being held
                            {
                                //atom is being grabbed, or has bonds.
                                continue;
                            }
                            if (API.metals.Contains(i1.field_2280) && API.metals.Contains(i2.field_2280))
                            {
                                //atoms are correct! yay!
                                int totalmetallicity = GetMetallicity(i1.field_2280) + GetMetallicity(i2.field_2280);
                                AtomType[] outputAtoms = { null, API.metals[(int)totalmetallicity / 2 + 6] }; //Atom on the right will have same metallicity whether even or odd total
                                if (totalmetallicity == 0)
                                {
                                    outputAtoms = new AtomType[] { Brimstone.API.VanillaAtoms.lead, Atoms.AntiLead };
                                }
                                else if ((int)totalmetallicity == 1)
                                {
                                    outputAtoms = new AtomType[] { Brimstone.API.VanillaAtoms.tin, Atoms.AntiLead };
                                }
                                else if ((int)totalmetallicity == -1)
                                {
                                    outputAtoms = new AtomType[] { Brimstone.API.VanillaAtoms.lead, Atoms.AntiTin };
                                }
                                else if (totalmetallicity % 2 == 1)
                                {
                                    outputAtoms[0] = API.metals[(int)totalmetallicity / 2 + 7];
                                }
                                else if (totalmetallicity % 2 == -1)//Atom on right changes cuz negative rounding.
                                {
                                    outputAtoms = new AtomType[] { API.metals[(int)totalmetallicity / 2 + 6], API.metals[(int)totalmetallicity / 2 + 5] };
                                }
                                else
                                {
                                    outputAtoms[0] = API.metals[(int)totalmetallicity / 2 + 6];
                                }
                                Brimstone.API.RemoveAtom(i1);
                                Brimstone.API.RemoveAtom(i2);//deletes atoms
                                Brimstone.API.DrawFallingAtom(seb, i1);
                                Brimstone.API.DrawFallingAtom(seb, i2);//make atoms fall into respective holes
                                Brimstone.API.AddSmallCollider(sim, part, RecessionOutput1);//collision of atom entering
                                Brimstone.API.AddSmallCollider(sim, part, RecessionOutput2);
                                pss[part].field_2743 = true; //sets it to active
                                pss[part].field_2744 = outputAtoms; //set atom(s) that will be made, array size is set, each index can be pulled from separately
                            }
                        }
                        else if (pss[part].field_2743)//if going to fire
                        {
                            Brimstone.API.AddAtom(sim, part, RecessionOutput1, pss[part].field_2744[0]);//make atom!
                            Brimstone.API.AddAtom(sim, part, RecessionOutput2, pss[part].field_2744[1]);//make atom!

                        }
                    }
                    else if (type == Transposal)
                    {

                        if (!sim.FindAtomRelative(part, TransposalInput).method_99(out AtomReference i1) || !sim.FindAtomRelative(part, TransposalBowl).method_99(out AtomReference i2))
                        {
                            //not enough atoms on inputs
                            continue;
                        }
                        if (i1.field_2281 || i1.field_2282) //81 checks for molecule, 82 checks for if that atom is being held
                        {
                            //input atom is being grabbed, or has bonds.
                            continue;
                        }
                        if (!API.transpositionTable.TryGetValue(i2.field_2280, out AtomType inversion))
                        {
                            //atom in bowl is not transposable.
                            continue;
                        }
                        if (i1.field_2280 == Atoms.Tenebrivex)
                        {
                            //input atom is Tenebrivex! Yay!
                            Brimstone.API.RemoveAtom(i1);//remove Tenebrivex
                            Brimstone.API.DrawFallingAtom(seb, i1);//Make it fall
                            Brimstone.API.ChangeAtom(i2, inversion);
                        }
                    }
                    else if (type == Corrosion)
                    {
                        if (first)
                        {
                            if (sim.FindAtomRelative(part, CorrosionOutput).method_1085()) //FindAtomRelative returns an atom, if it finds one at the index. 1085 returns whether the atom has an element as bool.
                            {
                                //output blocked
                                continue;
                            }
                            if (!sim.FindAtomRelative(part, CorrosionInput1).method_99(out AtomReference i1) || !sim.FindAtomRelative(part, CorrosionInput2).method_99(out AtomReference i2))
                            {
                                //not enough atoms on inputs
                                continue;
                            }
                            if (i1.field_2281 || i1.field_2282 || i2.field_2281 || i2.field_2282) //81 checks for molecule, 82 checks for if that atom is being held
                            {
                                //atom is being grabbed, or has bonds.
                                continue;
                            }
                            if (!(API.metals.Contains(i1.field_2280) || !API.metals.Contains(i2.field_2280)))
                            {
                                //atoms are not both metals
                                continue;
                            }
                            if ((GetMetallicity(i1.field_2280) >= 0 && GetMetallicity(i2.field_2280) <= 0) || (GetMetallicity(i1.field_2280) <= 0 && GetMetallicity(i2.field_2280) >= 0))
                            {
                                //atoms are correct! yay!
                                Brimstone.API.RemoveAtom(i1);
                                Brimstone.API.RemoveAtom(i2);//deletes atoms
                                Brimstone.API.DrawFallingAtom(seb, i1);
                                Brimstone.API.DrawFallingAtom(seb, i2);//make atoms fall into respective holes
                                Brimstone.API.AddSmallCollider(sim, part, CorrosionOutput);//collision of atom entering
                                pss[part].field_2743 = true; //sets it to active
                                pss[part].field_2744 = new AtomType[1] { API.metals[GetMetallicity(i1.field_2280) + GetMetallicity(i2.field_2280) + 6] }; //set atom(s) that will be made, array size is set, each index can be pulled from separately
                            }
                        }
                        else if (pss[part].field_2743)//if going to fire
                        {
                            Brimstone.API.AddAtom(sim, part, CorrosionOutput, pss[part].field_2744[0]);//make atom!
                        }
                    }
                    else if (type == Concatenation)
                    {
                        ConcatenationBehave(sim, first, pss[part], part, seb, ConcatenationConduit1, ConcatenationConduit2, ConcatenationYttriumInput, ConcatenationOutput);
                    }
                    else if (type == FlippyConcatenation)
                    {
                        ConcatenationBehave(sim, first, pss[part], part, seb, FlippyConcatenationConduit1, FlippyConcatenationConduit2, FlippyConcatenationYttriumInput, FlippyConcatenationOutput);
                    }
                    else if (type == class_191.field_1779)
                    {
                        if (first)
                        {
                            if (sim.FindAtomRelative(part, ConglomerationOutput).method_1085()) //FindAtomRelative returns an atom, if it finds one at the index. 1085 returns whether the atom has an element as bool.
                            {
                                //output blocked
                                continue;
                            }
                            if (!sim.FindAtomRelative(part, ConglomerationInput1).method_99(out AtomReference i1) || !sim.FindAtomRelative(part, ConglomerationInput2).method_99(out AtomReference i2))
                            {
                                //not enough atoms on inputs
                                continue;
                            }
                            if (i1.field_2281 || i1.field_2282 || i2.field_2281 || i2.field_2282) //81 checks for molecule, 82 checks for if that atom is being held
                            {
                                //atom is being grabbed, or has bonds.
                                continue;
                            }
                            if (i1.field_2280 == Atoms.Yttrium && i2.field_2280 == Atoms.Yttrium)
                            {
                                Brimstone.API.RemoveAtom(i1);
                                Brimstone.API.DrawFallingAtom(seb, i1);//Make it fall
                                Brimstone.API.RemoveAtom(i2);
                                Brimstone.API.DrawFallingAtom(seb, i2);//Make it fall
                                pss[part].field_2743 = true; //sets it to active
                                pss[part].field_2744 = new AtomType[] { Atoms.SusYttrium };
                                Brimstone.API.PlaySound(sim, class_238.field_1991.field_1845);
                            }
                        }
                    }
                }
            }
            );

        }
    }
}
