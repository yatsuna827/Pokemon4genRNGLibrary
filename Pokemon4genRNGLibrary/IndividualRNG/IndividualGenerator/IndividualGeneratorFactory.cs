using System;
using System.Collections.Generic;
using PokemonPRNG.LCG32.StandardLCG;
using PokemonStandardLibrary.PokeDex.Gen4;
using PokemonStandardLibrary;
using PokemonStandardLibrary.CommonExtension;

namespace Pokemon4genRNGLibrary
{
    interface IIndividualGeneratorFactory
    {
        IIndivGenerator GetIndivGenerator(ref uint seed, Pokemon.Species species);
    }

    class StandardIndivGeneratorFactory : IIndividualGeneratorFactory
    {
        private readonly IIndivGenerator indivGenerator;
        public IIndivGenerator GetIndivGenerator(ref uint seed, Pokemon.Species species)
            => indivGenerator;

        public StandardIndivGeneratorFactory(IIndivGenerator indivGenerator)
            => this.indivGenerator = indivGenerator;
    }

    static class CuteCharmIndivGeneratorFactory
    {
        public static IIndividualGeneratorFactory GetInstance(GameVersion gameVersion, IIndivGenerator defaultGenerator, Func<GenderRatio, IIndivGenerator> indivGeneratorSelector)
        {
            if ((int)gameVersion < 3) return new DPtCuteCharmIndivGeneratorFactory(defaultGenerator, indivGeneratorSelector);

            return new HGSSCuteCharmIndivGeneratorFactory(defaultGenerator, indivGeneratorSelector);
        }

        private class DPtCuteCharmIndivGeneratorFactory : IIndividualGeneratorFactory
        {
            private readonly IIndivGenerator defaultGenerator;
            private readonly Dictionary<GenderRatio, IIndivGenerator> indivGenerators;
            public IIndivGenerator GetIndivGenerator(ref uint seed, Pokemon.Species species)
            {
                if (species.GenderRatio.IsFixed()) return defaultGenerator;
                if (seed.GetRand() / DPtDenominators.Rand3 == 0) return defaultGenerator;

                return indivGenerators[species.GenderRatio];
            }

            public DPtCuteCharmIndivGeneratorFactory(IIndivGenerator defaultGenerator, Func<GenderRatio, IIndivGenerator> indivGeneratorSelector)
            {
                this.defaultGenerator = defaultGenerator;
                indivGenerators = new Dictionary<GenderRatio, IIndivGenerator>()
                {
                    { GenderRatio.M1F1, indivGeneratorSelector(GenderRatio.M1F1) },
                    { GenderRatio.M1F3, indivGeneratorSelector(GenderRatio.M1F3) },
                    { GenderRatio.M1F7, indivGeneratorSelector(GenderRatio.M1F7) },
                    { GenderRatio.M3F1, indivGeneratorSelector(GenderRatio.M3F1) },
                };
            }
        }

        private class HGSSCuteCharmIndivGeneratorFactory : IIndividualGeneratorFactory
        {
            private readonly IIndivGenerator defaultGenerator;
            private readonly Dictionary<GenderRatio, IIndivGenerator> indivGenerators;
            public IIndivGenerator GetIndivGenerator(ref uint seed, Pokemon.Species species)
            {
                if (species.GenderRatio.IsFixed()) return defaultGenerator;
                if (seed.GetRand(3) == 0) return defaultGenerator;

                return indivGenerators[species.GenderRatio];
            }

            public HGSSCuteCharmIndivGeneratorFactory(IIndivGenerator defaultGenerator, Func<GenderRatio, IIndivGenerator> indivGeneratorSelector)
            {
                this.defaultGenerator = defaultGenerator;
                indivGenerators = new Dictionary<GenderRatio, IIndivGenerator>()
                {
                    { GenderRatio.M1F1, indivGeneratorSelector(GenderRatio.M1F1) },
                    { GenderRatio.M1F3, indivGeneratorSelector(GenderRatio.M1F3) },
                    { GenderRatio.M1F7, indivGeneratorSelector(GenderRatio.M1F7) },
                    { GenderRatio.M3F1, indivGeneratorSelector(GenderRatio.M3F1) },
                };
            }
        }

    }

}
