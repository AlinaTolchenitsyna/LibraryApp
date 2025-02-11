using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using LibraryApp;

namespace LibraryAppTests
{
    [TestClass]
    public class IsReaderCorrect
    {
        [TestMethod]
        public void TestIsReaderCorrect_CheckingReader_trueReturned()
        {
            LibraryApp.DB.Reader reader = new LibraryApp.DB.Reader();
            reader.LastName = "Иванов";
            reader.FirstName = "Алексей";
            reader.Patronymic = "Сергеевич";
            reader.Birthday = new DateTime(2011, 5, 12);
            reader.Phone = "89101234567";
            bool actual = LibraryApp.BL.TReader.IsReaderCorrect(reader);
            bool expected = true;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void NegTestIsReaderCorrect_CheckingReader_falseReturned()
        {
            LibraryApp.DB.Reader reader = new LibraryApp.DB.Reader();
            reader.LastName = "Иванов";
            reader.FirstName = "Алексей";
            reader.Patronymic = "Сергеевич";
            reader.Birthday = new DateTime(2000, 5, 12);
            reader.Phone = "89101234567";
            bool actual = LibraryApp.BL.TReader.IsReaderCorrect(reader);
            bool expected = false;
            Assert.AreEqual(expected, actual);
        }
    }
}
