using Quintessential;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AlchemicalInversions
{
    internal static class Sounds
    {
        public static Sound Recession;
        public static Sound Conglomeration;
        public static Sound Transposal;
        public static Sound Corrosion;
        public static Sound Concatenation;
        public static void LoadSounds()
        {
            string contentDir = Brimstone.API.GetContentPath("Alchemical_Inversions").method_1087();
            Recession = Brimstone.API.GetSound(contentDir, "sounds/recession").method_1087();
            Conglomeration = Brimstone.API.GetSound(contentDir, "sounds/conglomeration").method_1087();
            Transposal = Brimstone.API.GetSound(contentDir, "sounds/transposal").method_1087();
            Corrosion = Brimstone.API.GetSound(contentDir, "sounds/corrosion").method_1087();
            Concatenation = Brimstone.API.GetSound(contentDir, "sounds/concatenation").method_1087();

            FieldInfo field = typeof(class_11).GetField("field_52", BindingFlags.Static | BindingFlags.NonPublic);
            Dictionary<string, float> volumeDictionary = (Dictionary<string, float>)field.GetValue(null);

            volumeDictionary.Add("recession", 0.3f);
            volumeDictionary.Add("conglomeration", 0.3f);
            volumeDictionary.Add("transposal", 0.2f);
            volumeDictionary.Add("corrosion", 0.3f);
            volumeDictionary.Add("concatenation", 0.3f);

            On.class_201.method_540 += Method_540;
        }

        public static void Unload()
        {
            On.class_201.method_540 -= Method_540;
        }

        public static void Method_540(On.class_201.orig_method_540 orig, class_201 self)
        {
            orig(self);
            Recession.field_4062 = false;
            Conglomeration.field_4062 = false;
            Transposal.field_4062 = false;
            Corrosion.field_4062 = false;
            Concatenation.field_4062 = false;
        }
    }
}
