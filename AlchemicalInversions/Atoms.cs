using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Brimstone;
using Quintessential;

namespace AlchemicalInversions
{
    public static class Atoms
    {
        public static AtomType Tenebrivex;
        public static AtomType AntiLead;
        public static AtomType AntiTin;
        public static AtomType AntiIron;
        public static AtomType AntiCopper;
        public static AtomType AntiSilver;
        public static AtomType AntiGold;
        public static AtomType Yttrium;

        public static void Initialize()
        {
            Tenebrivex = Brimstone.API.CreateNormalAtom(
                ID: 70,
                modName: "AlchemicalInversions",
                name: "Tenebrivex",
                pathToSymbol: "textures/atoms/Directrix777/AlchemicalInversions/Tenebrivex_Symbol",
                pathToDiffuse: "textures/atoms/Directrix777/AlchemicalInversions/Tenebrivex_Diffuse",
                pathToShade: "textures/atoms/Directrix777/AlchemicalInversions/Tenebrivex_Shade"
                );

            AntiLead = Brimstone.API.CreateMetalAtom(
                ID: 71,
                modName: "AlchemicalInversions",
                name: "Anti-Lead",
                pathToSymbol: "textures/atoms/Directrix777/AlchemicalInversions/Anti-Lead_Symbol",
                pathToLightramp: "textures/atoms/Directrix777/AlchemicalInversions/Anti-Lead_Lightramp"
                );

            AntiTin = Brimstone.API.CreateMetalAtom(
                ID: 72,
                modName: "AlchemicalInversions",
                name: "Anti-Tin",
                pathToSymbol: "textures/atoms/Directrix777/AlchemicalInversions/Anti-Tin_Symbol",
                pathToLightramp: "textures/atoms/Directrix777/AlchemicalInversions/Anti-Tin_Lightramp",
                promotesTo: AntiLead
                );

            AntiIron = Brimstone.API.CreateMetalAtom(
                ID: 73,
                modName: "AlchemicalInversions",
                name: "Anti-Iron",
                pathToSymbol: "textures/atoms/Directrix777/AlchemicalInversions/Anti-Iron_Symbol",
                pathToLightramp: "textures/atoms/Directrix777/AlchemicalInversions/Anti-Iron_Lightramp",
                promotesTo: AntiTin
                );

            AntiCopper = Brimstone.API.CreateMetalAtom(
                ID: 74,
                modName: "AlchemicalInversions",
                name: "Anti-Copper",
                pathToSymbol: "textures/atoms/Directrix777/AlchemicalInversions/Anti-Copper_Symbol",
                pathToLightramp: "textures/atoms/Directrix777/AlchemicalInversions/Anti-Copper_Lightramp",
                promotesTo: AntiIron
                );

            AntiSilver = Brimstone.API.CreateMetalAtom(
                ID: 75,
                modName: "AlchemicalInversions",
                name: "Anti-Silver",
                pathToSymbol: "textures/atoms/Directrix777/AlchemicalInversions/Anti-Silver_Symbol",
                pathToLightramp: "textures/atoms/Directrix777/AlchemicalInversions/Anti-Silver_Lightramp",
                promotesTo: AntiCopper
                );

            AntiGold = Brimstone.API.CreateMetalAtom(
                ID: 76,
                modName: "AlchemicalInversions",
                name: "Anti-Gold",
                pathToSymbol: "textures/atoms/Directrix777/AlchemicalInversions/Anti-Gold_Symbol",
                pathToLightramp: "textures/atoms/Directrix777/AlchemicalInversions/Anti-Gold_Lightramp",
                promotesTo: AntiSilver
                );

            Yttrium = Brimstone.API.CreateMetalAtom(
                ID: 77,
                modName: "AlchemicalInversions",
                name: "Yttrium",
                pathToSymbol: "textures/atoms/Directrix777/AlchemicalInversions/Anti-Tin_Symbol",
                pathToLightramp: "textures/atoms/Directrix777/AlchemicalInversions/Anti-Tin_Lightramp"
                );

            QApi.AddAtomType(Tenebrivex);
            QApi.AddAtomType(AntiLead);
            QApi.AddAtomType(AntiTin);
            QApi.AddAtomType(AntiIron);
            QApi.AddAtomType(AntiCopper);
            QApi.AddAtomType(AntiSilver);
            QApi.AddAtomType(AntiGold);
            QApi.AddAtomType(Yttrium);
        }
    }
}
