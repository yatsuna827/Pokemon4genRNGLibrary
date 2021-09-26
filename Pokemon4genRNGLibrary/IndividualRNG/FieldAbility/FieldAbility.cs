using System;
using System.Collections.Generic;
using System.Text;
using Pokemon4genMapData;

namespace Pokemon4genRNGLibrary
{
    public abstract class FieldAbility
    {
        internal abstract WildGenerator Accept<TVersion, TEncType>(WildGeneratorFactory<TVersion, TEncType> factory)
            where TVersion : struct, IWrappedGameVersion
            where TEncType : struct, IWrappedEncounterType<TVersion>;
    }

}
