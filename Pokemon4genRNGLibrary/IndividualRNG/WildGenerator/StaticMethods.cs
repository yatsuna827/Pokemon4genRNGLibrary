using PokemonPRNG.LCG32;
using PokemonPRNG.LCG32.StandardLCG;
using PokemonStandardLibrary.PokeDex.Gen4;
using Pokemon4genMapData;

namespace Pokemon4genRNGLibrary
{
    // CreateInstanceが大量に生えているのでファイルを分割した
    public partial class WildGenerator
    {
        public static WildGenerator CreateInstance<TVersion, TEncType>(IMapData<TVersion, TEncType> mapData)
            where TVersion : struct, IWrappedGameVersion
            where TEncType : struct, IWrappedEncounterType<TVersion>
        {
            WildGeneratorFactory<TVersion, TEncType> fac;
            switch (mapData.Type)
            {
                case MapType.BugCatching:
                    fac = new RerollWildGeneratorFactory<TVersion, TEncType>(mapData); break;
                case MapType.RuinHall_F:
                case MapType.RuinHall_R:
                case MapType.RuinHall_I:
                case MapType.RuinHall_E:
                case MapType.RuinHall_N:
                case MapType.RuinHall_D:
                case MapType.RuinCubicle:
                case MapType.RuinHiddenRoom:
                case MapType.RuinsOfAlph_Entrance:
                    fac = new UnownGeneratorFactory<TVersion, TEncType>(mapData); break;
                case MapType.RuinsOfAlph_Hall:
                    fac = new RuinsOfAlphGeneratorFactory<TVersion, TEncType>(mapData, null); break;
                default:
                    fac = new WildGeneratorFactory<TVersion, TEncType>(mapData); break;
            }

            return fac.Resolve();
        }
        public static WildGenerator CreateInstance<TVersion, TEncType>(IMapData<TVersion, TEncType> mapData, FieldAbility fieldAbility)
            where TVersion : struct, IWrappedGameVersion
            where TEncType : struct, IWrappedEncounterType<TVersion>
        {
            WildGeneratorFactory<TVersion, TEncType> fac;
            switch (mapData.Type)
            {
                case MapType.BugCatching:
                    fac = new RerollWildGeneratorFactory<TVersion, TEncType>(mapData); break;
                case MapType.RuinHall_F:
                case MapType.RuinHall_R:
                case MapType.RuinHall_I:
                case MapType.RuinHall_E:
                case MapType.RuinHall_N:
                case MapType.RuinHall_D:
                case MapType.RuinCubicle:
                case MapType.RuinHiddenRoom:
                case MapType.RuinsOfAlph_Entrance:
                    fac = new UnownGeneratorFactory<TVersion, TEncType>(mapData); break;
                case MapType.RuinsOfAlph_Hall:
                    fac = new RuinsOfAlphGeneratorFactory<TVersion, TEncType>(mapData, null); break;
                default:
                    fac = new WildGeneratorFactory<TVersion, TEncType>(mapData); break;
            }

            return fieldAbility.Accept(fac);
        }



        public static WildGenerator CreateInstance(IMapData<WrappedHeartGold, WrappedRuinOfAlph> mapData, RuinsOfAlphPuzzleProgress progress)
        {
            var fac = new RuinsOfAlphGeneratorFactory<WrappedHeartGold, WrappedRuinOfAlph>(mapData, progress);

            return fac.Resolve();
        }
        public static WildGenerator CreateInstance(IMapData<WrappedSoulSilver, WrappedRuinOfAlph> mapData, RuinsOfAlphPuzzleProgress progress)
        {
            var fac = new RuinsOfAlphGeneratorFactory<WrappedSoulSilver, WrappedRuinOfAlph>(mapData, progress);

            return fac.Resolve();
        }

        public static WildGenerator CreateInstance(IMapData<WrappedHeartGold, WrappedRuinOfAlph> mapData, RuinsOfAlphPuzzleProgress progress, GotUnowns gotUnowns)
        {
            var fac = new RuinsOfAlphGeneratorFactory<WrappedHeartGold, WrappedRuinOfAlph>(mapData, progress, gotUnowns);

            return fac.Resolve();
        }
        public static WildGenerator CreateInstance(IMapData<WrappedSoulSilver, WrappedRuinOfAlph> mapData, RuinsOfAlphPuzzleProgress progress, GotUnowns gotUnowns)
        {
            var fac = new RuinsOfAlphGeneratorFactory<WrappedSoulSilver, WrappedRuinOfAlph>(mapData, progress, gotUnowns);

            return fac.Resolve();
        }



        public static WildGenerator CreateInstance(IMapData<WrappedHeartGold, WrappedRuinOfAlph> mapData, FieldAbility fieldAbility, RuinsOfAlphPuzzleProgress progress)
        {
            var fac = new RuinsOfAlphGeneratorFactory<WrappedHeartGold, WrappedRuinOfAlph>(mapData, progress);

            return fieldAbility.Accept(fac);
        }
        public static WildGenerator CreateInstance(IMapData<WrappedSoulSilver, WrappedRuinOfAlph> mapData, FieldAbility fieldAbility, RuinsOfAlphPuzzleProgress progress)
        {
            var fac = new RuinsOfAlphGeneratorFactory<WrappedSoulSilver, WrappedRuinOfAlph>(mapData, progress);

            return fieldAbility.Accept(fac);
        }

        public static WildGenerator CreateInstance(IMapData<WrappedHeartGold, WrappedRuinOfAlph> mapData, FieldAbility fieldAbility, RuinsOfAlphPuzzleProgress progress, GotUnowns gotUnowns)
        {
            var fac = new RuinsOfAlphGeneratorFactory<WrappedHeartGold, WrappedRuinOfAlph>(mapData, progress, gotUnowns);

            return fieldAbility.Accept(fac);
        }
        public static WildGenerator CreateInstance(IMapData<WrappedSoulSilver, WrappedRuinOfAlph> mapData, FieldAbility fieldAbility, RuinsOfAlphPuzzleProgress progress, GotUnowns gotUnowns)
        {
            var fac = new RuinsOfAlphGeneratorFactory<WrappedSoulSilver, WrappedRuinOfAlph>(mapData, progress, gotUnowns);

            return fieldAbility.Accept(fac);
        }

    }
}
