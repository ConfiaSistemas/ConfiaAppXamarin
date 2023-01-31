using System;
using System.Collections.Generic;
using System.Text;

namespace ConfiApp.Modelos
{
    public class Output
    {
    }
    public class ModelTicket
    {
        public List<object> recordsets { get; set; }
        public Output output { get; set; }
        public List<object> rowsAffected { get; set; }
        public int returnValue { get; set; }
    }
}
