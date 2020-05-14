using System;
using System.Collections.Generic;

namespace PopHistoryFunction.EntityFramework
{
    public partial class PsaSet
    {
        public PsaSet()
        {
            PsaCard = new HashSet<PsaCard>();
        }

        public int Id { get; set; }
        public int CategoryId { get; set; }
        public int HeadingId { get; set; }
        public int SeriesId { get; set; }
        public string Name { get; set; }
        public int Year { get; set; }

        public Series Series { get; set; }
        public ICollection<PsaCard> PsaCard { get; set; }
    }
}
