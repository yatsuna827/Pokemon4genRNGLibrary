using System;
using Pokemon4genMapData;
using PokemonStandardLibrary.PokeDex.Gen4;

namespace Pokemon4genRNGLibrary
{
    // ポケトレ連鎖時のやつ.
    // スロット固定と色確定処理があるので.
    class PokeRaderWildGeneratorFactory<TVersion, TEncType> : WildGeneratorFactory<TVersion, TEncType>
        where TVersion : struct, IWrappedGameVersion
        where TEncType : struct, IWrappedEncounterType<TVersion>
    {
        private protected readonly bool inAlleviated;

        protected virtual ISlotSelector GetSlotSelector() => base.GetDefaultSlotSelector();
        protected virtual IPIDGenerator GetPIDGenerator() => base.GetDefaultPIDGenerator();

        public PokeRaderWildGeneratorFactory(IMapData<TVersion, TEncType> mapData, bool inAlleviated) : base(mapData)
            => this.inAlleviated = inAlleviated;

        protected override IIndivGenerator BuildIndivGenerator(IPIDGenerator pidGenerator, IHoldItemSelector holdItemSelector)
            => new StandardIndivGenerator(pidGenerator, holdItemSelector);

        public override WildGenerator Resolve()
        {
            var enc = new PokeRaderEncounterer(DPtGrassEncounterer.CreateInstance(mapData.BasicEncounterRate, inAlleviated));

            var slotSelector = GetSlotSelector();
            var lvGenerator = GetDefaultLvGenerator();

            var pidGenerator = GetPIDGenerator();

            var indivFactory = new StandardIndivGeneratorFactory(BuildIndivGenerator(pidGenerator, GetDefaultItemSelector()));

            return BuildWildGenerator(enc, slotSelector, lvGenerator, indivFactory);
        }

        public override WildGenerator Resolve(Synchronize synchronize)
        {
            var enc = new PokeRaderEncounterer(DPtGrassEncounterer.CreateInstance(mapData.BasicEncounterRate, inAlleviated));

            var slotSelector = GetSlotSelector();
            var lvGenerator = GetDefaultLvGenerator();

            var defaultNatureGenerator = StandardNatureGenerator.GetInstance(default(TVersion).Unwrap());

            var pidGenerator = new NaturePredeterminedPIDGenerator(StandardPIDGenerator.Instance, SynchronizeNatureGenerator.GetInstance(default(TVersion).Unwrap(), synchronize.SyncNature, defaultNatureGenerator));

            var indivFactory = new StandardIndivGeneratorFactory(BuildIndivGenerator(pidGenerator, GetDefaultItemSelector()));

            return BuildWildGenerator(enc, slotSelector, lvGenerator, indivFactory);
        }

        public override WildGenerator Resolve(CuteCharm cuteCharm)
        {
            var enc = new PokeRaderEncounterer(DPtGrassEncounterer.CreateInstance(mapData.BasicEncounterRate, inAlleviated));

            var slotSelector = GetSlotSelector();
            var lvGenerator = GetDefaultLvGenerator();

            var defaultNatureGenerator = StandardNatureGenerator.GetInstance(default(TVersion).Unwrap());

            var defaultGenerator = BuildIndivGenerator(GetDefaultPIDGenerator(), GetDefaultItemSelector());

            var indivFactory = CuteCharmIndivGeneratorFactory.GetInstance(
                default(TVersion).Unwrap(),
                defaultGenerator,
                (ratio) => new StandardIndivGenerator(new CuteCharmPIDGenerator(defaultNatureGenerator, cuteCharm.FixedGender, ratio), GetDefaultItemSelector())
            );

            return BuildWildGenerator(enc, slotSelector, lvGenerator, indivFactory);
        }

        // ポケトレ使用時はプレッシャー無効
        public override WildGenerator Resolve(Pressure _)
            => Resolve();
        public override WildGenerator Resolve(Static _)
            => Resolve();
        public override WildGenerator Resolve(MagnetPull _)
            => Resolve();


        public override WildGenerator Resolve(Illuminate _)
        {
            var enc = new PokeRaderEncounterer(DPtGrassEncounterer.CreateInstance(mapData.BasicEncounterRate * 2, inAlleviated));

            var slotSelector = GetSlotSelector();
            var lvGenerator = GetDefaultLvGenerator();

            var pidGenerator = GetPIDGenerator();

            var indivFactory = new StandardIndivGeneratorFactory(BuildIndivGenerator(pidGenerator, GetDefaultItemSelector()));

            return BuildWildGenerator(enc, slotSelector, lvGenerator, indivFactory);
        }
        public override WildGenerator Resolve(Stench _)
        {
            var enc = new PokeRaderEncounterer(DPtGrassEncounterer.CreateInstance(mapData.BasicEncounterRate / 2, inAlleviated));

            var slotSelector = GetSlotSelector();
            var lvGenerator = GetDefaultLvGenerator();

            var pidGenerator = GetPIDGenerator();

            var indivFactory = new StandardIndivGeneratorFactory(BuildIndivGenerator(pidGenerator, GetDefaultItemSelector()));

            return BuildWildGenerator(enc, slotSelector, lvGenerator, indivFactory);
        }

        public override WildGenerator Resolve(CompoundEyes _)
        {
            var enc = new PokeRaderEncounterer(DPtGrassEncounterer.CreateInstance(mapData.BasicEncounterRate, inAlleviated));

            var slotSelector = GetSlotSelector();
            var lvGenerator = GetDefaultLvGenerator();

            var pidGenerator = GetPIDGenerator();

            var indivFactory = new StandardIndivGeneratorFactory(BuildIndivGenerator(pidGenerator, new CompoundEyesItemSelector(default(TVersion).Unwrap())));

            return BuildWildGenerator(enc, slotSelector, lvGenerator, indivFactory);
        }
    }

