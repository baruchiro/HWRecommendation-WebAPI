using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.ML.Data;

namespace Regression.DataClasses
{
    public class Person
    {
        [ColumnName("Age")]
        public int Age { get; set; }
        [ColumnName("FieldInterest")]
        public string FieldInterest { get; set; }
        [ColumnName("MainUse")]
        public string MainUse { get; set; }
        [ColumnName("Gender")]
        public string Gender { get; set; }
        [ColumnName("People")]
        public int People { get; set; }
        [ColumnName("ComputerType")]
        public string ComputerType { get; set; }
        [ColumnName("Price")]
        public int Price { get; set; }
    }
}
