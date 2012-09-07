using Configuration_Manager.Util;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace TokenTextTranslator_Tests
{
    
    
    /// <summary>
    ///This is a test class for TokenTextTranslatorTest and is intended
    ///to contain all TokenTextTranslatorTest Unit Tests
    ///</summary>
	[TestClass()]
	public class TokenTextTranslatorTest
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
		[ClassInitialize()]
		public static void MyClassInitialize(TestContext testContext)
		{
            TokenTextTranslator.SetTokenTextTranslator("@@");
            TokenTextTranslator_Accessor.currentLang = @"C:\Projects\Configuration Manager\Configuration Manager\bin\texts\translation_EN.xml";
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
		///A test for CheckTextFormatting
		///</summary>
		[TestMethod()]
		public void ValidateEvenKeys_NullString_ReturnsFalse()
		{
            TokenTextTranslator.SetTokenTextTranslator("@@");
            TokenTextTranslator_Accessor.textToTranslate = null;
            Assert.AreEqual(false, TokenTextTranslator_Accessor.ValidateToken(""));
		}

		[TestMethod()]
		public void ValidateToken_EmptyToken_ReturnsFalse()
		{
			Assert.AreEqual(false, TokenTextTranslator_Accessor.ValidateToken("@@@@"));
		}

		[TestMethod()]
		public void ValidateToken_NullToken_ReturnsFalse()
		{
            Assert.AreEqual(false, TokenTextTranslator_Accessor.ValidateToken(null));
		}

		[TestMethod()]
		public void ValidateToken_ValidToken_ReturnsTrue()
		{
            Assert.AreEqual(true, TokenTextTranslator_Accessor.ValidateToken("@@100@@"));
		}

		[TestMethod()]
		public void ValidateToken_InvalidToken1_ReturnsFalse()
		{
            Assert.AreEqual(false, TokenTextTranslator_Accessor.ValidateToken("@@@1@"));
		}

		[TestMethod()]
		public void ValidateToken_InvalidToken2_ReturnsFalse()
		{
            Assert.AreEqual(false, TokenTextTranslator_Accessor.ValidateToken("@1@"));
		}

		[TestMethod()]
		public void ValidateToken_InvalidToken3_ReturnsFalse()
		{
            Assert.AreEqual(false, TokenTextTranslator_Accessor.ValidateToken("1@"));
		}

		[TestMethod()]
		public void SetSubPath_EmptyPath_ReturnFalse()
		{
            Assert.AreEqual(false, TokenTextTranslator_Accessor.SetSubPath(""));
		}

		[TestMethod()]
		public void SetSubPath_NullPath_ReturnFalse()
		{
            Assert.AreEqual(false, TokenTextTranslator_Accessor.SetSubPath(null));
		}

		[TestMethod()]
		public void SetSubPath_CorrectPath_ReturnTrue()
		{
            Assert.AreEqual(true, TokenTextTranslator_Accessor.SetSubPath("\\Languages\\Language\\Text"));
		}

		[TestMethod()]
		public void SetSubPath_WeirdString_ReturnTrue()
		{
            Assert.AreEqual(true, TokenTextTranslator_Accessor.SetSubPath("\\\\\\Languages\\Language\\Text\\\\"));
		}

		[TestMethod()]
		[ExpectedException(typeof(System.IO.FileNotFoundException))]
		public void GetTranslatedValues_LangFileNotExists_ThrowsException()
		{
            TokenTextTranslator_Accessor.currentLang = @"C:\idontexist.xml";
            TokenTextTranslator_Accessor.TranslateFromTextFile(null);
		}

        //[TestMethod()]
		public void GetTranslatedValues_EnglishFile_TranslatesOK()
		{
            TokenTextTranslator_Accessor.valuesToTranslate.Add("0");
            TokenTextTranslator_Accessor.valuesToTranslate.Add("1");
            TokenTextTranslator_Accessor.valuesToTranslate.Add("2");
            TokenTextTranslator_Accessor.valuesToTranslate.Add("3");

            TokenTextTranslator_Accessor.GetTranslatedValues();
			
			System.Collections.Generic.List<String> list = new System.Collections.Generic.List<String>();
			list.Add("Rolls");
			list.Add("Coins");
			list.Add("Notes");
			list.Add("");

            Assert.AreEqual(TokenTextTranslator_Accessor.translatedValues[0], list[0]);
            Assert.AreEqual(TokenTextTranslator_Accessor.translatedValues[1], list[1]);
            Assert.AreEqual(TokenTextTranslator_Accessor.translatedValues[2], list[2]);
            Assert.AreEqual(TokenTextTranslator_Accessor.translatedValues[3], list[3]);
		}

		[TestMethod()]
		public void Translate_StringWithoutValues_ReturnsSameString()
		{
            Assert.AreEqual("Hola?", TokenTextTranslator_Accessor.TranslateFromTextFile("Hola?"));
		}

		[TestMethod()]
		public void Translate_NullString_ReturnsEmptyString()
		{
            Assert.AreEqual("", TokenTextTranslator_Accessor.TranslateFromTextFile(null));
		}

		[TestMethod()]
		public void Translate_EmptyString_ReturnsEmptyString()
		{
            Assert.AreEqual("", TokenTextTranslator_Accessor.TranslateFromTextFile(""));
		}

		[TestMethod()]
		public void Translate_UnevenTokenString_ReturnsSameString()
		{
            Assert.AreEqual("@1@@ Hola", TokenTextTranslator_Accessor.TranslateFromTextFile("@1@@ Hola"));
		}

		[TestMethod()]
		public void Translate_CorrectedString_ReturnsValues()
		{
            Assert.AreEqual("Editor: Control, Parent", TokenTextTranslator_Accessor.TranslateFromTextFile("@@1@@: @@2@@, @@3@@"));
		}

		[TestMethod()]
		public void TranslateFormTextFile_StringWithoutTokens_ReturnsSameString()
		{
            Assert.AreEqual("I don't have tokens!", TokenTextTranslator_Accessor.TranslateFromTextFile("I don't have tokens!"));
		}
	}
}
