﻿using System.Text.Json;
using CS_FULLSTACK_28._11._2024.Context;
using CS_FULLSTACK_28._11._2024.Models;

internal class Program
{
    public static void Main()
    {
        //Vi henter inn en ny DataContext med all data vi trenger. 
        var context = new DataContext();
        /* var result = GetFrequencyOfGenre(context); */
        /* var result = GetTopTenMovies(context); */
        var result = GetMostAwardWinningGenre(context);
        SaveObjectAsJson(result);
    }
    //Lage en funksjon som kan lagre et objekt som json.
    public static void SaveObjectAsJson(object obj)
    {
        var jsonString = JsonSerializer.Serialize(obj);
        File.WriteAllText($"{Guid.NewGuid()}-result.json", jsonString);
    }

    //Lage en funksjon, som kan returnere frekvensen til sjangre. 
    public static object GetFrequencyOfGenre(DataContext context)
    {

        return context.Movies
                            //vi begynner med å lage en stor liste som inneholder alle elementene i alle Genre listene i hver Movie
                            .SelectMany(movie => movie.Genres)
                            //Vi grupperer hver genre med hverandre,
                            // slik at vi ender opp med en group som representerer alle gangene en genre dukker opp i resultatet fra SelectMany
                            .GroupBy(genre => genre)
                            //Vi prosjekterer hver gruppe til et nytt objekt, som inneholder navnet på sjangeren under Genre,
                            // og hvor mange elementer som fins i groupen under Count. 
                            .Select(group => new
                            {
                                Genre = group.Key,
                                Count = group.Count()
                            })
                            //Vi bruker orderbydescending til å sortere slik at den største verdien av Count kommer først.
                            .OrderByDescending(obj => obj.Count)
                            //Vi lager en liste av resultatet av spørringen vår over. 
                            .ToList();
    }

    //Lage en funksjon som returnerer de 10 beste filmene i følger userStats
    public static  List<Movie> GetTopTenMovies(DataContext context)
    {
        return context.Movies.Where(
            movie => 
            //Vi lager en lambdafunksjon hvor vi kryssrefererer mot UserMoviestats.
            context.UserMovieStats
            //Vi bruker OrderByDescending for å sortere UserMovieStats basert på rating, høyest rating først.
                .OrderByDescending(stat => stat.Rating)
                //Vi tar med videre tittelen for hver rating.
                .Select(stat => stat.Title)
                //Vi tar 10 elementer.
                .Take(10)
                //Vi ser om listen av ti elementer inneholder tittelen til vårt movie parameter.
                .Contains(movie.Title))
            //Vi returnerer resultatet til en liste over filmer.
            .ToList();
    }
    //En funksjon som returnerer hvilke sjanger som vinner best picture flest ganger.
    public static object GetMostAwardWinningGenre(DataContext context)
    {
        return context.Movies.Where(
            movie => 
            //Vi lager en kryssreferering til bestPicture.
            context.BestPictures
                    //Vi tar alle titlene i best picture til en egen liste.
                    .Select(pic => pic.Title)
                    //Vi ser om parameter movie sin tittel er i listen over best picture.
                    .Contains(movie.Title))
        //Da har vi filtrert vekk alle instanser av Movie objekter i Movies som ikke er i Best Picture.
        //Vi bruker SelectMany for å få samlet alle Genres i en stor liste av strenger.
        .SelectMany(movie => movie.Genres)
        //Vi grupperer hver genre med seg selv
        .GroupBy(genre => genre)
        //Vi prosjekterer hver gruppe til en nytt objekt
        .Select(group => new
        {
            Genre = group.Key,
            Count = group.Count()
        })
        //Vi sorterer resultatet basert på hvor stor Count er. Vi bruker OrderByDescending for å få det største resultatet først.
        .OrderByDescending(obj => obj.Count)
        //Vi kollapser spørringen til en liste. 
        .ToList();
    }
}