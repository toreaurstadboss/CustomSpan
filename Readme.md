## CustomSpan

A bare bones implementation of a Span in C#
The code here is just for demonstration purposes.
To use the custom Span, just instantiate it and use it as a regular span in C#.
Note - you should of course just use Span in .NET, the code is just for demonstration purposes.

```csharp

void Main(){

	var nums = Enumerable.Range(0, 1000).ToArray();
    var spanOfNums = new CustomSpan<int>(nums, 500, 500); //picks 500-999 elements from the nums
	var twentyToFifty = spanOfNums.Slice(20, 30); //slices the 20-50 of the spanOfNums
	twentyToFifty.PrintArrayContents(); //prints 520..549
}

```

The span is using the default indexer immutable, since it is readonly, but we provide a writable method too, in case we want to modify an element in the array.

```csharp

   //Demonstate how we can mutate the span contents - then we use GetWritable, which is a writable indexer

   for (int i = 0; i < twentyToFifty.Length; i++)
   {
       twentyToFifty.GetWritable(i) = (int)Math.Pow((double)twentyToFifty[i], 2); //mutates the Span contents - squares the elements , using GetWritable
       twentyToFifty.PrintArrayContents(); //prints the squared values of the numbers in array twentyToFifty
   }

```

See through the code to see how the custom Span is built up. The regular Span in System.Span namespace got a lot of functionality, the purpose of this repo was just to show some demo code displaying the Span.
