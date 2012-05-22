using Configuration_Manager;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Configuration_Manager.CustomControls;
using System.Windows.Forms;

namespace ComboBoxEditor_Tests
{
    
    
    /// <summary>
    ///This is a test class for ComboBoxEditorTest and is intended
    ///to contain all ComboBoxEditorTest Unit Tests
    ///</summary>
    [TestClass()]
    public class ComboBoxEditorTest
    {
        private TestContext testContextInstance;
        static CComboBox_Accessor cb = ControlFactory_Accessor.BuildCComboBox(new Form());
        static ComboBoxEditor_Accessor target = new ComboBoxEditor_Accessor(cb as ICustomControl);

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
        ///A test for MoveDownItem
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Configuration Manager.exe")]
        public void MoveDownItemTest()
        {
            PrivateObject param0 = null; // TODO: Initialize to an appropriate value
            ComboBoxEditor_Accessor target = new ComboBoxEditor_Accessor(param0); // TODO: Initialize to an appropriate value
            target.MoveDownItem();
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for MoveUpItem
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Configuration Manager.exe")]
        public void MoveUpItemTest()
        {
            fillOutComboBox();
            target.MoveUpItem();
            Assert.AreEqual("items4", cb.cd.comboBoxItems[2]);
        }

        [TestMethod()]
        [DeploymentItem("Configuration Manager.exe")]
        public void MoveUpItemTest2()
        {
            fillOutComboBox();
            target.MoveUpItem();
            Assert.AreEqual("items1", cb.cd.comboBoxItems[0]);
        }

        private void fillOutComboBox()
        {
            cb.cd.comboBoxConfigItems.Add("config1");
            cb.cd.comboBoxConfigItems.Add("config2");
            cb.cd.comboBoxConfigItems.Add("config3");
            cb.cd.comboBoxConfigItems.Add("config4");
            cb.cd.comboBoxRealItems.Add("real1");
            cb.cd.comboBoxRealItems.Add("real2");
            cb.cd.comboBoxRealItems.Add("real3");
            cb.cd.comboBoxRealItems.Add("real4");
            cb.cd.comboBoxItems.Add("items1");
            cb.cd.comboBoxItems.Add("items2");
            cb.cd.comboBoxItems.Add("items3");
            cb.cd.comboBoxItems.Add("items4");     
        }
    }
}
