using System;
using System.IO;
using System.Reflection;

using Assman.TestSupport;

using NUnit.Framework;

namespace Assman
{
    [TestFixture]
    public class TestEmbeddedResource
    {
        private const string _embeddedNs = "Assman.EmbeddedResources";
        
        [Test]
        public void GetContentGetsWholeContent()
        {
            IResource resource = new EmbeddedResource(this.GetType().Assembly, _embeddedNs + ".Embedded1.txt");
            string actualContent = resource.GetContent();

            Assert.That(actualContent, Is.EqualTo("my text content"));
        }

        [Test]
        public void LastModifiedEqualsAssemblyDate()
        {
            Assembly assembly = this.GetType().Assembly;
            IResource resource = new EmbeddedResource(assembly, _embeddedNs + ".Embedded1.txt");

            Assert.That(resource.LastModified, Is.EqualTo(new FileInfo(ToLocationPath(assembly.CodeBase)).LastWriteTime));
        }

        [Test]
        public void VirtualPathEqualsAssemblyUri()
        {
            Assembly assembly = this.GetType().Assembly;
            IResource resource = new EmbeddedResource(assembly, _embeddedNs + ".Embedded1.txt");

            Assert.That(resource.VirtualPath, Is.EqualTo("assembly://" + assembly.GetName().Name + "/" + _embeddedNs + ".Embedded1.txt"));
        }

[Test]
		public void FileExtensionIsCorrectlyCalculated()
		{
			Assembly assembly = this.GetType().Assembly;
			IResource resource = new EmbeddedResource(assembly, _embeddedNs + ".Embedded1.txt");

			resource.FileExtension.ShouldEqual(".txt");
		}

        private string ToLocationPath(string fileUri)
        {
            string uri = fileUri.Substring(8); //strip off file:///
            return uri.Replace("/", @"\");
        }
    }
}
