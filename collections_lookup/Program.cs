using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using ConsoleTableExt;

namespace collections_lookup
{
    class Program
    {
        static void Main(string[] args)
        {
            var counts = new[] { 10, 100, 1000, 10000, 100000, 1000000, 10000000 };

            Foo(counts, MeasureCreationAndLookup, "creation and lookup");
            Foo(counts, MeasureLookup, "lookup only");

            Console.ReadLine();
        }

        private static void Foo(IEnumerable<int> counts, Func<int, Stats> measureFunc, string title)
        {
            var table = CreateEmptyTable();

            var stats = counts.Select(measureFunc)
                .OrderBy(s => s.itemsCount)
                .ToArray();

            foreach (var s in stats)
            {
                table.Rows.Add(s.itemsCount, s.listTime, s.dictionaryTime, s.hashsetTime);
            }

            Console.WriteLine(title);

            ConsoleTableBuilder
                .From(table)
                .ExportAndWriteLine();
        }

        private static DataTable CreateEmptyTable()
        {
            var table = new DataTable();
            table.Columns.Add("items count", typeof(string));
            table.Columns.Add("list", typeof(string));
            table.Columns.Add("dictionary", typeof(string));
            table.Columns.Add("hashset", typeof(string));
            return table;
        }

        static Stats MeasureCreationAndLookup(int itemsCount)
        {
            Guid[] dataset = GenerateDataset(itemsCount);

            var itemToSearch = dataset[dataset.Length - 1];

            var listTime = Measure(() =>
            {
                var list = dataset.ToList();
                var contains = list.Contains(itemToSearch);
            });

            var dictionaryTime = Measure(() =>
            {
                var dictionary = dataset.ToDictionary(k => k);
                var contains = dictionary.ContainsKey(itemToSearch);
            });

            var hashsetTime = Measure(() =>
            {
                var hashset = dataset.ToHashSet();
                var contains = hashset.Contains(itemToSearch);
            });

            return new Stats()
            {
                itemsCount = itemsCount,
                listTime = listTime,
                dictionaryTime = dictionaryTime,
                hashsetTime = hashsetTime,
            };
        }

        static Stats MeasureLookup(int itemsCount)
        {
            Guid[] dataset = GenerateDataset(itemsCount);

            var itemToSearch = dataset[dataset.Length - 1];

            var list = dataset.ToList();
            var dictionary = dataset.ToDictionary(k => k);
            var hashset = dataset.ToHashSet();
          
            var listTime = Measure(() =>
            {
                var contains = list.Contains(itemToSearch);
            });

            var dictionaryTime = Measure(() =>
            {
                var contains = dictionary.ContainsKey(itemToSearch);
            });

            var hashsetTime = Measure(() =>
            {
                var contains = hashset.Contains(itemToSearch);
            });

            return new Stats()
            {
                itemsCount = itemsCount,
                listTime = listTime,
                dictionaryTime = dictionaryTime,
                hashsetTime = hashsetTime,
            };
        }

        private static Guid[] GenerateDataset(int itemsCount)
        {
            return Enumerable.Repeat(1, itemsCount)
                            .Select(i => Guid.NewGuid())
                            .ToArray();
        }

        static TimeSpan Measure(Action action)
        {
            var timer = new Stopwatch();
            timer.Start();

            action();

            timer.Stop();

            return timer.Elapsed;
        }
    }

    struct Stats
    {
        public int itemsCount;
        public TimeSpan listTime;
        public TimeSpan dictionaryTime;
        public TimeSpan hashsetTime;
    }
}
