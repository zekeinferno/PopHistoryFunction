using System;
using System.Collections.Generic;

namespace PopHistoryFunction.EntityFramework
{
    public partial class Series
    {
        public Series()
        {
            PsaCustomSet = new HashSet<PsaCustomSet>();
            PsaSet = new HashSet<PsaSet>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<PsaCustomSet> PsaCustomSet { get; set; }
        public ICollection<PsaSet> PsaSet { get; set; }
    }
}
