using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace DailyProgrammer
{
    class Program
    {
        static void Main(string[] args)
        {
            var league = new Tournament();
            league.GenerateRounds();
        }
    }


    public class Team
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public List<Team> TeamsPlayed { get; set; }
        public bool PlayedToday { get; set; }

        public Team(int id, string name)
        {
            Id = id;
            Name = name;
            TeamsPlayed = new List<Team>();
            PlayedToday = false;
        }
    }

    public class Tournament
    {
        public List<Team> Teams { get; private set; }
        public List<string> Matchups { get; private set; }
        public static Random RNG = new Random();

        public Tournament()
        {
            Teams = new List<Team>();
            var path = Path.Combine(Directory.GetCurrentDirectory(), @"Teams.txt");
            var teamNames = File.ReadAllLines(path);
            for (int i = 0; i < teamNames.Length; i++)
            {
                var team = new Team(i, teamNames[i]);
                Teams.Add(team);
            }
        }


        public void GenerateRounds()
        {
            //Home
            for (int i = 0; i < Teams.Count - 1; i++)
            {
                Console.WriteLine($"Round {i + 1}\n");

                GenerateHomeGames();
                //Clear up at the end of the day
                Teams.ForEach(x => x.PlayedToday = false);
                Console.WriteLine();
            }

            //Resets List of teams played
            Teams.ForEach(x => x.TeamsPlayed.Clear());

            //Away
            for (int i = Teams.Count; i < 2 * Teams.Count - 2; i++)
            {
                Console.WriteLine($"Round {i + 1}\n");

                GenerateAwayGames();
                //Clear up at the end of the day
                Teams.ForEach(x => x.PlayedToday = false);
                Console.WriteLine();
            }

        }

        public void GenerateHomeGames()
        {
            for (int j = 0; j < Teams.Count; j++)
            {
                GenerateGames(j);
                

            }
        }

        public void GenerateAwayGames()
        {
            for (int j = Teams.Count/2; j < 2*Teams.Count - 2; j++)
            {
                GenerateGames(j);
                
            }
        }

        public void GenerateGames(int index)
        {
            //Checks if the current team has played today
            if (Teams[(index >= Teams.Count) ? index - Teams.Count : index].PlayedToday)
            {
                return;
            }

            int randomNum = RNG.Next(Teams.Count);
            //First clause checks if the team was matched up vs itself
            //Second clause checks if it has played the team before
            //Third clause checks if the team to be played has already played today
            while ((randomNum == index)
                || Teams[index].TeamsPlayed.Any(x => x.Id == index)
                || Teams[randomNum].PlayedToday)
            {
                randomNum = RNG.Next(Teams.Count);
            }

            Console.WriteLine($"{Teams[index].Name} - {Teams[randomNum].Name}");

            //Now add who home and away team played
            Teams[index].TeamsPlayed.Add(Teams[randomNum]);
            Teams[randomNum].TeamsPlayed.Add(Teams[index]);

            Teams[index].PlayedToday = true;
            Teams[randomNum].PlayedToday = true;

        }

    }
}