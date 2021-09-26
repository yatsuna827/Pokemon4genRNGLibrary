using System;
using Pokemon4genMapData;
using PokemonStandardLibrary.PokeDex.Gen4;

namespace Pokemon4genRNGLibrary
{
    // 色確定ならベースのpidGeneratorを変える.
    // サファリ系ならベースのindivGeneratorを変える.
    // シンクロ、メロボ、プレッシャー、静電気、磁力、発光、悪臭、複眼

    // FieldAbility、GameVersion、MapType<GameVersion>、エンカウント判定をするかどうか(WithEncounterHANTEI)、
    // 判定をする場合は自転車とかビードロとかお札とか
    // ポケトレだとさらになんかややこしいかもしれない...

    // HGSS/DPt
    // サファリ系
    // 色確定

    // - HGSSアンノーンの進捗
    // - HGSSアンノーンラジオの有無
    // - DPtポケトレ大揺れフラグ
    // - DPtポケトレ光る草むら
    // - DPtヒンバス

    // 別のライブラリから渡ってくるのはMapData<GameVersion>.
    // フィールドでMapTypeが取れる.

    // EncounterTypeによってGeneratorに影響する場合がある
    // - 釣り系・岩砕きは強制的にWithEncounter

    // MapTypeは本質的にGeneratorの分岐のために導入した概念
    // - 純粋にはMapDataに持たせるべきですらない.

    // MapType(=MapData. メソッド内部でMapTypeを見て出し分ければいいので...) x FieldAbilityでダブルディスパッチ？
    // エンドポイントはWildGenerator.CreateWildGenerator(mapData, fieldAbility, args)にしたい…けど！

    // WildGeneratorの分岐って、具体的に何が変わるの？
    // - SlotSelector
    //   - フィールド特性の静電気/磁力に依存する
    //   - OptionalSlot
    // - LvGenerator
    //   - EncounterTypeによって有無が違う
    //   - プレッシャー
    // - IndivGeneratorFactory
    //   - 


