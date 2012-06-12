using Configuration_Manager.Util;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace StringFormatter_Tests
{
    
    
    /// <summary>
    ///This is a test class for StringFormatterTest and is intended
    ///to contain all StringFormatterTest Unit Tests
    ///</summary>
    [TestClass()]
    public class StringFormatterTest
    {
        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion

        /// <summary>
        ///A test for FormatText
        ///</summary>
        [TestMethod()]
        public void FormatText_SimpleString_ReturnsExpected()
        {
            string text = "Hola, Qué tal estamos, Adiós!";
            string format = "Hola, ##This##, Adiós!";
            string expected = "Qué tal estamos";
            string actual;
            actual = StringFormatter.FormatText(text, format);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void FormatText_SimpleString_ReturnsExpected2()
        {
            string text = "RD, CD, OH, YAY";
            string format = "RD, ##This##, OH, YAY";
            string expected = "CD";
            string actual;
            actual = StringFormatter.FormatText(text, format);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void FormatText_EndToken_ReturnsExpected()
        {
            string text = "Uno; Dos44; tres...Cuatro;Cinco";
            string format = "Uno; Dos44; ##This##";
            string expected = "tres...Cuatro;Cinco";
            string actual;
            actual = StringFormatter.FormatText(text, format);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void FormatText_BegginingToken_ReturnsExpected()
        {
            string text = "Uno; Dos44; tres...Cuatro;Cinco";
            string format = "##This##; tres...Cuatro;Cinco";
            string expected = "Uno; Dos44";
            string actual;
            actual = StringFormatter.FormatText(text, format);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void FormatText_NoToken_ReturnsSameText()
        {
            string text = "Uno; Dos44; tres...Cuatro;Cinco";
            string format = "tres...Cuatro;Cinco";
            string expected = "Uno; Dos44; tres...Cuatro;Cinco";
            string actual;
            actual = StringFormatter.FormatText(text, format);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void FormatText_WildCards_ReturnsNoIdea()
        {
            string text = "Hola, Qué tal, Adiós";
            string format = "*, ##This##, *";
            string expected = "Qué tal";
            string actual = StringFormatter.FormatText(text, format);
            Assert.AreEqual(expected, actual);
        }

        //[TestMethod()]
        //public void WildcardsToRegex_NormalPattern_ExpectedResult()
        //{
        //    string pattern = "Esta noche vamos a tope.";
        //    string actual = StringFormatter.WildcardsToRegex(pattern);
        //    Assert.AreEqual("Esta noche vamos a tope.", actual);
        //}
    }
}
