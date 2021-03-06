using System;
using System.Configuration;
using System.Text;

using Assman.Configuration;
using Assman.TestObjects;

using NUnit.Framework;

namespace Assman
{
	[TestFixture]
	public class TestPluginElement
	{
		private PluginElement _element;

		[SetUp]
		public void Init()
		{
			_element = new PluginElement();
		}

		[Test]
		public void WhenTypeSpecified_ItIsNewedUpAndReturned()
		{
			_element.Type = typeof (StubPlugin).AssemblyQualifiedName;

			var finder = _element.CreatePlugin();

			Assert.That(finder, Is.InstanceOf<StubPlugin>());
		}

		[Test]
		[ExpectedException(typeof(ConfigurationErrorsException))]
		public void WhenTypeIsInvalid_ConfigurationErrorsExceptionThrown()
		{
			_element.Type = "bogus";

			_element.CreatePlugin();
		}

		[Test]
		[ExpectedException(typeof(ConfigurationErrorsException))]
		public void WhenTypeDoesNotImplementIResourceFinder_ConfigurationErrorsExceptionThrown()
		{
			_element.Type = typeof(StringBuilder).FullName;

			_element.CreatePlugin();
		}
	}

	
}