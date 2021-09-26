using System;
using System.Collections.Generic;
using System.Text;
using PokemonStandardLibrary;

namespace Pokemon4genRNGLibrary
{
    public class Synchronize : FieldAbility
    {
        public Nature SyncNature { get; }
        internal override WildGenerator Accept<TVersion, TEncType>(WildGeneratorFactory<TVersion, TEncType> factory)
            => factory.Resolve(this);

        public Synchronize(Nature nature)
            => SyncNature = nature;
    }
}
