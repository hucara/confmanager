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
		///A test for CheckTextFormatting
		///</summary>
		[TestMethod()]
		public void ValidateEvenKeys_NullString_ReturnsFalse()
		{
			TokenTextTranslator_Accessor target = new TokenTextTranslator_Accessor("@@");
			target.textToTranslate = null;
			Assert.AreEqual(false, target.ValidateToken(""));
		}

		[TestMethod()]
		public void ValidateToken_EmptyToken_ReturnsFalse()
		{
			TokenTextTranslator_Accessor target = new TokenTextTranslator_Accessor("@@");
			Assert.AreEqual(false, target.ValidateToken("@@@@"));
		}

		[TestMethod()]
		public void ValidateToken_NullToken_ReturnsFalse()
		{
			TokenTextTranslator_Accessor target = new TokenTextTranslator_Accessor("@@");
			Assert.AreEqual(false, target.ValidateToken(null));
		}

		[TestMethod()]
		public void ValidateToken_ValidToken_ReturnsTrue()
		{
			TokenTextTranslator_Accessor target = new TokenTextTranslator_Accessor("@@");
			Assert.AreEqual(true, target.ValidateToken("@@100@@"));
		}

		[TestMethod()]
		public void ValidateToken_InvalidToken1_ReturnsFalse()
		{
			TokenTextTranslator_Accessor target = new TokenTextTranslator_Accessor("@@");
			Assert.AreEqual(false, target.ValidateToken("@@@1@"));
		}

		[TestMethod()]
		public void ValidateToken_InvalidToken2_ReturnsFalse()
		{
			TokenTextTranslator_Accessor target = new TokenTextTranslator_Accessor("@@");
			Assert.AreEqual(false, target.ValidateToken("@1@"));
		}

		[TestMethod()]
		public void ValidateToken_InvalidToken3_ReturnsFalse()
		{
			TokenTextTranslator_Accessor target = new TokenTextTranslator_Accessor("@@");
			Assert.AreEqual(false, target.ValidateToken("1@"));
		}

		[TestMethod()]
		public void SetSubPath_EmptyPath_ReturnFalse()
		{
			TokenTextTranslator_Accessor target = new TokenTextTranslator_Accessor("@@");
			Assert.AreEqual(false, target.SetSubPath(""));
		}

		[TestMethod()]
		public void SetSubPath_NullPath_ReturnFalse()
		{
			TokenTextTranslator_Accessor target = new TokenTextTranslator_Accessor("@@");
			Assert.AreEqual(false, target.SetSubPath(null));
		}

		[TestMethod()]
		public void SetSubPath_CorrectPath_ReturnTrue()
		{
			TokenTextTranslator_Accessor target = new TokenTextTranslator_Accessor("@@");
			Assert.AreEqual(true, target.SetSubPath("\\Languages\\Language\\Text"));
		}

		[TestMethod()]
		public void SetSubPath_WeirdString_ReturnTrue()
		{
			TokenTextTranslator_Accessor target = new TokenTextTranslator_Accessor("@@");
			Assert.AreEqual(true, target.SetSubPath("\\\\\\Languages\\Language\\Text\\\\"));
		}

		[TestMethod()]
		[ExpectedException(typeof(System.IO.FileNotFoundException))]
		public void GetTranslatedValues_LangFileNotExists_ThrowsException()
		{
			TokenTextTranslator_Accessor target = new TokenTextTranslator_Accessor("@@");
			target.currentLang = @"C:\idontexist.xml";
			target.TranslateFromTextFile(null);
		}

		[TestMethod()]
		public void GetTranslatedValues_EnglishFile_TranslatesOK()
		{
			TokenTextTranslator_Accessor target = new TokenTextTranslator_Accessor("@@");
			target.currentLang = @"C:\Projects\Configuration Manager\Configuration Manager\bin\texts\TextFile_EN.xml";

			target.valuesToTranslate.Add("0");
			target.valuesToTranslate.Add("1");
			target.valuesToTranslate.Add("2");
			target.valuesToTranslate.Add("3");

			target.GetTranslatedValues();
			
			System.Collections.Generic.List<String> list = new System.Collections.Generic.List<String>();
			list.Add("Rolls");
			list.Add("Coins");
			list.Add("Notes");
			list.Add("");

			Assert.AreEqual(target.translatedValues[0], list[0]);
			Assert.AreEqual(target.translatedValues[1], list[1]);
			Assert.AreEqual(target.translatedValues[2], list[2]);
			Assert.AreEqual(target.translatedValues[3], list[3]);
		}

		[TestMethod()]
		public void Translate_StringWithoutValues_ReturnsSameString()
		{
			TokenTextTranslator_Accessor target = new TokenTextTranslator_Accessor("@@");
			target.currentLang = @"C:\Projects\Configuration Manager\Configuration Manager\bin\texts\TextFile_EN.xml";
			Assert.AreEqual("Hola?", target.TranslateFromTextFile("Hola?"));
		}

		[TestMethod()]
		public void Translate_NullString_ReturnsEmptyString()
		{
			TokenTextTranslator_Accessor target = new TokenTextTranslator_Accessor("@@");
			target.currentLang = @"C:\Projects\Configuration Manager\Configuration Manager\bin\texts\TextFile_EN.xml";
			Assert.AreEqual("", target.TranslateFromTextFile(null));
		}

		[TestMethod()]
		public void Translate_EmptyString_ReturnsEmptyString()
		{
			TokenTextTranslator_Accessor target = new TokenTextTranslator_Accessor("@@");
			target.currentLang = @"C:\Projects\Configuration Manager\Configuration Manager\bin\texts\TextFile_EN.xml";
			Assert.AreEqual("", target.TranslateFromTextFile(""));
		}

		[TestMethod()]
		public void Translate_UnevenTokenString_ReturnsEmptyString()
		{
			TokenTextTranslator_Accessor target = new TokenTextTranslator_Accessor("@@");
			target.currentLang = @"C:\Projects\Configuration Manager\Configuration Manager\bin\texts\TextFile_EN.xml";
			Assert.AreEqual("*ERROR*", target.TranslateFromTextFile("@1@@ Hola"));
		}

		[TestMethod()]
		public void Translate_CorrectedString_ReturnsValues()
		{
			TokenTextTranslator_Accessor target = new TokenTextTranslator_Accessor("@@");
			target.currentLang = @"C:\Projects\Configuration Manager\Configuration Manager\bin\texts\TextFile_EN.xml";
			Assert.AreEqual("Warning: standby, Hardw. error", target.TranslateFromTextFile("@@256@@: @@312@@, @@265@@"));
		}
	}
}
