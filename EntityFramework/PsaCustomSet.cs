using System;
using System.Collections.Generic;

namespace PopHistoryFunction.EntityFramework
{
    public partial class PsaCustomSet
    {
        public PsaCustomSet()
        {
            PsaCustomSetCard = new HashSet<PsaCustomSetCard>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int SeriesId { get; set; }

        public Series Series { get; set; }
        public ICollection<PsaCustomSetCard> PsaCustomSetCard { get; set; }
    }
}
