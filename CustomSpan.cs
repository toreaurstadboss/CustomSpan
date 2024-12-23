using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

public readonly ref struct CustomSpan<T>
{
    private readonly ref T _reference;
    private readonly int _length;

    public int Length => _length;

    public CustomSpan(T[] array)
    {
        ArgumentNullException.ThrowIfNull(array);
        if (!typeof(T).IsValueType && array.GetType() != typeof(T[]))
        {
            // Covariance guard
            throw new ArgumentException($"Covariance between types {typeof(T).FullName} and {array.GetType().FullName} is not supported in CustomSpan");
        }
        _reference = ref MemoryMarshal.GetArrayDataReference(array);
        _length = array.Length;
    }

    public CustomSpan(T[] array, int start, int length)
    {
        ArgumentNullException.ThrowIfNull(array);
        if (!typeof(T).IsValueType && array.GetType() != typeof(T[]))
        {
            // Covariance guard
            throw new ArgumentException($"Covariance between types {typeof(T).FullName} and {array.GetType().FullName} is not supported in CustomSpan");
        }


#if TARGET_64BIT
		if ((ulong)(uint)start + (ulong)(uint)length > (ulong)(uint)array.Length){
			throw new IndexOutOfRangeException("The index was out of bounds for the array");
		}	
#else
        if ((uint)start + (uint)length > (uint)array.Length)
        {
            throw new IndexOutOfRangeException("The index was out of bounds for the array");
        }
#endif


        _reference = ref Unsafe.Add(ref MemoryMarshal.GetArrayDataReference(array), (nint)(uint)start); //nint - native integer
        _length = length;

    }

    public CustomSpan(ref T reference)
    {
        _reference = ref reference;
        _length = 1;
    }

    public CustomSpan(ref T reference, int length)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(length, 0);
        _reference = ref reference;
        _length = length;
    }

    // Read-only indexer
    public ref readonly T this[int index]
    {
        get
        {
            if ((uint)index >= (uint)_length)
            {
                throw new IndexOutOfRangeException();
            }
            return ref Unsafe.Add(ref _reference, index);
        }
    }

    // Read-write indexer
    public ref T GetWritable(int index)
    {
        if ((uint)index >= (uint)_length)
        {
            throw new IndexOutOfRangeException();
        }
        return ref Unsafe.Add(ref _reference, index);
    }

    public CustomSpan<T> Slice(int offset)
    {
        if ((uint)offset > (uint)_length)
        {
            throw new ArgumentOutOfRangeException();
        }
        return new CustomSpan<T>(ref Unsafe.Add(ref _reference, offset), _length - offset);
    }

    public CustomSpan<T> Slice(int offset, int sliceLength)
    {
        if (((uint)offset + (uint)sliceLength) > (uint)_length)
        {
            throw new ArgumentOutOfRangeException();
        }
        return new CustomSpan<T>(ref Unsafe.Add(ref _reference, offset), sliceLength);
    }

    public void PrintArrayContents()
    {
        for (int i = 0; i < _length; i++)
        {
            Console.WriteLine(this[i]);
        }
    }

    public ReadOnlySpan<T> AsReadOnlySpan()
    {
        return MemoryMarshal.CreateReadOnlySpan(ref _reference, _length);
    }

}
