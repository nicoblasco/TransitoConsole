namespace ConsoleTransito
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ProcessLogs
    {
        public int Id { get; set; }

        public DateTime Fecha { get; set; }

        public string Name { get; set; }

        public bool IsOk { get; set; }

        public string ErrorDescripcion { get; set; }

        public int ProcessId { get; set; }

        public virtual Processes Processes { get; set; }
    }
}
