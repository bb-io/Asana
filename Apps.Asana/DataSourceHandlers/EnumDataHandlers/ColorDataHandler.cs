using Blackbird.Applications.Sdk.Common.Dictionaries;

namespace Apps.Asana.DataSourceHandlers.EnumDataHandlers;

public class ColorDataHandler : IStaticDataSourceHandler
{
    public Dictionary<string, string> GetData()
        => new()
        {
            { "dark-pink", "Dark pink" },
            { "dark-green", "Dark green" },
            { "dark-blue", "Dark blue" },
            { "dark-red", "Dark red" },
            { "dark-teal", "Dark teal" },
            { "dark-brown", "Dark brown" },
            { "dark-orange", "Dark orange" },
            { "dark-purple", "Dark purple" },
            { "dark-warm-gray", "Dark warm gray" },
            { "light-pink", "Light pink" },
            { "light-green", "Light green" },
            { "light-blue", "Light blue" },
            { "light-red", "Light red" },
            { "light-teal", "Light teal" },
            { "light-brown", "Light brown" },
            { "light-orange", "Light orange" },
            { "light-purple", "Light purple" },
            { "light-warm-gray", "Light warm gray" },
            { "none", "None" }
        };
}