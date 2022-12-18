// See https://aka.ms/new-console-template for more information

//Console.WriteLine("Hello, World!");

using newtonsoft_playpen;
using Newtonsoft.Json;

var emp = new Employee { Id = Guid.NewGuid(), Name = "Nauman" };

var json = JsonConvert.SerializeObject(emp, Formatting.Indented, new EmployeeConverter());

Console.WriteLine(json);

var deserialized = JsonConvert.DeserializeObject<Employee>(json, new EmployeeConverter());

Console.WriteLine($"{deserialized.Id} -- {deserialized.Name}");
