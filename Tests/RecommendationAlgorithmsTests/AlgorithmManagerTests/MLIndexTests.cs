using AlgorithmManager;
using EnumsNET;
using Microsoft.ML.Data;
using System.Linq;
using Xunit;

namespace AlgorithmManagerTests
{
    public class MLIndexTests
    {
        [Fact]
        public void DataKindToDataViewType_AllDataKind_validateReturn()
        {
            var s = string.Join('\n', Enums.GetMembers<DataKind>().Select(e => $"{{ DataKind.{e.Value}, }},"));
            foreach (var dataKind in Enums.GetMembers<DataKind>().Select(e => e.Value))
            {
                Assert.True(MLIndex.DataKindToDataViewType.ContainsKey(dataKind), $"Dictionary not contains {dataKind}");
            }
        }
    }
}
