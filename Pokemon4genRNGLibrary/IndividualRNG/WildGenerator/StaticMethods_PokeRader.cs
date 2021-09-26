using System;
using System.Collections.Generic;
using System.Text;
using Pokemon4genMapData;

namespace Pokemon4genRNGLibrary
{
    public class PokeRaderArgs
    {
        public bool inAlleviated;
    }
    public class ChainPokeRaderArgs<TVersion, TEncType> : PokeRaderArgs
        where TVersion : struct, IWrappedGameVersion
        where TEncType : struct, IWrappedEncounterType<TVersion> 
    {
        public Func<IMapData<TVersion, TEncType>, Slot> slotSelector;
    }
    public class ShinyPokeRaderArgs<TVersion, TEncType> : ChainPokeRaderArgs<TVersion, TEncType>
        where TVersion : struct, IWrappedGameVersion
        where TEncType : struct, IWrappedEncounterType<TVersion> 
    {
        public ushort TID, SID;
    }

    public partial class WildGenerator
    {
        public static WildGenerator CreateInstance(IMapData<WrappedDiamond, WrappedGrass> mapData, PokeRaderArgs args)
        {
            var fac = new PokeRaderWildGeneratorFactory<WrappedDiamond, WrappedGrass>(mapData, args?.inAlleviated ?? false);

            return fac.Resolve();
        }
        public static WildGenerator CreateInstance(IMapData<WrappedDiamond, WrappedGrass> mapData, FieldAbility fieldAbility, PokeRaderArgs args)
        {
            var fac = new PokeRaderWildGeneratorFactory<WrappedDiamond, WrappedGrass>(mapData, args?.inAlleviated ?? false);

            return fieldAbility.Accept(fac);
        }
        public static WildGenerator CreateInstance(IMapData<WrappedDiamond, WrappedGrass> mapData, ChainPokeRaderArgs<WrappedDiamond, WrappedGrass> args)
        {
            var fac = new ChainPokeRaderWildGeneratorFactory<WrappedDiamond, WrappedGrass>(mapData, args?.inAlleviated ?? false, args?.slotSelector);

            return fac.Resolve();
        }
        public static WildGenerator CreateInstance(IMapData<WrappedDiamond, WrappedGrass> mapData, FieldAbility fieldAbility, ChainPokeRaderArgs<WrappedDiamond, WrappedGrass> args)
        {
            var fac = new ChainPokeRaderWildGeneratorFactory<WrappedDiamond, WrappedGrass>(mapData, args?.inAlleviated ?? false, args?.slotSelector);

            return fieldAbility.Accept(fac);
        }
        public static WildGenerator CreateInstance(IMapData<WrappedDiamond, WrappedGrass> mapData, ShinyPokeRaderArgs<WrappedDiamond, WrappedGrass> args)
        {
            var fac = new ShinyPokeRaderWildGeneratorFactory<WrappedDiamond, WrappedGrass>(mapData, args?.inAlleviated ?? false, args?.slotSelector, args?.TID ?? 0, args?.SID ?? 0);

            return fac.Resolve();
        }
        public static WildGenerator CreateInstance(IMapData<WrappedDiamond, WrappedGrass> mapData, FieldAbility fieldAbility, ShinyPokeRaderArgs<WrappedDiamond, WrappedGrass> args)
        {
            var fac = new ShinyPokeRaderWildGeneratorFactory<WrappedDiamond, WrappedGrass>(mapData, args?.inAlleviated ?? false, args?.slotSelector, args?.TID ?? 0, args?.SID ?? 0);

            return fieldAbility.Accept(fac);
        }


