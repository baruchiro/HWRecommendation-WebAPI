using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.ML;

namespace Regression.Extensions
{
    public static class DataViewPrintExtensions
    {
        public static void PrintPreview(this IDataView dataView, int maxRows = 100)
        {
            var preview = dataView.Preview(maxRows);
            foreach (var rowInfo in preview.RowView)
            {
                Console.WriteLine(string.Join('\t', rowInfo.Values));
            }
        }
    }
}
