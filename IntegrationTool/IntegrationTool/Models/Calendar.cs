//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace IntegrationTool.Models
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    
    public partial class Calendar
    {
        public int CalendarId { get; set; }
        public System.DateTime ExecutionStartDate { get; set; }
        public Nullable<System.DateTime> NextExecutionDate { get; set; }
        public string Emails { get; set; }
        public int IntegrationId { get; set; }
        public int RecurrenceId { get; set; }
        public System.DateTime ExecutionEndDate { get; set; }

        [JsonIgnore]
        public virtual Integration Integration { get; set; }
        [JsonIgnore]
        public virtual Recurrence Recurrence { get; set; }
    }
}
