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

        public static void Initialize()
        {
            Tenebrivex = API.CreateNormalAtom(
                ID: 70,
                modName: "AlchemicalInversions",
                name: "Tenebrivex",
                pathToSymbol: "textures/atoms/Directrix777/AlchemicalInversions/Tenebrivex_Symbol",
                pathToDiffuse: "textures/atoms/Directrix777/AlchemicalInversions/Tenebrivex_Diffuse",
                pathToShade: "textures/atoms/Directrix777/AlchemicalInversions/Tenebrivex_Shade"
                );

            QApi.AddAtomType(Tenebrivex);
        }
    }
}
