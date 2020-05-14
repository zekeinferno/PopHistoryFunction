using System;
using System.Collections.Generic;

namespace PopHistoryFunction.EntityFramework
{
    public partial class PsaPopHistoryFunction
    {
        public int Id { get; set; }
        public int CardId { get; set; }
        public int? PopAuth { get; set; }
        public int? Pop01 { get; set; }
        public int? Pop02 { get; set; }
        public int? Pop03 { get; set; }
        public int? Pop04 { get; set; }
        public int? Pop05 { get; set; }
        public int? Pop06 { get; set; }
        public int? Pop07 { get; set; }
        public int? Pop08 { get; set; }
        public int? Pop085 { get; set; }
        public int? Pop09 { get; set; }
        public int? Pop095 { get; set; }
        public int? Pop10 { get; set; }
        public DateTime DateCreated { get; set; }

        public PsaCard Card { get; set; }
    }
}
