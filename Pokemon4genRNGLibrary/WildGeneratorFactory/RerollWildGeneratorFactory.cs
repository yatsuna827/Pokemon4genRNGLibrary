using Pokemon4genMapData;

namespace Pokemon4genRNGLibrary
{
    class RerollWildGeneratorFactory<TVersion, TEncType> : WildGeneratorFactory<TVersion, TEncType>
        where TVersion : struct, IWrappedGameVersion
        where TEncType : struct, IWrappedEncounterType<TVersion>
    {
        public RerollWildGeneratorFactory(IMapData<TVersion, TEncType> mapData) : base(mapData) { }

        protected override IIndivGenerator BuildIndivGenerator(IPIDGenerator pidGenerator, IHoldItemSelector holdItemSelector)
            => new RerollIndivGenerator(pidGenerator, holdItemSelector);

        public override WildGenerator Resolve(CuteCharm cuteCharm)
        {
            var enc = GetEncounterer();

            var slotSelector = GetDefaultSlotSelector();
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

    }
}
