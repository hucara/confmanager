// <copyright file="TokenTextTranslatorTest.cs" company="Microsoft">Copyright © Microsoft 2012</copyright>
using System;
using Configuration_Manager.Util;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Configuration_Manager.Util
{
    /// <summary>This class contains parameterized unit tests for TokenTextTranslator</summary>
    [PexClass(typeof(TokenTextTranslator))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
    [TestClass]
    public partial class TokenTextTranslatorTest
    {
    }
}
