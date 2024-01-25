namespace BlazoriseIconsMapper.Models;

public class FontAwesomeIcon
{
    public List<string> Changes { get; set; }
    public string Label { get; set; }
    public FontAwesomeIconSearch Search { get; set; }
    public List<string> Styles { get; set; }
    public string Unicode { get; set; }
    public bool Voted { get; set; }
}

public class FontAwesomeIconSearch
{
    public List<string> Terms { get; set; }
}