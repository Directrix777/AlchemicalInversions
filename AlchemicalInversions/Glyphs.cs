using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Brimstone;
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
        public static void Initialize()
        {
            Conglomeration = API.CreateSimpleGlyph(
                ID: "alchemical-inversions-conglomeration",
                name: "Glyph of Conglomeration",
                description: "The glyph of Conglomeration consumes one Vitae and one Mors atom to create an atom of Tenebrivex.",
                cost: 20,
                glow: class_238.field_1989.field_97.field_386,
                stroke: class_238.field_1989.field_97.field_387,
                icon: class_238.field_1989.field_90.field_245.field_319, //Placeholder, delete once this glyph has a proper texture
                //icon: API.GetTexture("textures/parts/Directrix777/AlchemicalInversions/conglomeration"),
                hoveredIcon: API.GetTexture("textures/parts/Directrix777/AlchemicalInversions/conglomeration_hover"),
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
            QApi.RunAfterCycle(static (sim, first) =>
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
                            if(i1.field_2281 || i1.field_2282 || i2.field_2281 || i2.field_2282) //81 checks for molecule, 82 checks for if that atom is being held
                            {
                                //atom is being grabbed, or has bonds.
                                continue;
                            }
                            if((i1.field_2280 == API.VanillaAtoms.vitae && i2.field_2280 == API.VanillaAtoms.mors) || (i2.field_2280 == API.VanillaAtoms.vitae && i1.field_2280 == API.VanillaAtoms.mors))
                            {
                                //atoms are correct! yay!
                                API.RemoveAtom(i1);
                                API.RemoveAtom(i2);//deletes atoms
                                API.DrawFallingAtom(seb, i1);
                                API.DrawFallingAtom(seb, i2);//make atoms fall into respective holes
                                API.AddSmallCollider(sim, part, ConglomerationOutput);//collision of atom entering
                                pss[part].field_2743 = true; //sets it to active
                                pss[part].field_2744 = new AtomType[1] { Atoms.Tenebrivex }; //set atom(s) that will be made, array size is set, each index can be pulled from separately
                            }
                        }
                        else if (pss[part].field_2743)//if going to fire
                        {
                            API.AddAtom(sim, part, ConglomerationOutput, pss[part].field_2744[0]);//make atom!
                        }
                    }
                }
            }
            );
        }
    }
}
