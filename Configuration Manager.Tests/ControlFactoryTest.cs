using Configuration_Manager;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Configuration_Manager.CustomControls;

namespace Configuration_Manager.Tests
{
    
    
    /// <summary>
    ///This is a test class for ControlFactoryTest and is intended
    ///to contain all ControlFactoryTest Unit Tests
    ///</summary>
    [TestClass()]
    public class ControlFactoryTest
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
        ///A test for getInstance
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Configuration Manager.exe")]
        public void getInstanceTest()
        {
            ControlFactory_Accessor expected = null; // TODO: Initialize to an appropriate value
            ControlFactory_Accessor actual;
            actual = ControlFactory_Accessor.getInstance();
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for BuildCToolStripButton
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Configuration Manager.exe")]
        public void BuildCToolStripButtonTest()
        {
            ControlFactory_Accessor target = new ControlFactory_Accessor(); // TODO: Initialize to an appropriate value
            ControlDescription_Accessor cd = null; // TODO: Initialize to an appropriate value
            CToolStripButton_Accessor expected = null; // TODO: Initialize to an appropriate value
            CToolStripButton_Accessor actual;
            actual = target.BuildCToolStripButton(cd);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for BuildCTabPage
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Configuration Manager.exe")]
        public void BuildCTabPageTest()
        {
            ControlFactory_Accessor target = new ControlFactory_Accessor(); // TODO: Initialize to an appropriate value
            ControlDescription_Accessor cd = null; // TODO: Initialize to an appropriate value
            CTabPage_Accessor expected = null; // TODO: Initialize to an appropriate value
            CTabPage_Accessor actual;
            actual = target.BuildCTabPage(cd);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for BuildCLabel
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Configuration Manager.exe")]
        public void BuildCLabelTest()
        {
            ControlFactory_Accessor target = new ControlFactory_Accessor(); // TODO: Initialize to an appropriate value
            ControlDescription_Accessor cd = null; // TODO: Initialize to an appropriate value
            CLabel_Accessor expected = null; // TODO: Initialize to an appropriate value
            CLabel_Accessor actual;
            actual = target.BuildCLabel(cd);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }
    }
}
