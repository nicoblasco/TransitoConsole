namespace ConsoleTransito
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Processes
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Processes()
        {
            ProcessLogs = new HashSet<ProcessLogs>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime FechaInicio { get; set; }

        public DateTime? FechaFin { get; set; }

        public string Detalle { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProcessLogs> ProcessLogs { get; set; }
    }
}
