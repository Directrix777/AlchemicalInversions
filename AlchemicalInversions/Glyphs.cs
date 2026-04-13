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

        public static Texture ConglomerationAnimismusSymbol = Brimstone.API.GetTexture("textures/parts/Directrix777/AlchemicalInversions/Conglomeration/conglomeration_animismus_symbol");
        public static Texture MetalSymbol = Brimstone.API.GetTexture("textures/parts/Directrix777/AlchemicalInversions/Recession/recession_metal_symbol");
        public static Texture TransposalTenebrivexSymbol = Brimstone.API.GetTexture("textures/parts/Directrix777/AlchemicalInversions/Transposal/transposal_tenebrivex_symbol");
        private static int GetMetallicity(AtomType atomtype)
        {
            if(!API.metals.Contains(atomtype))
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
                foreach(var h in new HexIndex[] {ConglomerationInput1, ConglomerationInput2})
                {
                    renderer.method_528(class_238.field_1989.field_90.field_255.field_293, h, Vector2.Zero);//renders holes
                    renderer.method_529(ConglomerationAnimismusSymbol, h, Vector2.Zero);//renders holes
                }
                renderer.method_528(class_238.field_1989.field_90.field_228.field_272, ConglomerationOutput, Vector2.Zero);//renders void under iris
                int irisFrame = 15;
                bool afterIrisOpens = false;
                Molecule risingAtom = null;
                Vector2 risingOffset = uco.field_1984 + class_187.field_1742.method_492(ConglomerationOutput).Rotated(uco.field_1985);
                if(pss.field_2743)
                {
                    irisFrame = class_162.method_404((int)(class_162.method_411(1, -1, time) * 16), 0, 15);
                    afterIrisOpens = time > 0.5;
                    risingAtom = Molecule.method_1121(pss.field_2744[0]);
                    if(!afterIrisOpens)
                    {
                        Editor.method_925(risingAtom, risingOffset, new HexIndex(0,0), 0, 1, time, 1, false, null);
                    }
                }
                renderer.method_529(class_238.field_1989.field_90.field_246[irisFrame], ConglomerationOutput, Vector2.Zero);//renders current iris frame
                renderer.method_528(class_238.field_1989.field_90.field_228.field_271, ConglomerationOutput, Vector2.Zero);//renders rim above iris
                if(pss.field_2743 && afterIrisOpens)
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
                    renderer.method_529(MetalSymbol, h, Vector2.Zero);
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
                renderer.method_529(TransposalTenebrivexSymbol, TransposalInput, Vector2.Zero);//renders crooked??? Why?
                renderer.method_528(class_238.field_1989.field_90.field_170, TransposalBowl, Vector2.Zero); //renders bowl
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
                    else if(type == Transposal)
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
                    else if(type == class_191.field_1779)
                    {
                        if(first)
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
