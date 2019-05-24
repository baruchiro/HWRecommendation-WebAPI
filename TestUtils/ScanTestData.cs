using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace TestUtils
{
    public class ScanTestData: IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { TestUtils.GenerateScan() };
            yield return new object[] { TestUtils.GenerateEmptyComponentsScan() };
            yield return new object[] { TestUtils.GenerateEmptyScan() };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
