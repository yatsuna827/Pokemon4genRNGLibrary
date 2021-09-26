using System;
using System.Linq;
using PokemonStandardLibrary.PokeDex.Gen4;

namespace Pokemon4genRNGLibrary
{
    interface IIndivGenerator
    {
        Pokemon.Individual GenerateIndividual(ref uint seed, uint lv, Pokemon.Species species);
    }
    
    class StandardIndivGenerator : IIndivGenerator
    {
        private readonly IPIDGenerator pidGenerator;
        private readonly IIVsGenerator ivsGenerator;
        private readonly IHoldItemSelector holdItemSelector;

        public Pokemon.Individual GenerateIndividual(ref uint seed, uint lv, Pokemon.Species species)
        {
            var pid = pidGenerator.GeneratePID(ref seed);
            var ivs = ivsGenerator.GenerateIVs(ref seed);

            var indiv = species.GetIndividual(lv, ivs, pid);
            indiv.HoldItem = holdItemSelector.SelectItem(ref seed, species);

            return indiv;
        }

        public StandardIndivGenerator(IPIDGenerator pidGenerator, IHoldItemSelector holdItemSelector)
        {
            this.pidGenerator = pidGenerator;
            this.ivsGenerator = new StandardIVsGenerator();
            this.holdItemSelector = holdItemSelector;
        }
    }

    class UnownIndivGenerator : IIndivGenerator
    {
        private readonly IPIDGenerator pidGenerator;
        private readonly IIVsGenerator ivsGenerator;
        private readonly IHoldItemSelector holdItemSelector;
        private readonly IUnownFormGenerator unownFormGenerator;

        public Pokemon.Individual GenerateIndividual(ref uint seed, uint lv, Pokemon.Species species)
        {
            var pid = pidGenerator.GeneratePID(ref seed);
            var ivs = ivsGenerator.GenerateIVs(ref seed);

            var item = holdItemSelector.SelectItem(ref seed, species);
            var form = unownFormGenerator.GenerateUnownForm(ref seed);

            var indiv = Pokemon.GetPokemon(species.Name, form).GetIndividual(lv, ivs, pid);
            indiv.HoldItem = item;

            return indiv;
        }

        public UnownIndivGenerator(IPIDGenerator pidGenerator, IHoldItemSelector holdItemSelector, IUnownFormGenerator unownFormGenerator)
        {
            this.pidGenerator = pidGenerator;
            this.ivsGenerator = new StandardIVsGenerator();
            this.holdItemSelector = holdItemSelector;
            this.unownFormGenerator = unownFormGenerator;
        }
    }

    class RerollIndivGenerator : IIndivGenerator
    {
        private readonly IPIDGenerator pidGenerator;
        private readonly IIVsGenerator ivsGenerator;
        private readonly IHoldItemSelector holdItemSelector;

        public Pokemon.Individual GenerateIndividual(ref uint seed, uint lv, Pokemon.Species species)
        {
            int cnt = 0;
            while (true)
            {
                var pid = pidGenerator.GeneratePID(ref seed);
                var ivs = ivsGenerator.GenerateIVs(ref seed);

                if (++cnt == 4 || ivs.Contains(31u))
                {
                    var indiv = species.GetIndividual(lv, ivs, pid);
                    indiv.HoldItem = holdItemSelector.SelectItem(ref seed, species);

                    return indiv;
                }
            }
        }

        public RerollIndivGenerator(IPIDGenerator pidGenerator, IHoldItemSelector holdItemSelector)
        {
            this.pidGenerator = pidGenerator;
            this.ivsGenerator = new StandardIVsGenerator();
            this.holdItemSelector = holdItemSelector;
        }
    }
}
