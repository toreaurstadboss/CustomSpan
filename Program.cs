public static partial class Program
{

    static void Main(string[] args)
    {

        //Demonstrate first how to just slice the Span two times

        var nums = Enumerable.Range(0, 1000).ToArray();
        var spanOfNums = new CustomSpan<int>(nums, 500, 500);
        var twentyToFifty = spanOfNums.Slice(20, 30);
        twentyToFifty.PrintArrayContents(); //prints 520..54

        //Demonstate how we can mutate the span contents - then we use GetWritable, which is a writable indexer

        for (int i = 0; i < twentyToFifty.Length; i++)
        {
            twentyToFifty.GetWritable(i) = (int)Math.Pow((double)twentyToFifty[i], 2); //mutates the Span contents - squares the elements , using GetWritable
        }

        twentyToFifty.PrintArrayContents(); //prints the squared values of the numbers in array twentyToFifty


    }

}
