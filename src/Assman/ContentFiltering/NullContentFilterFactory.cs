namespace Assman.ContentFiltering
{
	public class NullContentFilterFactory : IContentFilterFactory
	{
		private static readonly IContentFilterFactory _instance = new NullContentFilterFactory();

		public static IContentFilterFactory Instance
		{
			get { return _instance; }
		}

		private NullContentFilterFactory() {}
		
		public IContentFilter CreateFilter(ResourceContentSettings settings)
		{
			return NullContentFilter.Instance;
		}
	}
}