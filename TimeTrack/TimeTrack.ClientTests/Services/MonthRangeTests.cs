using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TimeTrack.Client.Services.Tests
{
    [TestClass()]
    public class MonthRangeTests
    {
        private static MonthRange SeptemberFifth()
        {
            return new MonthRange(new DateTime(2022, 9, 5));
        }
        [TestMethod()]
        public void CurrentTest()
        {
            var range = SeptemberFifth();
            Assert.AreEqual(new DateTime(2022, 8, 27), range.Current);
        }

        [TestMethod()]
        public void MoveNextTest()
        {
            var range = SeptemberFifth();
            while (range.MoveNext()) { }
            Assert.AreEqual(new DateTime(2022, 10, 1), range.Current);
        }

        [TestMethod()]
        public void ResetTest()
        {
            var range = SeptemberFifth();
            var start = range.Current;
            range.MoveNext();
            range.MoveNext();
            Assert.AreNotEqual(start, range.Current);
            range.Reset();
            Assert.AreEqual(start, range.Current);
        }

        [TestMethod()]
        public void ForeachTest()
        {
            var range = SeptemberFifth();
            var count = 0;
            foreach(var day in range)
            {
                count++;
            }
            Assert.AreEqual(35, count);
        }
    }
}