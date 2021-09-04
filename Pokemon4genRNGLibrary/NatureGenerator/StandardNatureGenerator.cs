using PokemonStandardLibrary;
using PokemonPRNG.LCG32.StandardLCG;
using PokemonStandardLibrary.PokeDex.Gen4;

namespace Pokemon4genRNGLibrary
{
    static class StandardNatureGenerator
    {
        public static INatureGenerator GetInstance(GameVersion gameVersion)
            => (int)gameVersion < 3 ? dpt : hgss;

        private static readonly INatureGenerator dpt = new DPtStandardNatureGenerator();
        private static readonly INatureGenerator hgss = new HGSSStandardNatureGenerator();

        private class DPtStandardNatureGenerator : INatureGenerator
        {
            public Nature GenerateFixedNature(ref uint seed) => (Nature)(seed.GetRand() / DPtDenominators.Rand25);
        }

        private class HGSSStandardNatureGenerator : INatureGenerator
        {
            public Nature GenerateFixedNature(ref uint seed) => (Nature)seed.GetRand(25);
        }
    }

}
