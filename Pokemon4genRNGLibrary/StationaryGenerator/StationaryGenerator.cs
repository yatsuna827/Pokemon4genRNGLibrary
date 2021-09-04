using System;
using System.Collections.Generic;
using System.Text;
using PokemonPRNG.LCG32;
using PokemonPRNG.LCG32.StandardLCG;
using PokemonStandardLibrary.PokeDex.Gen4;
using Pokemon4genMapData;

namespace Pokemon4genRNGLibrary
{
    public abstract class PIDRerollFlag
    {
        public class WithReroll : PIDRerollFlag { private WithReroll() { } }
        public class WithoutReroll : PIDRerollFlag { private WithoutReroll() { } }
        private protected PIDRerollFlag() { } 
    }

    public class StationaryGenerator : IGeneratable<RNGResult<Pokemon.Individual>>
    {
        private readonly uint lv;
        private readonly Pokemon.Species species;
        private readonly IIndividualGeneratorFactory individualGeneratorFactory;

        public RNGResult<Pokemon.Individual> Generate(uint seed)
        {
            var start = seed;

            var indiv = individualGeneratorFactory
                        .GetIndivGenerator(ref seed, species)
                        .GenerateIndividual(ref seed, lv, species);

            seed.Advance(); // 用途不明.

            return new RNGResult<Pokemon.Individual>()
            {
                HeadSeed = start,
                TailSeed = seed,
                Content = indiv
            };
        }

        private StationaryGenerator(Pokemon.Species species, uint lv, IIndividualGeneratorFactory individualGeneratorFactory)
            => (this.species, this.lv, this.individualGeneratorFactory) = (species, lv, individualGeneratorFactory);


        private static IGeneratable<RNGResult<Pokemon.Individual>> CreateWithoutPIDRerollInstance(GameVersion gameVersion, Pokemon.Species species, uint lv)
        {
            var gen = new StandardIndivGenerator(StandardPIDGenerator.Instance, new StandardItemSelector(gameVersion));

            return new SimpleGenerator(species, lv, gen);
        }

        public static IGeneratable<RNGResult<Pokemon.Individual>> CreateInstance<TFlag>(GameVersion gameVersion, Pokemon.Species species, uint lv)
            where TFlag : PIDRerollFlag
        {
            if (typeof(TFlag) == typeof(PIDRerollFlag.WithoutReroll))
                return CreateWithoutPIDRerollInstance(gameVersion, species, lv);

            var pid = new NaturePredeterminedPIDGenerator(StandardPIDGenerator.Instance, StandardNatureGenerator.GetInstance(gameVersion));
            var gen = new StandardIndivGenerator(pid, new StandardItemSelector(gameVersion));
            var fac = new StandardIndivGeneratorFactory(gen);

            return new StationaryGenerator(species, lv, fac);
        }
        public static IGeneratable<RNGResult<Pokemon.Individual>> CreateInstance<TFlag>(GameVersion gameVersion, Pokemon.Species species, uint lv, Synchronize synchronize)
            where TFlag : PIDRerollFlag.WithReroll
        {
            var pid = new NaturePredeterminedPIDGenerator(
                StandardPIDGenerator.Instance, 
                SynchronizeNatureGenerator.GetInstance(gameVersion, synchronize.SyncNature, StandardNatureGenerator.GetInstance(gameVersion)));
            var gen = new StandardIndivGenerator(pid, new StandardItemSelector(gameVersion));
            var fac = new StandardIndivGeneratorFactory(gen);

            return new StationaryGenerator(species, lv, fac);
        }
        public static IGeneratable<RNGResult<Pokemon.Individual>> CreateInstance<TFlag>(GameVersion gameVersion, Pokemon.Species species, uint lv, CuteCharm cuteCharm)
            where TFlag : PIDRerollFlag.WithReroll
        {
            var pid = new NaturePredeterminedPIDGenerator(StandardPIDGenerator.Instance, StandardNatureGenerator.GetInstance(gameVersion));
            var gen = new StandardIndivGenerator(pid, new StandardItemSelector(gameVersion));
            var fac = CuteCharmIndivGeneratorFactory.GetInstance(
                gameVersion,
                gen,
                (ratio) => new StandardIndivGenerator(
                    new CuteCharmPIDGenerator(
                        StandardNatureGenerator.GetInstance(gameVersion), 
                        cuteCharm.FixedGender, 
                        ratio), 
                    new StandardItemSelector(gameVersion))
            );

            return new StationaryGenerator(species, lv, fac);
        }


        private class SimpleGenerator : IGeneratable<RNGResult<Pokemon.Individual>>
        {
            private readonly uint lv;
            private readonly Pokemon.Species species;
            private readonly IIndivGenerator indivGenerator;

            public RNGResult<Pokemon.Individual> Generate(uint seed)
            {
                var start = seed;

                var indiv = indivGenerator.GenerateIndividual(ref seed, lv, species);

                seed.Advance(); // 用途不明.

                return new RNGResult<Pokemon.Individual>()
                {
                    HeadSeed = start,
                    TailSeed = seed,
                    Content = indiv
                };
            }

            public SimpleGenerator(Pokemon.Species species, uint lv, IIndivGenerator indivGenerator)
                => (this.species, this.lv, this.indivGenerator) = (species, lv, indivGenerator);
        }
    }

}