    class ChainPokeRaderWildGeneratorFactory<TVersion, TEncType> : PokeRaderWildGeneratorFactory<TVersion, TEncType>
        where TVersion : struct, IWrappedGameVersion
        where TEncType : struct, IWrappedEncounterType<TVersion>
    {
        private readonly ISlotSelector slotSelector;

        protected override ISlotSelector GetSlotSelector() => slotSelector;

        public ChainPokeRaderWildGeneratorFactory(IMapData<TVersion, TEncType> mapData, bool inAlleviated, Func<IMapData<TVersion, TEncType>, Slot> slotSelector) : base(mapData, inAlleviated)
            => this.slotSelector = new FixedSlotSelector(slotSelector(mapData).Convert(Pokemon.GetPokemon));
    }

    class ShinyPokeRaderWildGeneratorFactory<TVersion, TEncType> : ChainPokeRaderWildGeneratorFactory<TVersion, TEncType>
        where TVersion : struct, IWrappedGameVersion
        where TEncType : struct, IWrappedEncounterType<TVersion>
    {
        private readonly IPIDGenerator shinyPIDGenerator;
        protected override IPIDGenerator GetPIDGenerator() => shinyPIDGenerator;

        public override WildGenerator Resolve(Synchronize synchronize)
        {
            var enc = new PokeRaderEncounterer(DPtGrassEncounterer.CreateInstance(mapData.BasicEncounterRate, inAlleviated));

            var slotSelector = GetSlotSelector();
            var lvGenerator = GetDefaultLvGenerator();

            var defaultNatureGenerator = StandardNatureGenerator.GetInstance(default(TVersion).Unwrap());

            var pidGenerator = new NaturePredeterminedPIDGenerator(shinyPIDGenerator, SynchronizeNatureGenerator.GetInstance(default(TVersion).Unwrap(), synchronize.SyncNature, defaultNatureGenerator));

            var indivFactory = new StandardIndivGeneratorFactory(BuildIndivGenerator(pidGenerator, GetDefaultItemSelector()));

            return BuildWildGenerator(enc, slotSelector, lvGenerator, indivFactory);
        }

        public override WildGenerator Resolve(CuteCharm cuteCharm)
        {
            var enc = new PokeRaderEncounterer(DPtGrassEncounterer.CreateInstance(mapData.BasicEncounterRate, inAlleviated));

            var slotSelector = GetSlotSelector();
            var lvGenerator = GetDefaultLvGenerator();

            var indivFactory = CuteCharmIndivGeneratorFactory.GetInstance(
                default(TVersion).Unwrap(), BuildIndivGenerator(GetPIDGenerator(), GetDefaultItemSelector()),
                (ratio) => new StandardIndivGenerator(new GenderPredeterminedPIDGenerator(GetPIDGenerator(), cuteCharm.FixedGender, ratio), GetDefaultItemSelector())
            );

            return BuildWildGenerator(enc, slotSelector, lvGenerator, indivFactory);
        }


        public ShinyPokeRaderWildGeneratorFactory(IMapData<TVersion, TEncType> mapData, bool inAlleviated, Func<IMapData<TVersion, TEncType>, Slot> slotSelector, ushort tid, ushort sid) : base(mapData, inAlleviated, slotSelector)
            => this.shinyPIDGenerator = new ForceShinyPIDGeneratorAtom(tid, sid);
    }
}
