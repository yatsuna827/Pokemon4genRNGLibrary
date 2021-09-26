using PokemonPRNG.LCG32.StandardLCG;
using PokemonStandardLibrary.PokeDex.Gen4;

namespace Pokemon4genRNGLibrary
{
    class PokeRaderEncounterer : IEncounterer
    {
        private readonly IEncounterer instance;
        public PokeRaderEncounterer(IEncounterer instance)
            => this.instance = instance;

        public bool CheckEncounter(ref uint seed)
        {
            instance.CheckEncounter(ref seed);
            return true;
        }
    }
    class DPtGrassEncounterer : IEncounterer
    {
        private readonly uint thresh;
        public virtual bool CheckEncounter(ref uint seed)
            => seed.GetRand() / DPtDenominators.Rand100 < 40 && seed.GetRand() / DPtDenominators.Rand100 < thresh;

        private DPtGrassEncounterer(uint thresh)
            => this.thresh = thresh;

        class DPtAlleviatedGrassEncounterer : DPtGrassEncounterer
        {
            public DPtAlleviatedGrassEncounterer(uint thresh) : base(thresh) { }

            public override bool CheckEncounter(ref uint seed)
                => seed.GetRand() / DPtDenominators.Rand100 < 5 && base.CheckEncounter(ref seed);

        }

        public static DPtGrassEncounterer CreateInstance(uint thresh, bool inAlleviated)
        {
            if (inAlleviated) return new DPtAlleviatedGrassEncounterer(thresh);
            else return new DPtGrassEncounterer(thresh);
        }

    }
}
