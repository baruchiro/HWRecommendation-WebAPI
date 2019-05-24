using System;
using System.Collections;
using System.Collections.Generic;
using HW.Bot.Model;
using HWWebApi.Models;
using Models;

namespace HWWebApi.UnitTest.Utils
{
    public class ScanTestData: IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { GenerateScan() };
            yield return new object[] { GenerateEmptyComponentsScan() };
            yield return new object[] { GenerateEmptyScan() };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public static Scan GenerateScan()
        {
            return new Scan
            {
                Computer = TestUtils.TestUtils.GenerateComputer(),
                CreationDateTime = DateTime.Now,
                User = new User { Age = 25, Gender = Gender.MALE, Name = "User Name", WorkArea = "My work" }
            };
        }

        public static Scan GenerateEmptyComponentsScan()
        {
            return new Scan
            {
                Computer = new Computer(),
                CreationDateTime = new DateTime(),
                User = new User()
            };
        }

        public static Scan GenerateEmptyScan()
        {
            return new Scan();
        }
    }
}
