using BenchmarkDotNet.Attributes;

namespace FindAndFirstDifference;

[MemoryDiagnoser]
public class ListTest
{
    [Benchmark]
    public int? TestFindInt()
    {
       try
       {
            var list = new List<int>(5000);
            for (int i = 0; i < 5000; i++)
            {
                list.Add(i);
            }
        
            return list.Find(i => i > 1200);
       }
       catch(Exception ex){
        Console.Write(ex.Message+"   EXCEPTION");
        throw;
       }
    }
    
    [Benchmark]
    public int? TestFirstInt()
    {
        var list = new List<int>(5000);
        for (int i = 0; i < 5000; i++)
        {
            list.Add(i);
        }
    
        return list.FirstOrDefault(i => i > 1200);
    }
    
    [Benchmark]
    public object? TestFindObject()
    {
        var list = new List<Student>(5000);
        for (int i = 0; i < 5000; i++)
        {
            list.Add(new Student()
            {
                Id = i + 1,
                Name = "Student" + i,
                Age = i % 4 + 18
            });
        }
    
        return list.Find(o => o.Id == 339);
    }
    
    [Benchmark]
    public object? TestFirstObject()
    {
        var list = new List<Student>(5000);
        for (int i = 0; i < 5000; i++)
        {
            list.Add(new Student()
            {
                Id = i+1,
                Name="Student"+i,
                Age = i%4 + 18
            });
        }
    
        return list.FirstOrDefault(o => o.Id==339);
    }
}

class Student
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Age { get; set; }
}