using System;

using AlmWitt.Web.ResourceManagement.Configuration;

namespace AlmWitt.Web.ResourceManagement
{
    /// <summary>
    /// Represents a specific type of web resource that can be consolidated and included on a web page.
    /// </summary>
	public abstract class ResourceType
    {
		private static readonly ResourceType _clientScript = new ClientScriptResourceType();
		private static readonly ResourceType _css = new CssResourceType();

		/// <summary>
		/// Gets the client script resource type (i.e. javascript).
		/// </summary>
		public static ResourceType ClientScript
    	{
    		get { return _clientScript; }
    	}

		/// <summary>
		/// Gets the css resource type.
		/// </summary>
		public static ResourceType Css
    	{
    		get { return _css; }
    	}

    	/// <summary>
    	/// Gets the file extension of this resource type.
    	/// </summary>
		public abstract string Extension { get; }
        
		/// <summary>
		/// Gets the consolidated url of the given resource path using the given configuration.
		/// </summary>
		/// <param name="config"></param>
		/// <param name="resourcePath"></param>
		/// <returns></returns>
		public abstract string GetResourceUrl(ResourceManagementConfiguration config, string resourcePath);
    	
		/// <summary>
		/// Gets an instance of a <see cref="IResourceIncluder"/> for this resource type.
		/// </summary>
		/// <param name="includerFactory"></param>
		/// <returns></returns>
		public abstract IResourceIncluder GetIncluder(IResourceIncluderFactory includerFactory);
    }

	internal class ClientScriptResourceType : ResourceType
	{
		public override string Extension
		{
			get { return ".js"; }
		}

		public override string GetResourceUrl(ResourceManagementConfiguration config, string resourcePath)
		{
			return config.GetScriptUrl(resourcePath);
		}

		public override IResourceIncluder GetIncluder(IResourceIncluderFactory includerFactory)
		{
			return includerFactory.CreateClientScriptIncluder();
		}
	}

	internal class CssResourceType : ResourceType
	{
		public override string Extension
		{
			get { return ".css"; }
		}

		public override string GetResourceUrl(ResourceManagementConfiguration config, string resourcePath)
		{
			return config.GetStylesheetUrl(resourcePath);
		}

		public override IResourceIncluder GetIncluder(IResourceIncluderFactory includerFactory)
		{
			return includerFactory.CreateCssIncluder();
		}
	}
}