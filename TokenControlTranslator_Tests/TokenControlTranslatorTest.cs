using Configuration_Manager.Util;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace TokenControlTranslator_Tests
{
    
    
    /// <summary>
    ///This is a test class for TokenControlTranslatorTest and is intended
    ///to contain all TokenControlTranslatorTest Unit Tests
    ///</summary>
    [TestClass()]
    public class TokenControlTranslatorTest
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
        ///A test for GetValueTranslatedPairs
        ///</summary>
        //[TestMethod()]
        //public void GetValueTranslatedPairsTest()
        //{
        //    TokenControlTranslator_Accessor target = new TokenControlTranslator_Accessor(); // TODO: Initialize to an appropriate value
        //    target.SetTokenKey("##");
        //    string textToTranslate = "This is just a test for ##CLabel1##, ##CLabel2##, ##CComboBox0##.";

        //    Dictionary<string, string> expected = new Dictionary<string, string>();
        //    expected.Add("##CLabel1##", "Sintonize with:");
        //    expected.Add("##CLabel2##", "Frequency:");
        //    expected.Add("##CComboBox0##", "##CLabel4##");

        //    Dictionary<string, string> actual;
        //    actual = target.GetValueTranslatedPairs(textToTranslate);
        //    Assert.AreEqual(expected, actual);
        //}

        /// <summary>
        ///A test for TranslateFromControl
        ///</summary>
        [TestMethod()]
        public void TranslateFromControlTest()
        {
            TokenControlTranslator_Accessor target = new TokenControlTranslator_Accessor(); // TODO: Initialize to an appropriate value

            target.SetTokenKey("##");
            string textToTranslate = "This is just a test for ##CLabel1##, ##CLabel2##, ##CComboBox0##.";
            string expected = "This is just a test for Sintonize with:, Frequency:, ##CLabel4##.";
            string actual;
            actual = target.TranslateFromControl(textToTranslate);
            Assert.AreEqual(expected, actual);
        }
    }
}
