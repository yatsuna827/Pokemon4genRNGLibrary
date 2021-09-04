using System;
using System.Collections.Generic;
using System.Linq;
using PokemonPRNG.LCG32.StandardLCG;

namespace Pokemon4genRNGLibrary
{
    public class RuinsOfAlphPuzzleProgress
    {
        public bool Omanyte { get; set; }
        public bool Kabuto { get; set; }
        public bool Aerodactyl { get; set; }
        public bool Hoh_oh { get; set; }

        internal bool AllSolved => Omanyte && Kabuto && Aerodactyl && Hoh_oh;
        internal bool NeverSolved => !Omanyte && !Kabuto && !Aerodactyl && !Hoh_oh;
    }

    class HGSSCommonUnownFormGenerator : IUnownFormGenerator
    {
        private readonly uint m;
        private readonly string[] appearableForms;
        public string GenerateUnownForm(ref uint seed)
            => appearableForms[seed.GetRand(m)];

        public HGSSCommonUnownFormGenerator(RuinsOfAlphPuzzleProgress progress)
        {
            if (progress.NeverSolved) throw new Exception("アンノーンは出現しません");

            var forms = new List<string>();
            if (progress.Kabuto) forms.AddRange(Enumerable.Range('A', 10).Select(_ => ((char)_).ToString()));
            if (progress.Aerodactyl) forms.AddRange(Enumerable.Range('R', 5).Select(_ => ((char)_).ToString()));
            if (progress.Hoh_oh) forms.AddRange(Enumerable.Range('K', 7).Select(_ => ((char)_).ToString()));
            if (progress.Omanyte) forms.AddRange(Enumerable.Range('W', 4).Select(_ => ((char)_).ToString()));

            appearableForms = forms.ToArray();
            m = (uint)appearableForms.Length;
        }
    }

}
