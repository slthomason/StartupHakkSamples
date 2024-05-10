using System.Collections;

namespace RecordsAreCheatersInDDD
{
    internal class Program
    {
        public record RecordWithArray1(int number, string[] strings);
        
        public record struct RecordWithArray2(int number, string[] strings);

        //Just for to be complete
        public record class RecordWithArray3(int number, string[] strings);

        static void Main(string[] args)
        {
            //Are r1 and r2 same?
            RecordWithArray1 r1 = new(1, ["string", "one"]);
            RecordWithArray1 r2 = new(1, ["string", "one"]);

            //Are r3 and r4 same?
            RecordWithArray2 r3 = new(1, ["string", "two"]);
            RecordWithArray2 r4 = new(1, ["string", "two"]);

            //Are r5 and r6 same?
            RecordWithArray3 r5 = new(1, ["string", "three"]);
            RecordWithArray3 r6 = new(1, ["string", "three"]);

            string[] sa1 = ["stringarray", "one"];
            string[] sa2 = ["stringarray", "two"];

            Console.WriteLine($"Two << record >> with array members are same is ( == operator : EqualityComparer) => {r1 == r2} : { EqualityComparer<RecordWithArray1>.Default.Equals(r1, r2) }");
            Console.WriteLine($"Two << record struct >> with array members are same is ( == operator : EqualityComparer) => {r3 == r4} : {EqualityComparer<RecordWithArray2>.Default.Equals(r3, r4)}");
            Console.WriteLine($"Two << record class >> with array members are same is ( == operator : EqualityComparer) => {r5 == r6} : {EqualityComparer<RecordWithArray3>.Default.Equals(r5, r6)}");
            Console.WriteLine("-----------------------------------------------------------------------------------------------------------------");
            Console.WriteLine($"Two << string[] >> is equal with << == >> operator is: { sa1 == sa2 }");
            Console.WriteLine($"Two << string[] >> is equal with EqualityComparer is: { EqualityComparer<string[]>.Default.Equals(sa1, sa2) }");
        }
    }
}
