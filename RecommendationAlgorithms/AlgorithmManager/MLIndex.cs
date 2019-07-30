using System.Collections.Generic;
using System.Linq;
using Microsoft.ML.Data;

namespace AlgorithmManager
{
    public class MLIndex
    {
        public static readonly IDictionary<DataKind, DataViewType> DataKindToDataViewType =
            new Dictionary<DataKind, DataViewType>
            {
                {DataKind.SByte, NumberDataViewType.SByte},
                {DataKind.Byte, NumberDataViewType.Byte},
                {DataKind.Int16, NumberDataViewType.Int16},
                {DataKind.UInt16, NumberDataViewType.UInt16},
                {DataKind.Int32, NumberDataViewType.Int32},
                {DataKind.UInt32, NumberDataViewType.UInt32},
                {DataKind.Int64, NumberDataViewType.Int64},
                {DataKind.UInt64, NumberDataViewType.UInt64},
                {DataKind.Single, NumberDataViewType.Single},
                {DataKind.Double, NumberDataViewType.Double},
                {DataKind.String, TextDataViewType.Instance},
                {DataKind.Boolean, BooleanDataViewType.Instance},
                {DataKind.TimeSpan, TimeSpanDataViewType.Instance},
                {DataKind.DateTime, DateTimeDataViewType.Instance},
                {DataKind.DateTimeOffset, DateTimeOffsetDataViewType.Instance},
            };

        public static readonly IDictionary<DataViewType, DataKind> DataViewTypeToDataKind =
            DataKindToDataViewType.ToDictionary(t => t.Value, t => t.Key);
        public static readonly DataKind[] _intTypes = {
            DataKind.Byte,
            DataKind.SByte,
            DataKind.Int16,
            DataKind.Int32,
            DataKind.Int64,
            DataKind.UInt16,
            DataKind.UInt32,
            DataKind.UInt64
        };
        public static readonly DataKind[] _doubleTypes =
        {
            DataKind.Double
        };
    }
}
