using Configuration_Manager.Util;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using Configuration_Manager;

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
        private Model m = Model.getInstance();

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
        [ClassInitialize()]
        public static void MyClassInitialize(TestContext testContext)
        {
            TokenControlTranslator_Accessor.tokenKey = "##";
        }
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

        /// <summary>
        ///A test for TranslateFromControl
        ///</summary>
        [TestMethod()]
        public void TranslateFromControlTest()
        {
            string textToTranslate = "This is just a test for ##CLabel1##, ##CLabel2##, ##CComboBox0##.";
            string expected = "This is just a test for Sintonize with:, Frequency:, ##CLabel4##.";
            string actual;
            actual = TokenControlTranslator_Accessor.TranslateFromControl(textToTranslate);
            Assert.AreEqual(expected, actual);
        }

        public void BuildModel()
        {
            System.Windows.Forms.Form f = new System.Windows.Forms.Form();

            m.AllControls.Add(ControlFactory.BuildCLabel(f));
            m.AllControls.Add(ControlFactory.BuildCComboBox(f));
            m.AllControls.Add(ControlFactory.BuildCLabel(f));
            m.AllControls.Add(ControlFactory.BuildCComboBox(f));
            m.AllControls.Add(ControlFactory.BuildCLabel(f));
            m.AllControls.Add(ControlFactory.BuildCComboBox(f));
        }
    }
}
