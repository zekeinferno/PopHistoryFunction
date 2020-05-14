using System;
using System.Collections.Generic;

namespace PopHistoryFunction.EntityFramework
{
    public partial class PsaCustomSetCard
    {
        public int Id { get; set; }
        public int CustomSetId { get; set; }
        public int CardId { get; set; }

        public PsaCard Card { get; set; }
        public PsaCustomSet CustomSet { get; set; }
    }
}
