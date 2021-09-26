using PokemonStandardLibrary;
using PokemonPRNG.LCG32.StandardLCG;
using PokemonStandardLibrary.PokeDex.Gen4;

namespace Pokemon4genRNGLibrary
{
    static class SynchronizeNatureGenerator
    {
        public static INatureGenerator GetInstance(GameVersion gameVersion, Nature syncNature, INatureGenerator defaultGenerator)
        {
            if ((int)gameVersion < 3) return new DPtSyncNatureGenerator(syncNature, defaultGenerator);
            
            return new HGSSSyncNatureGenerator(syncNature, defaultGenerator);
        }

        private class HGSSSyncNatureGenerator : INatureGenerator
        {
            private readonly Nature syncNature;
            private readonly INatureGenerator defaultGenerator;
            public Nature GenerateFixedNature(ref uint seed)
            {
                if ((seed.GetRand() & 1) == 0) return syncNature;

                return defaultGenerator.GenerateFixedNature(ref seed);
            }

            public HGSSSyncNatureGenerator(Nature syncNature, INatureGenerator defaultGenerator)
                => (this.syncNature, this.defaultGenerator) = (syncNature, defaultGenerator);
        }

        private class DPtSyncNatureGenerator : INatureGenerator
        {
            private readonly Nature syncNature;
            private readonly INatureGenerator defaultGenerator;
            public Nature GenerateFixedNature(ref uint seed)
            {
                if ((seed.GetRand() >> 15) == 0) return syncNature;

                return defaultGenerator.GenerateFixedNature(ref seed);
            }

            public DPtSyncNatureGenerator(Nature syncNature, INatureGenerator defaultGenerator)
                => (this.syncNature, this.defaultGenerator) = (syncNature, defaultGenerator);
        }
    }
}
