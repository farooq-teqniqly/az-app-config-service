using System.Collections.Generic;

namespace WebAppWithKeyVault
{
    public class Header
    {
        public string Name { get; set; }
        public string Type { get; set; }

        public Context Context { get; set; }
    }

    public class Context
    {
        public string Tenant { get; set; }

        public IEnumerable<MetaDatum> Metadata { get; set; }
    }

    public class MetaDatum
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }
}
