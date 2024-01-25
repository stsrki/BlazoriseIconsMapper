using Blazorise.Icons.FontAwesome;
using Microsoft.AspNetCore.Components;
using YamlDotNet.Serialization.NamingConventions;
using YamlDotNet.Serialization;
using BlazoriseIconsMapper.Models;
using System.Text.Json;

namespace BlazoriseIconsMapper.Components.Pages;

public partial class Home
{
    enum DownloadStatus
    {
        None,
        Downloading,
        Reading,
        Downloaded,
        Error,
    }

    private DownloadStatus FontAwesomeDownloadStatus = DownloadStatus.None;
    private DownloadStatus FabricCoreDownloadStatus = DownloadStatus.None;

    Dictionary<string, FontAwesomeIcon> FontAwesomeIcons;
    List<FabricCoreIcon> FabricCoreIcons;
    List<BlazoriseIcon> BlazoriseIcons;

    async Task DownloadFontAwesome()
    {
        try
        {
            FontAwesomeDownloadStatus = DownloadStatus.Downloading;

            var file = await HttpClient.GetStringAsync( "https://raw.githubusercontent.com/FortAwesome/Font-Awesome/5.x/js-packages/%40fortawesome/fontawesome-free/metadata/icons.yml" );
            //var file = await HttpClient.GetStringAsync( "/assets/fontawesome/icons.yml" );

            var deserializer = new DeserializerBuilder()
                .WithNamingConvention( CamelCaseNamingConvention.Instance )  // see height_in_inches in sample yml 
                .IgnoreUnmatchedProperties()
                .Build();

            FontAwesomeDownloadStatus = DownloadStatus.Reading;

            FontAwesomeIcons = deserializer.Deserialize<Dictionary<string, FontAwesomeIcon>>( file );

            FontAwesomeDownloadStatus = DownloadStatus.Downloaded;
        }
        catch ( Exception exc )
        {
            FontAwesomeDownloadStatus = DownloadStatus.Error;
        }
    }

    async Task DownloadFabricCore()
    {
        try
        {
            FabricCoreDownloadStatus = DownloadStatus.Downloading;

            var file = await HttpClient.GetStringAsync( "https://raw.githubusercontent.com/OfficeDev/office-ui-fabric-core/master/src/data/icons.json" );

            FabricCoreDownloadStatus = DownloadStatus.Reading;

            FabricCoreIcons = JsonSerializer.Deserialize<List<FabricCoreIcon>>( file, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            } );

            FabricCoreDownloadStatus = DownloadStatus.Downloaded;
        }
        catch ( Exception exc )
        {
            FabricCoreDownloadStatus = DownloadStatus.Error;
        }
    }

    async Task MapIcons()
    {
        try
        {
            BlazoriseIcons = new List<BlazoriseIcon>();

            BlazoriseIcons.AddRange( from fa in FontAwesomeIcons
                                     from fc in FabricCoreIcons
                                     where string.Compare( fa.Key, fc.Name, StringComparison.OrdinalIgnoreCase ) == 0
                                     select new BlazoriseIcon
                                     {
                                         Name = fa.Key
                                     } );

        }
        catch ( Exception exc )
        {
        }
    }

    [Inject] public HttpClient HttpClient { get; set; }
}