    // フィールド特性は幽霊型にしない！！！
    // 内部でWildGeneratorとダブルディスパッチする.
    class WildGeneratorFactory<TVersion, TEncType>
        where TVersion : struct, IWrappedGameVersion
        where TEncType : struct, IWrappedEncounterType<TVersion>
    {
        private protected readonly IMapData<TVersion, TEncType> mapData;

        public WildGeneratorFactory(IMapData<TVersion, TEncType> mapData)
            => this.mapData = mapData;

        protected IEncounterer GetEncounterer()
        {
            switch (mapData.Type)
            {
                case MapType.OldRod:
                case MapType.GoodRod:
                case MapType.SuperRod:
                case MapType.RockSmash:
                    return SimpleEncounterer.CreateInstance(default(TVersion).Unwrap(), mapData.BasicEncounterRate);
            }

            return new ConfirmEncounterer();
        }

        // 特に分岐はしなくて良いと思う.
        protected ISlotSelector GetDefaultSlotSelector()
        {
            var s = StandardSlotSelector.CreateInstance(default(TVersion).Unwrap(), mapData.EncounterTable);
            if (mapData.OptionalSlots.Count > 0) return new OptionalSlotSelector(mapData.OptionalSlots[0].Convert(Pokemon.GetPokemon), s);

            return s;
        }

        // Grass系とそれ以外で分岐.
        protected ILvGenerator GetDefaultLvGenerator()
        {
            if (default(TEncType).Unwrap() == EncounterType.Grass)
                return FixedLvGenerator.GetInstance();
            else
                return VariableLvGenerator.GetInstance();
        }

        // 色確定のときに変える？
        protected IPIDGenerator GetDefaultPIDGenerator()
            => new NaturePredeterminedPIDGenerator(StandardPIDGenerator.Instance, StandardNatureGenerator.GetInstance(default(TVersion).Unwrap()));

        // 特に分岐はしなくて良いと思う.
        protected IHoldItemSelector GetDefaultItemSelector()
            => new StandardItemSelector(default(TVersion).Unwrap());

        protected virtual IIndivGenerator BuildIndivGenerator(IPIDGenerator pidGenerator, IHoldItemSelector holdItemSelector)
            => new StandardIndivGenerator(pidGenerator, holdItemSelector);

        protected WildGenerator BuildWildGenerator(IEncounterer encounterer, ISlotSelector slotSelector, ILvGenerator lvGenerator, IIndividualGeneratorFactory individualGeneratorFactory)
            => new WildGenerator(encounterer, slotSelector, lvGenerator, individualGeneratorFactory, (int)default(TVersion).Unwrap() < 2 ? 5 : 1u);

        public virtual WildGenerator Resolve()
        {
            var enc = GetEncounterer();
            var slotSelector = GetDefaultSlotSelector();
            var lvGenerator = GetDefaultLvGenerator();

            var pidGenerator = GetDefaultPIDGenerator();

            var indivFactory = new StandardIndivGeneratorFactory(BuildIndivGenerator(pidGenerator, GetDefaultItemSelector()));

            return BuildWildGenerator(enc, slotSelector, lvGenerator, indivFactory);
        }

        public virtual WildGenerator Resolve(Synchronize synchronize)
        {
            var enc = GetEncounterer();

            var slotSelector = GetDefaultSlotSelector();
            var lvGenerator = GetDefaultLvGenerator();

            var defaultNatureGenerator = StandardNatureGenerator.GetInstance(default(TVersion).Unwrap());

            var pidGenerator = new NaturePredeterminedPIDGenerator(StandardPIDGenerator.Instance, SynchronizeNatureGenerator.GetInstance(default(TVersion).Unwrap(), synchronize.SyncNature, defaultNatureGenerator));

            var indivFactory = new StandardIndivGeneratorFactory(BuildIndivGenerator(pidGenerator, GetDefaultItemSelector()));

            return BuildWildGenerator(enc, slotSelector, lvGenerator, indivFactory);
        }

        public virtual WildGenerator Resolve(CuteCharm cuteCharm)
        {
            var enc = GetEncounterer();

            var slotSelector = GetDefaultSlotSelector();
            var lvGenerator = GetDefaultLvGenerator();

            var defaultNatureGenerator = StandardNatureGenerator.GetInstance(default(TVersion).Unwrap());

            var defaultGenerator = BuildIndivGenerator(GetDefaultPIDGenerator(), GetDefaultItemSelector());

            var indivFactory = CuteCharmIndivGeneratorFactory.GetInstance(
                default(TVersion).Unwrap(),
                defaultGenerator,
                (ratio) => BuildIndivGenerator(new CuteCharmPIDGenerator(defaultNatureGenerator, cuteCharm.FixedGender, ratio), GetDefaultItemSelector())
            );

            return BuildWildGenerator(enc, slotSelector, lvGenerator, indivFactory);
        }

        public virtual WildGenerator Resolve(Pressure _)
        {
            var enc = GetEncounterer();

            var slotSelector = GetDefaultSlotSelector();
            var lvGenerator = PressureLvGenerator.CreateInstanse(default(TVersion).Unwrap(), GetDefaultLvGenerator());

            var pidGenerator = GetDefaultPIDGenerator();

            var indivFactory = new StandardIndivGeneratorFactory(BuildIndivGenerator(pidGenerator, GetDefaultItemSelector()));

            return BuildWildGenerator(enc, slotSelector, lvGenerator, indivFactory);
        }

        public virtual WildGenerator Resolve(Static _)
        {
            var enc = GetEncounterer();

            var slotSelector = StaticSlotSelector.CreateInstance(default(TVersion).Unwrap(), GetDefaultSlotSelector(), mapData.EncounterTable);
            var lvGenerator = GetDefaultLvGenerator();

            var pidGenerator = GetDefaultPIDGenerator();

            var indivFactory = new StandardIndivGeneratorFactory(BuildIndivGenerator(pidGenerator, GetDefaultItemSelector()));

            return BuildWildGenerator(enc, slotSelector, lvGenerator, indivFactory);
        }

        public virtual WildGenerator Resolve(MagnetPull _)
        {
            var enc = GetEncounterer();

            var slotSelector = MagnetPullSlotSelector.CreateInstance(default(TVersion).Unwrap(), GetDefaultSlotSelector(), mapData.EncounterTable);
            var lvGenerator = GetDefaultLvGenerator();

            var pidGenerator = GetDefaultPIDGenerator();

            var indivFactory = new StandardIndivGeneratorFactory(BuildIndivGenerator(pidGenerator, GetDefaultItemSelector()));

            return BuildWildGenerator(enc, slotSelector, lvGenerator, indivFactory);
        }

        public virtual WildGenerator Resolve(CompoundEyes _)
        {
            var enc = GetEncounterer();

            var slotSelector = GetDefaultSlotSelector();
            var lvGenerator = GetDefaultLvGenerator();

            var pidGenerator = GetDefaultPIDGenerator();

            var indivFactory = new StandardIndivGeneratorFactory(BuildIndivGenerator(pidGenerator, new CompoundEyesItemSelector(default(TVersion).Unwrap())));

            return BuildWildGenerator(enc, slotSelector, lvGenerator, indivFactory);
        }

        public virtual WildGenerator Resolve(Stench _)
        {
            if (mapData.Type != MapType.RockSmash) return Resolve();

            var enc = SimpleEncounterer.CreateInstance(default(TVersion).Unwrap(), mapData.BasicEncounterRate / 2);
            var slotSelector = GetDefaultSlotSelector();
            var lvGenerator = GetDefaultLvGenerator();

            var pidGenerator = GetDefaultPIDGenerator();

            var indivFactory = new StandardIndivGeneratorFactory(BuildIndivGenerator(pidGenerator, GetDefaultItemSelector()));

            return BuildWildGenerator(enc, slotSelector, lvGenerator, indivFactory);
        }

        public virtual WildGenerator Resolve(Illuminate _)
        {
            var v = default(TVersion).Unwrap();
            // DPtの場合は無視
            if ((int)v < 3) return Resolve();
            // 草むら/波乗りは現状で無視
            if(mapData.Type != MapType.OldRod || mapData.Type != MapType.GoodRod || mapData.Type != MapType.SuperRod || mapData.Type != MapType.RockSmash) return Resolve();

            var enc = SimpleEncounterer.CreateInstance(default(TVersion).Unwrap(), mapData.BasicEncounterRate * 2);
            var slotSelector = GetDefaultSlotSelector();
            var lvGenerator = GetDefaultLvGenerator();

            var pidGenerator = GetDefaultPIDGenerator();

            var indivFactory = new StandardIndivGeneratorFactory(BuildIndivGenerator(pidGenerator, GetDefaultItemSelector()));

            return BuildWildGenerator(enc, slotSelector, lvGenerator, indivFactory);
        }

    }
}
