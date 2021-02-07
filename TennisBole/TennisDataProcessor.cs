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
            public int ATP { get; set; }
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
            const string DataNotAvailable = "None";

            Players.Clear();

            UTR.ImportCSV(UTRFileName);
            ATP.ImportCSV(ATPFileName);

            bool test = UTR.Players[68].Equals(UTR.Players[0]);

            foreach (TennisRawData.Player UTRPlayer in UTR.Players)
            {
                if (UTRPlayer.Age.Equals(DataNotAvailable) ||
                    UTRPlayer.Nationality.Equals(DataNotAvailable) ||
                    UTRPlayer.Gender.Equals(DataNotAvailable))
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
                    combinedPlayer.ATP = (int)ATPPlayer.Ranking;

                    ATP.Players.Remove(ATPPlayer);
                }
                else
                {
                    combinedPlayer.Name = UTRPlayer.Name;
                    combinedPlayer.Age = int.Parse(UTRPlayer.Age);
                    combinedPlayer.Gender = UTRPlayer.Gender;
                    combinedPlayer.Nationality = UTRPlayer.Nationality;
                    combinedPlayer.UTR = UTRPlayer.Ranking;
                    combinedPlayer.ATP = (int)RankingNotAvailable;
                }

                Players.Add(combinedPlayer);

            }

            foreach (TennisRawData.Player ATPPlayer in ATP.Players)
            {
                if (ATPPlayer.Age.Equals(DataNotAvailable) ||
                    ATPPlayer.Nationality.Equals(DataNotAvailable) ||
                    ATPPlayer.Gender.Equals(DataNotAvailable))
                    continue;

                Player combinedPlayer = new Player();

                combinedPlayer.Name = ATPPlayer.Name;
                combinedPlayer.Age = int.Parse(ATPPlayer.Age);
                combinedPlayer.Gender = ATPPlayer.Gender;
                combinedPlayer.Nationality = ATPPlayer.Nationality;
                combinedPlayer.UTR = RankingNotAvailable;
                combinedPlayer.ATP = (int)ATPPlayer.Ranking;

                Players.Add(combinedPlayer);
            }

            Players = Players.Distinct().ToList();
        }
        private static void EliminateOverAgedPlayer()
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
        private static void EliminateLowUTRPlayer()
        {
            List<Player> toBeEliminated = new List<Player>();

            foreach (Player player in Players)
            {
                if (player.UTR < MinimumUTRRanking)
                    toBeEliminated.Add(player);
            }

            foreach (Player player in toBeEliminated)
                Players.Remove(player);
        }
        private static void CalculateMyRank()
        {
            int maxATP = 0;
            foreach (Player player in Players)
            {
                if (player.ATP > maxATP)
                    maxATP = player.ATP;
            }

            foreach (Player player in Players)
            {
                if (player.UTR == RankingNotAvailable)
                    player.MyRank = player.ATP;
                else if (player.ATP == RankingNotAvailable)
                    player.MyRank = player.UTR;
                else
                    player.MyRank = CalculateMyRank(player.UTR, player.ATP, maxATP);
            }

            Players.Sort((x, y) => -x.MyRank.CompareTo(y.MyRank));
        }
        private static double CalculateMyRank(double UTR, int ATP, int maxATP)
        {
            const double MyRankMax = 16.5;
            double processedATP = (MyRankMax - 1) * ((maxATP - ATP + 1) / maxATP) + 1;
            return (UTR * UTRWeight + processedATP * ATPWeight) / 100;
        }
        public static List<ListViewItem> GetPlayerListViewItems()
        {
            const string DataNotAvailable = "N/A";

            TennisDataProcessor.EliminateOverAgedPlayer();
            TennisDataProcessor.EliminateLowUTRPlayer();
            TennisDataProcessor.CalculateMyRank();

            List<ListViewItem> items = new List<ListViewItem>();

            foreach (Player player in Players)
            {
                ListViewItem item = new ListViewItem(player.Name);

                item.SubItems.Add(player.Age.ToString());
                item.SubItems.Add(player.Gender);
                item.SubItems.Add(IOCConverter.CodeToCountryName(player.Nationality));
                if (player.UTR == RankingNotAvailable)
                    item.SubItems.Add(DataNotAvailable);
                else
                    item.SubItems.Add(player.UTR.ToString(DoubleToStringFormat));
                if (player.ATP == RankingNotAvailable)
                    item.SubItems.Add(DataNotAvailable);
                else
                    item.SubItems.Add(player.ATP.ToString());
                item.SubItems.Add(player.MyRank.ToString(DoubleToStringFormat));

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

        public record Country
        {
            public string Code;

            public double SumUTR;
            public double SumATP;
            public double SumMyRank;

            public double AvgUTR;
            public double AvgATP;
            public double AvgMyRank;

            public int TotalPlayers;
        }
        private static List<Country> Countries { get; set; } = new List<Country>();
        private static void GenerateCountryData()
        {
            foreach (Player player in Players)
            {
                if (Countries.Exists((x) => x.Code.Equals(player.Nationality)))
                {
                    Country country = Countries.FindLast((x) => x.Code.Equals(player.Nationality));

                    country.TotalPlayers++;
                    country.SumUTR += player.UTR;
                    country.SumATP += player.ATP;
                    country.SumMyRank += player.MyRank;
                }
                else
                {
                    Country country = new Country();

                    country.Code = player.Nationality;
                    country.TotalPlayers = 1;
                    country.SumUTR = player.UTR;
                    country.SumATP = player.ATP;
                    country.SumMyRank = player.MyRank;

                    Countries.Add(country);
                }
            }

            Countries.ForEach((x) =>
            {
                x.AvgUTR = x.SumUTR / x.TotalPlayers;
                x.AvgATP = x.SumATP / x.TotalPlayers;
                x.AvgMyRank = x.SumMyRank / x.TotalPlayers;
            });
            Countries.Sort((x, y) => -x.AvgMyRank.CompareTo(y.AvgMyRank));
        }
        public static List<ListViewItem> GetCountryListViewItems()
        {
            TennisDataProcessor.GenerateCountryData();
            List<ListViewItem> countryListViewItems = new List<ListViewItem>();

            foreach (Country country in Countries)
            {
                ListViewItem countryItem = new ListViewItem(IOCConverter.CodeToCountryName(country.Code));
                countryItem.SubItems.Add(country.TotalPlayers.ToString());
                countryItem.SubItems.Add(country.AvgUTR.ToString(DoubleToStringFormat));
                countryItem.SubItems.Add(country.AvgATP.ToString(DoubleToStringFormat));
                countryItem.SubItems.Add(country.AvgMyRank.ToString(DoubleToStringFormat));

                countryListViewItems.Add(countryItem);
            }

            return countryListViewItems;
        }
        private static string DoubleToStringFormat = "N2";
    }
}
