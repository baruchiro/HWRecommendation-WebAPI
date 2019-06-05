using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.ML;
using Microsoft.ML.Data;

namespace Regression.Extensions
{
    public static class DataViewPrintExtensions
    {
        public static void PrintPreview(this IDataView dataView, int maxRows = 100, TextWriter writer = null)
        {
            dataView.Preview(maxRows).Print(writer);
        }

        public static void Print(this DataDebuggerPreview preview, TextWriter writer = null)
        {
            writer = writer ?? Console.Out;

            writer.WriteLine(string.Join('\t', preview.Schema.Select(c => c.Name)));

            writer.WriteLine(
                string.Join(Environment.NewLine, preview.RowView.Select(r =>
                    string.Join('\t', r.Values.Select(kv => kv.Value)))));
        }

        public static void PrintPreviewByColumn(this IDataView dataView, int maxRows = 100, TextWriter writer = null)
        {
            dataView.Preview(maxRows).PrintByColumn(writer: writer);
        }

        public static void PrintByColumn(this DataDebuggerPreview preview, bool isHidden = false, TextWriter writer = null, string separatorLine = null)
        {
            writer = writer ?? Console.Out;

            writer.WriteLine(separatorLine);

            var columns = preview.ColumnView.AsEnumerable();
            if (!isHidden)
            {
                columns = columns.Where(c => !c.Column.IsHidden);
            }

            foreach (var columnInfo in columns)
            {
                writer.WriteLine(columnInfo);
                writer.WriteLine("\t" + string.Join('\t', columnInfo.Values));
                writer.WriteLine();
            }

            writer.WriteLine(separatorLine);
        }
    }
}
