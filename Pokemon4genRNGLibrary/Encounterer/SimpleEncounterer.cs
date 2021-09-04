using PokemonPRNG.LCG32.StandardLCG;
using PokemonStandardLibrary.PokeDex.Gen4;

namespace Pokemon4genRNGLibrary
{
    class SimpleEncounterer
    {
        public static IEncounterer CreateInstance(GameVersion gameVersion, uint thresh)
        {
            if ((int)gameVersion < 3)
                return new DPtSimpleEcnounterer(thresh);
            else
                return new HGSSSimpleEcnounterer(thresh);
        }

        private class DPtSimpleEcnounterer : IEncounterer
        {
            private readonly uint thresh;
            public bool CheckEncounter(ref uint seed)
                => seed.GetRand() / DPtDenominators.Rand100 < thresh;

            public DPtSimpleEcnounterer(uint thresh)
                => this.thresh = thresh;
        }

        private class HGSSSimpleEcnounterer : IEncounterer
        {
            private readonly uint thresh;
            public bool CheckEncounter(ref uint seed)
                => seed.GetRand(100) < thresh;

            public HGSSSimpleEcnounterer(uint thresh)
                => this.thresh = thresh;
        }
    }
}
