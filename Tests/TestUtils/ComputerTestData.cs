using System.Collections;
using System.Collections.Generic;

namespace TestUtils
{
    public class ComputerTestData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { TestUtils.GenerateComputer() };
            yield return new object[] { TestUtils.GenerateEmptyComponentsComputer() };
            yield return new object[] { TestUtils.GenerateEmptyComputer() };
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
