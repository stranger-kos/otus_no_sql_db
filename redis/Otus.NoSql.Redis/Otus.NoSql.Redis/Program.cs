using System.Diagnostics;
using Otus.NoSql.Redis.Models;
using ServiceStack.Redis;

var values = new List<Employee>(5_000_00);

for (int i = 0; i < 5_000_00; i++)
{
    var employee = Employee.Get(i);

    values.Add(employee);
}

var redis = new RedisClient("localhost:6379");

using var pipeline = redis.CreatePipeline();

var watch = new Stopwatch();
Console.WriteLine("Begin load...");
watch.Start();

foreach (var item in values)
{
    //    string
    pipeline.QueueCommand(e => e.Add(item.Id.ToString(), item.Salary.ToString()));

    //    hash
    //pipeline.QueueCommand(e => e.SetEntryInHash(item.Id.ToString(), "salary", item.Salary.ToString()));

    //    zset
    //pipeline.QueueCommand(e => e.AddItemToSortedSet(item.Id.ToString(), item.Salary.ToString()));

    //list
    //pipeline.QueueCommand(e => e.AddItemToList(item.Id.ToString(), item.Salary.ToString()));
}

pipeline.Flush();

watch.Stop();

Console.WriteLine("Load time: " + watch.Elapsed);

watch = new Stopwatch();
Console.WriteLine("Begin Get...");
watch.Start();

//string
var value = redis.GetValue("11525");

//hash
//var value = redis.GetAllEntriesFromHash("11525");

//zset
//var value = redis.GetAllItemsFromSortedSet("11525");

//list
//var value = redis.GetAllItemsFromList("11525");

watch.Stop();
Console.WriteLine("Get time: " + watch.Elapsed);