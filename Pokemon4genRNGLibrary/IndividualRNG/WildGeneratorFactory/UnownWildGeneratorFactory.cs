using System;
using Pokemon4genMapData;

namespace Pokemon4genRNGLibrary
{
    class UnownGeneratorFactory<TVersion, TEncType> : WildGeneratorFactory<TVersion, TEncType>
        where TVersion : struct, IWrappedGameVersion
        where TEncType : struct, IWrappedEncounterType<TVersion> 
    {
        public UnownGeneratorFactory(IMapData<TVersion, TEncType> mapData): base(mapData)
        {
            switch (mapData.Type)
            {
                case MapType.RuinCubicle:
                    unownFormGenerator = new DPtCommonUnownFormGenerator(); break;
                case MapType.RuinHall_F:
                    unownFormGenerator = new DPtFixedUnownFormGenerator("F"); break;
                case MapType.RuinHall_R:
                    unownFormGenerator = new DPtFixedUnownFormGenerator("R"); break;
                case MapType.RuinHall_I:
                    unownFormGenerator = new DPtFixedUnownFormGenerator("I"); break;
                case MapType.RuinHall_E:
                    unownFormGenerator = new DPtFixedUnownFormGenerator("E"); break;
                case MapType.RuinHall_N:
                    unownFormGenerator = new DPtFixedUnownFormGenerator("N"); break;
                case MapType.RuinHall_D:
                    unownFormGenerator = new DPtFixedUnownFormGenerator("D"); break;
                case MapType.RuinHiddenRoom:
                    unownFormGenerator = new RareUnownFormGenerator(); break;
                default:
                    throw new Exception("Bad MapType");
            }
        }

        private readonly IUnownFormGenerator unownFormGenerator;
        protected override IIndivGenerator BuildIndivGenerator(IPIDGenerator pidGenerator, IHoldItemSelector holdItemSelector)
            => new UnownIndivGenerator(pidGenerator, holdItemSelector, unownFormGenerator);
    }

    class RuinsOfAlphGeneratorFactory<TVersion, TEncType> : WildGeneratorFactory<TVersion, TEncType>
        where TVersion : struct, IWrappedGameVersion
        where TEncType : struct, IWrappedEncounterType<TVersion>
    {
        public RuinsOfAlphGeneratorFactory(IMapData<TVersion, TEncType> mapData, RuinsOfAlphPuzzleProgress progress) : base(mapData)
        {
            if(mapData.Type != MapType.RuinsOfAlph_Hall)
                throw new Exception("Bad MapType");

            unownFormGenerator = new HGSSCommonUnownFormGenerator(progress);
        }
        public RuinsOfAlphGeneratorFactory(IMapData<TVersion, TEncType> mapData, RuinsOfAlphPuzzleProgress progress, GotUnowns gotUnowns) : base(mapData)
        {
            if (mapData.Type != MapType.RuinsOfAlph_Hall)
                throw new Exception("Bad MapType");

            if((uint)gotUnowns == 0x1FFFFFF && progress.AllSolved)
                unownFormGenerator = new HGSSCommonUnownFormGenerator(progress);
            else
                unownFormGenerator = HGSSRadioUnownFormGenerator.CreateInstance(progress, gotUnowns);
        }

        private readonly IUnownFormGenerator unownFormGenerator;
        protected override IIndivGenerator BuildIndivGenerator(IPIDGenerator pidGenerator, IHoldItemSelector holdItemSelector)
            => new UnownIndivGenerator(pidGenerator, holdItemSelector, unownFormGenerator);
    }
}
