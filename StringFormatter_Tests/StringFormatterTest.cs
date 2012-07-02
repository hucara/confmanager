﻿using Configuration_Manager.Util;
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
        ///
        [TestMethod()]
        public void GetFormattedText_TokenMiddle_ReturnsExpected()
        {
            string text = "Those; fucking; wildcards";
            string format = "* ##this##; *";
            string actual = StringFormatter.GetFormattedText(text, format);
            Assert.AreEqual("fucking", actual);
        }

        [TestMethod()]
        public void GetFormattedText_TokenBeginning_ReturnsExpected()
        {
            string text = "RD: CD: HD:";
            string format = "##this##:*";
            string actual = StringFormatter.GetFormattedText(text, format);
            Assert.AreEqual("RD", actual);
        }

        [TestMethod()]
        public void GetFormattedText_TokenEnd_ReturnsExpected()
        {
            string text = "RD: CD: HD:";
            string format = "*:##this##";
            string actual = StringFormatter.GetFormattedText(text, format);
            Assert.AreEqual(" CD: HD:", actual);
        }

        [TestMethod()]
        public void GetFormattedText_TokenEnd_ReturnsExpected2()
        {
            string text = "RD :CD: HD";
            string format = "* :##this##";
            string actual = StringFormatter.GetFormattedText(text, format);
            Assert.AreEqual("CD: HD", actual);
        }

        [TestMethod()]
        public void GetFormattedText_NoWildCardsTokenEnd_ReturnsExpected()
        {
            string text = "RD :CD: HD";
            string format = "RD :##this##";
            string actual = StringFormatter.GetFormattedText(text, format);
            Assert.AreEqual("CD: HD", actual);
        }

        [TestMethod()]
        public void GetFormattedText_NoWildCardsTokenBegin_ReturnsExpected()
        {
            string text = "RD :CD: HD";
            string format = "##this## HD";
            string actual = StringFormatter.GetFormattedText(text, format);
            Assert.AreEqual("RD :CD:", actual);
        }

        [TestMethod()]
        public void GetFormattedText_NoWildCardsTokenMiddle_ReturnsExpected()
        {
            string text = "RD :CD: HD";
            string format = "RD ##this## HD";
            string actual = StringFormatter.GetFormattedText(text, format);
            Assert.AreEqual(":CD:", actual);
        }

        [TestMethod()]
        public void GetFormattedText_WildCardsQuestionEnd_ReturnsExpected()
        {
            string text = "RD : CD : HD";
            string format = "##this##???????";
            string actual = StringFormatter.GetFormattedText(text, format);
            Assert.AreEqual("RD : ", actual);
        }

        [TestMethod()]
        public void GetFormattedText_WildCardsQuestionBeginning_ReturnsExpected()
        {
            string text = "RD : CD : HD";
            string format = "???##this##";
            string actual = StringFormatter.GetFormattedText(text, format);
            Assert.AreEqual(": CD : HD", actual);
        }

        [TestMethod()]
        public void GetFormattedText_WildCardsQuestionMiddle_ReturnsExpected()
        {
            string text = "RD : CD : HD";
            string format = "???##this##???";
            string actual = StringFormatter.GetFormattedText(text, format);
            Assert.AreEqual(": CD :", actual);
        }

        [TestMethod()]
        public void GetFormattedText_WildCardsMixed_ReturnsExpected()
        {
            string text = "RD : CD : HD";
            string format = "???##this## H*";
            string actual = StringFormatter.GetFormattedText(text, format);
            Assert.AreEqual(": CD :", actual);
        }

        [TestMethod()]
        public void GetFormattedText_WildCardsMixed_ReturnsExpected2()
        {
            string text = "RD : CD : HD";
            string format = "*D ##this##???";
            string actual = StringFormatter.GetFormattedText(text, format);
            Assert.AreEqual(": CD :", actual);
        }

        [TestMethod()]
        public void FinalTests_CombineWildcards_ReturnsExpected1()
        {
            string text = "Hello my friend";
            string format = "?* ##this## *";
            string actual = StringFormatter.GetFormattedText(text, format);
            Assert.AreEqual("my", actual);
        }

        [TestMethod()]
        public void FinalTests_CombineWildcards_ReturnsExpected2()
        {
            string text = "Hello my friend";
            string format = "##this##y ???*";
            string actual = StringFormatter.GetFormattedText(text, format);
            Assert.AreEqual("Hello m", actual);
        }

        [TestMethod()]
        public void FinalTests_CombineWildcards_ReturnsExpected3()
        {
            string text = "Hello my friend";
            string format = "??????* ##this##";
            string actual = StringFormatter.GetFormattedText(text, format);
            Assert.AreEqual("friend", actual);
        }

        [TestMethod()]
        public void GetUnformattedText_CombineWildcards_ReturnsExpected()
        {
            string text = "Hello my friend";
            string format = "??????##this## f*";
            string newValue = "your";
            string actual = StringFormatter.GetUnFormattedText(newValue, text, format);
            Assert.AreEqual("Hello your friend", actual);
        }

        [TestMethod()]
        public void GetUnformattedText_CombineWildcards_ReturnsExpected2()
        {
            string text = "Hello my friend";
            string format = "##this## f*";
            string newValue = "Bye my";
            string actual = StringFormatter.GetUnFormattedText(newValue, text, format);
            Assert.AreEqual("Bye my friend", actual);
        }

        [TestMethod()]
        public void GetUnformattedText_CombineWildcards_ReturnsExpected3()
        {
            string text = "Hello my friend";
            string format = "##this##";
            string newValue = "Cómo estás amigo";
            string actual = StringFormatter.GetUnFormattedText(newValue, text, format);
            Assert.AreEqual("Cómo estás amigo", actual);
        }

        [TestMethod()]
        public void GetUnformattedText_CombineWildcards_ReturnsExpected4()
        {
            string text = "Hello my friend";
            string format = "##this## my ??????";
            string newValue = "Qué pasa";
            string actual = StringFormatter.GetUnFormattedText(newValue, text, format);
            Assert.AreEqual("Qué pasa my friend", actual);
        }
    }
}
