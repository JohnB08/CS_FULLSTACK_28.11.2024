using System.Net.Http.Headers;
using System.Text.Json;
using CS_FULLSTACK_28._11._2024.Models;

namespace CS_FULLSTACK_28._11._2024.Context;


//Her lager vi en modell for en felles kolleksjon av all data vår applikasjon skal bruke. 
public class DataContext
{
    public List<Movie> Movies;
    public List<BestPicture> BestPictures;
    public List<UserMovieStats> UserMovieStats;
    //Vi bruker konstruktoren for å passe på at all data fra filer kommer inn i rett format. 
    public DataContext()
    {
        //Vi henter inn data fra de to json filene.
        var jsonMovieString = File.ReadAllText("Movies.json");
        var jsonBestPictureString = File.ReadAllText("BestPictures.json");

        //Vi initialiserer Movies som resultatet fra JsonSerializeren eller en tom liste.
        Movies = JsonSerializer.Deserialize<List<Movie>>(jsonMovieString) ?? [];

        //Siden jsonSerializeren kun kan serialize til en nøyaktig representasjon av dataen i jsonStringen,
        // må vi først generere en Liste av liste av strenger. Her velger vi å returnere en tom liste, hvis serializeren refererer til null.
        var tempList = JsonSerializer.Deserialize<List<List<string>>>(jsonBestPictureString) ?? [];

        //Vi bruker verdiene i tempList, og prosjekterer ut et ny BestPicture objekt for hvert listeelement i tempList.
        BestPictures = tempList.Select(list =>
        {
            int.TryParse(list[1], out int result);
            return new BestPicture()
            {
                Title = list[0],
                Year = result
            };
        }).ToList();

        //Vi genererer en liste av UserMovieStats objekter for hver instans av Movie i Movies.
        UserMovieStats = Movies.Select(movie => new UserMovieStats()
        {
            Title = movie.Title
        }).ToList();

        //Vi velger ca 30% av alle UserMovieStats objektene våre, og gir de HasSeen = true, og en random rating mellom 0 - 10.
        var randomizer = new Random();
        foreach (var item in UserMovieStats.OrderBy(_ => Guid.NewGuid()).Take((int)(UserMovieStats.Count * 0.3)))
        {
            item.HasSeen = true;
            item.Rating = randomizer.NextDouble() * 10;
        }
    }
}
