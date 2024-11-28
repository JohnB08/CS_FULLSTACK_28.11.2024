using System;
using System.Text.Json.Serialization;

namespace CS_FULLSTACK_28._11._2024.Models;


//Model av data vi vil hente fra objektene i Movies.Json
public class Movie
{
    [JsonPropertyName("title")]
    public string? Title { get; set; }
    [JsonPropertyName("year")]
    public int Year { get; set; }
    [JsonPropertyName("cast")]
    public List<string> Cast { get; set; } = [];
    [JsonPropertyName("genres")]
    public List<string> Genres { get; set; } = [];
}
