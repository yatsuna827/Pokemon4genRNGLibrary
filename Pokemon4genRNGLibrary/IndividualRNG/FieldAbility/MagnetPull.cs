using System;
using System.Collections.Generic;
using System.Text;

namespace Pokemon4genRNGLibrary
{
    public class MagnetPull : FieldAbility
    {
        internal override WildGenerator Accept<TVersion, TEncType>(WildGeneratorFactory<TVersion, TEncType> factory)
            => factory.Resolve(this);
    }
}