        public static WildGenerator CreateInstance(IMapData<WrappedPearl, WrappedGrass> mapData, PokeRaderArgs args)
        {
            var fac = new PokeRaderWildGeneratorFactory<WrappedPearl, WrappedGrass>(mapData, args?.inAlleviated ?? false);

            return fac.Resolve();
        }
        public static WildGenerator CreateInstance(IMapData<WrappedPearl, WrappedGrass> mapData, FieldAbility fieldAbility, PokeRaderArgs args)
        {
            var fac = new PokeRaderWildGeneratorFactory<WrappedPearl, WrappedGrass>(mapData, args?.inAlleviated ?? false);

            return fieldAbility.Accept(fac);
        }
        public static WildGenerator CreateInstance(IMapData<WrappedPearl, WrappedGrass> mapData, ChainPokeRaderArgs<WrappedPearl, WrappedGrass> args)
        {
            var fac = new ChainPokeRaderWildGeneratorFactory<WrappedPearl, WrappedGrass>(mapData, args?.inAlleviated ?? false, args?.slotSelector);

            return fac.Resolve();
        }
        public static WildGenerator CreateInstance(IMapData<WrappedPearl, WrappedGrass> mapData, FieldAbility fieldAbility, ChainPokeRaderArgs<WrappedPearl, WrappedGrass> args)
        {
            var fac = new ChainPokeRaderWildGeneratorFactory<WrappedPearl, WrappedGrass>(mapData, args?.inAlleviated ?? false, args?.slotSelector);

            return fieldAbility.Accept(fac);
        }
        public static WildGenerator CreateInstance(IMapData<WrappedPearl, WrappedGrass> mapData, ShinyPokeRaderArgs<WrappedPearl, WrappedGrass> args)
        {
            var fac = new ShinyPokeRaderWildGeneratorFactory<WrappedPearl, WrappedGrass>(mapData, args?.inAlleviated ?? false, args?.slotSelector, args?.TID ?? 0, args?.SID ?? 0);

            return fac.Resolve();
        }
        public static WildGenerator CreateInstance(IMapData<WrappedPearl, WrappedGrass> mapData, FieldAbility fieldAbility, ShinyPokeRaderArgs<WrappedPearl, WrappedGrass> args)
        {
            var fac = new ShinyPokeRaderWildGeneratorFactory<WrappedPearl, WrappedGrass>(mapData, args?.inAlleviated ?? false, args?.slotSelector, args?.TID ?? 0, args?.SID ?? 0);

            return fieldAbility.Accept(fac);
        }



        public static WildGenerator CreateInstance(IMapData<WrappedPlatinum, WrappedGrass> mapData, PokeRaderArgs args)
        {
            var fac = new PokeRaderWildGeneratorFactory<WrappedPlatinum, WrappedGrass>(mapData, args?.inAlleviated ?? false);

            return fac.Resolve();
        }
        public static WildGenerator CreateInstance(IMapData<WrappedPlatinum, WrappedGrass> mapData, FieldAbility fieldAbility, PokeRaderArgs args)
        {
            var fac = new PokeRaderWildGeneratorFactory<WrappedPlatinum, WrappedGrass>(mapData, args?.inAlleviated ?? false);

            return fieldAbility.Accept(fac);
        }
        public static WildGenerator CreateInstance(IMapData<WrappedPlatinum, WrappedGrass> mapData, ChainPokeRaderArgs<WrappedPlatinum, WrappedGrass> args)
        {
            var fac = new ChainPokeRaderWildGeneratorFactory<WrappedPlatinum, WrappedGrass>(mapData, args?.inAlleviated ?? false, args?.slotSelector);

            return fac.Resolve();
        }
        public static WildGenerator CreateInstance(IMapData<WrappedPlatinum, WrappedGrass> mapData, FieldAbility fieldAbility, ChainPokeRaderArgs<WrappedPlatinum, WrappedGrass> args)
        {
            var fac = new ChainPokeRaderWildGeneratorFactory<WrappedPlatinum, WrappedGrass>(mapData, args?.inAlleviated ?? false, args?.slotSelector);

            return fieldAbility.Accept(fac);
        }
        public static WildGenerator CreateInstance(IMapData<WrappedPlatinum, WrappedGrass> mapData, ShinyPokeRaderArgs<WrappedPlatinum, WrappedGrass> args)
        {
            var fac = new ShinyPokeRaderWildGeneratorFactory<WrappedPlatinum, WrappedGrass>(mapData, args?.inAlleviated ?? false, args?.slotSelector, args?.TID ?? 0, args?.SID ?? 0);

            return fac.Resolve();
        }
        public static WildGenerator CreateInstance(IMapData<WrappedPlatinum, WrappedGrass> mapData, FieldAbility fieldAbility, ShinyPokeRaderArgs<WrappedPlatinum, WrappedGrass> args)
        {
            var fac = new ShinyPokeRaderWildGeneratorFactory<WrappedPlatinum, WrappedGrass>(mapData, args?.inAlleviated ?? false, args?.slotSelector, args?.TID ?? 0, args?.SID ?? 0);

            return fieldAbility.Accept(fac);
        }
    }
}
