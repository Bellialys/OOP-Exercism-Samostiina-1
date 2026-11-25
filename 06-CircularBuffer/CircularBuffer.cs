using System;

public class CircularBuffer<T>
{
    private readonly T[] buffer;
    private int readIndex, writeIndex, count;
    public CircularBuffer(int capacity) => buffer = new T[capacity];
    public T Read()
    {
        if (count == 0) throw new InvalidOperationException("Buffer is empty.");
        T value = buffer[readIndex];
        buffer[readIndex] = default!;
        readIndex = (readIndex + 1) % buffer.Length;
        count--;
        return value;
    }
    public void Write(T value)
    {
        if (count == buffer.Length) throw new InvalidOperationException("Buffer is full.");
        buffer[writeIndex] = value;
        writeIndex = (writeIndex + 1) % buffer.Length;
        count++;
    }
    public void Overwrite(T value)
    {
        if (count == buffer.Length) Read();
        Write(value);
    }
    public void Clear()
    {
        Array.Clear(buffer, 0, buffer.Length);
        readIndex = writeIndex = count = 0;
    }
}
