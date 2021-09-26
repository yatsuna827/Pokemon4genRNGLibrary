using System;
using System.Collections.Generic;
using System.Linq;
using PokemonPRNG.LCG32.StandardLCG;

namespace Pokemon4genRNGLibrary
{
    class HGSSRadioUnownFormGenerator : IUnownFormGenerator
    {
        private readonly uint m;
        private readonly string[] appearableForms;
        private readonly IUnownFormGenerator defaultGenerator;

        public string GenerateUnownForm(ref uint seed)
        {
            if (seed.GetRand(100) >= 50) return defaultGenerator.GenerateUnownForm(ref seed);

            return appearableForms[seed.GetRand(m)];
        }

        private HGSSRadioUnownFormGenerator(IUnownFormGenerator defaultGenerator, string[] forms)
        {
            m = (uint)forms.Length;
            appearableForms = forms;
            this.defaultGenerator = defaultGenerator;
        }

        public static IUnownFormGenerator CreateInstance(RuinsOfAlphPuzzleProgress progress, GotUnowns gotUnowns)
        {
            if (progress.NeverSolved) throw new Exception("アンノーンは出現しません");

            var notGotten = Enumerable.Range(0, 26).Where(i => ((int)gotUnowns & (1 << i)) == 0)
                .Select(i => ((char)('A' + i)).ToString()).ToArray();

            if (notGotten.Length == 0) return new HGSSCommonUnownFormGenerator(progress);

            return new HGSSRadioUnownFormGenerator(new HGSSCommonUnownFormGenerator(progress), notGotten);
        }
    }


    [Flags]
    public enum GotUnowns
    {
        A = 1 << 0,
        B = 1 << 1,
        C = 1 << 2,
        D = 1 << 3,
        E = 1 << 4,
        F = 1 << 5,
        G = 1 << 6,
        H = 1 << 7,
        I = 1 << 8,
        J = 1 << 9,
        K = 1 << 10,
        L = 1 << 11,
        M = 1 << 12,
        N = 1 << 13,
        O = 1 << 14,
        P = 1 << 15,
        Q = 1 << 16,
        R = 1 << 17,
        S = 1 << 18,
        T = 1 << 19,
        U = 1 << 20,
        V = 1 << 21,
        W = 1 << 22,
        X = 1 << 23,
        Y = 1 << 24,
        Z = 1 << 25,
    }
}
