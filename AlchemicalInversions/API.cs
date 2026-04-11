using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlchemicalInversions
{
    public static class API
    {
        public static Dictionary<AtomType, AtomType> transpositionTable = new();
        internal static AtomType[] metals = 
        { 
            Atoms.AntiGold, 
            Atoms.AntiSilver, 
            Atoms.AntiCopper, 
            Atoms.AntiIron, 
            Atoms.AntiTin, 
            Atoms.AntiLead, 
            Atoms.Yttrium, 
            Brimstone.API.VanillaAtoms.lead, 
            Brimstone.API.VanillaAtoms.tin, 
            Brimstone.API.VanillaAtoms.iron, 
            Brimstone.API.VanillaAtoms.copper, 
            Brimstone.API.VanillaAtoms.silver, 
            Brimstone.API.VanillaAtoms.gold 
        };

        internal static AtomType[] cardinals = 
        {
            Brimstone.API.VanillaAtoms.mors, 
            Brimstone.API.VanillaAtoms.earth, 
            Brimstone.API.VanillaAtoms.water, 
            Brimstone.API.VanillaAtoms.salt, 
            Atoms.Tenebrivex, 
            Brimstone.API.VanillaAtoms.fire, 
            Brimstone.API.VanillaAtoms.air, 
            Brimstone.API.VanillaAtoms.vitae
        };
        public static void Initialize()
        {
            for (int i = 0; i < 6; i++)
            {
                transpositionTable.Add(metals[i], metals[metals.Length - 1 - i]);
                transpositionTable.Add(metals[metals.Length - 1 - i], metals[i]);
            }
            for (int i = 0; i < 4; i++)
            {
                transpositionTable.Add(cardinals[i], cardinals[cardinals.Length - 1 - i]);
                transpositionTable.Add(cardinals[cardinals.Length - 1 - i], cardinals[i]);
            }
        }
    }
}
