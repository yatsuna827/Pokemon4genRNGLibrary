using PokemonPRNG.LCG32;
using PokemonPRNG.LCG32.StandardLCG;
using PokemonStandardLibrary.PokeDex.Gen4;
using Pokemon4genMapData;

namespace Pokemon4genRNGLibrary
{
    public partial class WildGenerator : IGeneratable<RNGResult<Pokemon.Individual>>
    {
        private readonly IEncounterer encounterer;
        private readonly ISlotSelector slotSelector;
        private readonly ILvGenerator lvGenerator;
        private readonly IIndividualGeneratorFactory individualGeneratorFactory;
        private readonly uint afterBattleAdvances;

        public RNGResult<Pokemon.Individual> Generate(uint seed)
        {
            var start = seed;
            // 出現判定
            if (!encounterer.CheckEncounter(ref seed)) return new RNGResult<Pokemon.Individual>()
            {
                HeadSeed = start,
                TailSeed = seed,
                Content = null
            };

            var slot = slotSelector.SelectSlot(ref seed);

            var lv = lvGenerator.GenerateLv(ref seed, slot);

            var indiv = individualGeneratorFactory
                        .GetIndivGenerator(ref seed, slot.Pokemon)
                        .GenerateIndividual(ref seed, lv, slot.Pokemon);

            seed.Advance(afterBattleAdvances); // 用途不明.

            return new RNGResult<Pokemon.Individual>()
            {
                HeadSeed = start,
                TailSeed = seed,
                Content = indiv
            };
        }

        public ExtWildGenerator<TOption> Hook<TOption>(ISideEffectiveGeneratable<TOption> hook)
            => new ExtWildGenerator<TOption>(encounterer, slotSelector, lvGenerator, individualGeneratorFactory, hook);

        internal WildGenerator(IEncounterer encounterer, 
            ISlotSelector slotSelector,
            ILvGenerator lvGenerator, 
            IIndividualGeneratorFactory individualGeneratorFactory,
            uint afterBattleAdvances)
        {
            this.encounterer = encounterer;
            this.slotSelector = slotSelector;
            this.lvGenerator = lvGenerator;
            this.individualGeneratorFactory = individualGeneratorFactory;
            this.afterBattleAdvances = afterBattleAdvances;
        }
    }

    /// <summary>
    /// 個体生成に続く生成処理をカスタマイズできるクラス.
    /// WildGeneratorと違い、戦闘終了時の1 or 5消費は行わないので、考慮したい場合はhookに持たせる必要がある.
    /// </summary>
    /// <typeparam name="TOption"></typeparam>
    public class ExtWildGenerator<TOption> : IGeneratable<RNGResult<Pokemon.Individual, TOption>>
    {
        private readonly IEncounterer encounterer;
        private readonly ISlotSelector slotSelector;
        private readonly ILvGenerator lvGenerator;
        private readonly IIndividualGeneratorFactory individualGeneratorFactory;
        private readonly ISideEffectiveGeneratable<TOption> hook;

        public RNGResult<Pokemon.Individual, TOption> Generate(uint seed)
        {
            var start = seed;
            // 出現判定
            if (!encounterer.CheckEncounter(ref seed)) return new RNGResult<Pokemon.Individual, TOption>()
            {
                HeadSeed = start,
                TailSeed = seed,
            };

            var slot = slotSelector.SelectSlot(ref seed);

            var lv = lvGenerator.GenerateLv(ref seed, slot);

            var indiv = individualGeneratorFactory
                        .GetIndivGenerator(ref seed, slot.Pokemon)
                        .GenerateIndividual(ref seed, lv, slot.Pokemon);

            var option = hook.Generate(ref seed);

            return new RNGResult<Pokemon.Individual, TOption>()
            {
                HeadSeed = start,
                TailSeed = seed,
                Content = indiv,
                Option = option
            };
        }

        internal ExtWildGenerator(IEncounterer encounterer, ISlotSelector slotSelector, ILvGenerator lvGenerator, IIndividualGeneratorFactory individualGeneratorFactory, ISideEffectiveGeneratable<TOption> hook)
        {
            this.encounterer = encounterer;
            this.slotSelector = slotSelector;
            this.lvGenerator = lvGenerator;
            this.individualGeneratorFactory = individualGeneratorFactory;
            this.hook = hook;
        }
    }

}
