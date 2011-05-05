using System;
using System.IO;
using System.Web;
using System.Web.Configuration;

using Assman.Configuration;

namespace Assman
{
    public class ResourceHttpHandler : HttpHandlerBase
    {
        private readonly IHttpHandler _staticHandler;
        private readonly HttpHandlerBase _dynamicHandler;
        private readonly IThreadSafeInMemoryCache<string, bool> _fileExistenceCache;

        public ResourceHttpHandler() : this(CreateStaticFileHandler(), new DynamicResourceHttpHandler(), CreateFileExistenceCache()) {}
        
        internal ResourceHttpHandler(IHttpHandler staticHandler, HttpHandlerBase dynamicHandler, IThreadSafeInMemoryCache<string,bool> fileExistenceCache)
        {
            _staticHandler = staticHandler;
            _dynamicHandler = dynamicHandler;
            _fileExistenceCache = fileExistenceCache;
        }

        public override void ProcessRequest(HttpContextBase context)
        {
            if(FileExists(context.Request.PhysicalPath))
            {
                _staticHandler.ProcessRequest(context.ToHttpContext());
            }
            else
            {
                _dynamicHandler.ProcessRequest(context);
            }
        }

        private bool FileExists(string physicalPath)
        {
            return _fileExistenceCache.GetOrAdd(physicalPath, () => File.Exists(physicalPath));
        }

        private static IThreadSafeInMemoryCache<string,bool> CreateFileExistenceCache()
        {
            var configLoader = new DefaultConfigLoader();
            var compilationSection = configLoader.GetSection<CompilationSection>("system.web/compilation");

            if (compilationSection != null && compilationSection.Debug)
                return new NullThreadSafeInMemoryCache<string, bool>();
            else
                return new ThreadSafeInMemoryCache<string, bool>(Comparers.FileSystemPath);
        }

        private static IHttpHandler CreateStaticFileHandler()
        {
            var webAssembly = typeof (IHttpHandler).Assembly;
            var staticHandlerType = webAssembly.GetType("System.Web.StaticFileHandler", true);
            return (IHttpHandler) Activator.CreateInstance(staticHandlerType, nonPublic:true);
        }
    }
}