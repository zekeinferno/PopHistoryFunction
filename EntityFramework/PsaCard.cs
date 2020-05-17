using System;
using System.Collections.Generic;

namespace PopHistoryFunction.EntityFramework
{
    public partial class PsaCard
    {
        public PsaCard()
        {
            PsaCustomSetCard = new HashSet<PsaCustomSetCard>();
            PsaPopHistoryFunction = new HashSet<PsaPopHistory>();
        }

        public int Id { get; set; }
        public int SetId { get; set; }
        public string CardNumber { get; set; }
        public string NamePrimary { get; set; }
        public string NameSecondary { get; set; }
        public int CurrentTotalGraded { get; set; }
        public int CurrentPop10 { get; set; }

        public PsaSet Set { get; set; }
        public ICollection<PsaCustomSetCard> PsaCustomSetCard { get; set; }
        public ICollection<PsaPopHistory> PsaPopHistoryFunction { get; set; }
    }
}
