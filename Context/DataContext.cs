
using System.Text.Json;
using CS_FULLSTACK_28._11._2024.Models;

namespace CS_FULLSTACK_28._11._2024.Context;


//Her lager vi en modell for en felles kolleksjon av all data vår applikasjon skal bruke. 
public class DataContext
{
    public List<Movie> Movies;
    public List<BestPicture> BestPictures;

    public UserDictionary Users;
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
        Users = new UserDictionary(Movies);
    }
    public class UserDictionary(List<Movie> movies) : Dictionary<string, List<UserMovieStats>>
    {
        private readonly List<UserMovieStats> _movieStats = movies.Select(movie => new UserMovieStats() { Title = movie.Title }).ToList();
        public void Add(string name)
        {
            if (ContainsKey(name)) return;
            this[name] = _movieStats;
        }
    }
}
