using System;
using System.IO;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using CsvHelper;

namespace TennisBole
{
    public static class TennisDataProcessor
    {
        private class TennisRawData
        {
            public class Player : IEquatable<Player>
            {
                public string Name { get; set; }
                public string Age { get; set; }
                public string Gender { get; set; }
                public string Nationality { get; set; }
                public double Ranking { get; set; }

                public bool Equals(Player other)
                {
                    if (this.Name.Equals(other.Name) &&
                        this.Age == other.Age &&
                        this.Gender.Equals(other.Gender) &&
                        this.Nationality.Equals(other.Nationality))
                    {
                        return true;
                    }
                    else return false;
                }
                public override int GetHashCode()
                {
                    return string.Concat(Name, Age, Gender, Nationality).GetHashCode();
                }
            }
            public List<Player> Players { get; set; } = new List<Player>();
            public void ImportCSV(string fileName)
            {
                Players.Clear();

                using (TextReader tennisCSVFile = new StreamReader(fileName))
                using (CsvReader tennisCSVReader = new CsvReader(tennisCSVFile, CultureInfo.InvariantCulture))
                {
                    tennisCSVReader.Read();
                    tennisCSVReader.ReadHeader();

                    while (tennisCSVReader.Read())
                    {
                        Player player = tennisCSVReader.GetRecord<Player>();
                        Players.Add(player);
                    }
                }

                Players = Players.Distinct().ToList();
            }
        }
        private static List<Player> Players { get; set; } = new List<Player>();
        private static TennisRawData UTR = new TennisRawData();
        private static TennisRawData ATP = new TennisRawData();
        private class Player : IEquatable<Player>
        {
            public string Name { get; set; }
            public int Age { get; set; }
            public string Gender { get; set; }
            public string Nationality { get; set; }
            public double UTR { get; set; }
            public double ATP { get; set; }
            public double MyRank { get; set; }
            public bool Equals(Player other)
            {
                if (this.Name.Equals(other.Name) &&
                    this.Age == other.Age &&
                    this.Gender.Equals(other.Gender) &&
                    this.Nationality.Equals(other.Nationality))
                {
                    return true;
                }
                else return false;
            }
        }
        private static readonly double RankingNotAvailable = -1.0;
        public static void ImportCSV(string UTRFileName, string ATPFileName)
        {
            Players.Clear();

            UTR.ImportCSV(UTRFileName);
            ATP.ImportCSV(ATPFileName);

            bool test = UTR.Players[68].Equals(UTR.Players[0]);

            foreach (TennisRawData.Player UTRPlayer in UTR.Players)
            {
                if (UTRPlayer.Age.Equals("None") || 
                    UTRPlayer.Nationality.Equals("None") ||
                    UTRPlayer.Gender.Equals("None"))
                    continue;

                Player combinedPlayer = new Player();

                if (ATP.Players.Exists((x) => x.Equals(UTRPlayer)))
                {
                    TennisRawData.Player ATPPlayer = ATP.Players.FindLast((x) => x.Equals(UTRPlayer));

                    combinedPlayer.Name = UTRPlayer.Name;
                    combinedPlayer.Age = int.Parse(UTRPlayer.Age);
                    combinedPlayer.Gender = UTRPlayer.Gender;
                    combinedPlayer.Nationality = UTRPlayer.Nationality;
                    combinedPlayer.UTR = UTRPlayer.Ranking;
                    combinedPlayer.ATP = ATPPlayer.Ranking;

                    ATP.Players.Remove(ATPPlayer);
                }
                else
                {
                    combinedPlayer.Name = UTRPlayer.Name;
                    combinedPlayer.Age = int.Parse(UTRPlayer.Age);
                    combinedPlayer.Gender = UTRPlayer.Gender;
                    combinedPlayer.Nationality = UTRPlayer.Nationality;
                    combinedPlayer.UTR = UTRPlayer.Ranking;
                    combinedPlayer.ATP = RankingNotAvailable;
                }

                Players.Add(combinedPlayer);

            }

            foreach (TennisRawData.Player ATPPlayer in ATP.Players)
            {
                if (ATPPlayer.Age.Equals("None") ||
                    ATPPlayer.Nationality.Equals("None") ||
                    ATPPlayer.Gender.Equals("None"))
                    continue;

                Player combinedPlayer = new Player();

                combinedPlayer.Name = ATPPlayer.Name;
                combinedPlayer.Age = int.Parse(ATPPlayer.Age);
                combinedPlayer.Gender = ATPPlayer.Gender;
                combinedPlayer.Nationality = ATPPlayer.Nationality;
                combinedPlayer.UTR = RankingNotAvailable;
                combinedPlayer.ATP = ATPPlayer.Ranking;

                Players.Add(combinedPlayer);
            }

            Players = Players.Distinct().ToList();
        }
        public static void EliminateOverAgedPlayer()
        {
            List<Player> toBeEliminated = new List<Player>();

            foreach (Player player in Players)
            {
                bool shouldEliminate = false;

                if (SpecificLimits.Exists((x) => x.Nationality.Equals(player.Nationality)))
                {
                    SpecificLimit limit = SpecificLimits.FindLast((x) => x.Nationality.Equals(player.Nationality));
                    if (player.Age > limit.Age)
                        shouldEliminate = true;
                }
                else if (GlobalMaxAge != -1)
                {
                    if (player.Age > GlobalMaxAge)
                        shouldEliminate = true;
                }

                if (shouldEliminate)
                    toBeEliminated.Add(player);
            }

            foreach (Player player in toBeEliminated)
                Players.Remove(player);
        }
        private static readonly double MinimumUTRRanking = 11.0;
        public static void EliminateLowUTRPlayer()
        {
            List<Player> toBeEliminated = new List<Player>();

            foreach (Player player in Players)
            {
                if (player.UTR<MinimumUTRRanking)
                    toBeEliminated.Add(player);
            }

            foreach (Player player in toBeEliminated)
                Players.Remove(player);
        }
        public static void CalculateMyRank()
        {
            foreach(Player player in Players)
            {
                if (player.UTR == RankingNotAvailable)
                    player.MyRank = player.ATP;
                else if (player.ATP == RankingNotAvailable)
                    player.MyRank = player.UTR;
                else
                    player.MyRank = (player.UTR * UTRWeight + player.ATP * ATPWeight) / 100;
            }

            Players.Sort((x, y) => -x.MyRank.CompareTo(y.MyRank));
        }
        public static List<ListViewItem> GetListViewItems()
        {
            List<ListViewItem> items = new List<ListViewItem>();

            foreach (Player player in Players)
            {
                ListViewItem item = new ListViewItem(player.Name);

                item.SubItems.Add(player.Age.ToString());
                item.SubItems.Add(player.Gender);
                item.SubItems.Add(IOCConverter.CodeToCountryName(player.Nationality));
                if(player.UTR==RankingNotAvailable)
                    item.SubItems.Add("N/A");
                else
                    item.SubItems.Add(player.UTR.ToString());
                if (player.ATP == RankingNotAvailable)
                    item.SubItems.Add("N/A");
                else
                    item.SubItems.Add(player.ATP.ToString());
                item.SubItems.Add(player.MyRank.ToString());

                items.Add(item);
            }

            return items;
        }
        public static int GlobalMaxAge { get; set; } = 18;
        public static readonly int NoGlobalMaxAge = -1;
        public record SpecificLimit
        {
            public string Nationality;
            public int Age;
        }
        public static List<SpecificLimit> SpecificLimits { get; set; } = new List<SpecificLimit>();

        public static readonly int DefaultWeight = 50;
        public static int UTRWeight { get; set; } = DefaultWeight;
        public static int ATPWeight { get; set; } = DefaultWeight;
    }
}
