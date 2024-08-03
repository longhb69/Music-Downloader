using System.ComponentModel.DataAnnotations;

namespace Music.Models;

public class Image
{
    public string Id { get; set; }  
    [Url]
    public string url { get; set; } 
    public int height { get; set; } 
    public int width { get; set; }  
}
