using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Fu.Infrastructure.Data;
using Fu.Module.Localization.Models;

namespace Fu.Module.Localization
{
    public class EfStringLocalizerFactory : IStringLocalizerFactory
    {
        private readonly IRepository<Translate> _resourceRepository;
        private IList<ResourceString> _resourceStrings;

        public EfStringLocalizerFactory(IRepository<Translate> resourceRepository)
        {
            _resourceRepository = resourceRepository;
            LoadResources();
        }

        public IStringLocalizer Create(Type resourceSource)
        {
            return new EfStringLocalizer(_resourceStrings);
        }

        public IStringLocalizer Create(string baseName, string location)
        {
            return new EfStringLocalizer(_resourceStrings);
        }

        private void LoadResources()
        {
            _resourceStrings = _resourceRepository.Query().Include(x => x.Culture).Select(x => new ResourceString
            {
                Culture = x.Culture.Name,
                Key = x.Key,
                Value = x.Value
            }).ToList();
        }
    }
}
