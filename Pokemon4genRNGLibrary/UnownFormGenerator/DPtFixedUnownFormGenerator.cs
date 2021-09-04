using System;
using System.Collections.Generic;
using System.Text;

namespace Pokemon4genRNGLibrary
{
    class DPtFixedUnownFormGenerator: IUnownFormGenerator
    {
        private readonly string fixedForm;
        public string GenerateUnownForm(ref uint seed)
            => fixedForm;

        public DPtFixedUnownFormGenerator(string form)
            => fixedForm = form;
    }
}
