using SharpDX;
using SharpDX.Direct3D11;
using System;
using Buffer = SharpDX.Direct3D11.Buffer;


namespace VideoLib
{
    public abstract class BufferBase : IDisposable
    {
        internal Buffer NativeBuffer;
        internal GraphicsDevice GraphicsDevice;
        internal BufferUsage BufferUsage;
        internal int Stride;
        internal int SizeInBytes;

        public void Update<T>(T[] data, int offset) 
            where T : struct
        {
            if (BufferUsage == BufferUsage.Normal)
                throw new Exception("Buffer is not dynamic!");

            WriteToBuffer(data, offset);
        }

        private void WriteToBuffer<T>(T[] data, int offset) 
            where T : struct
        {
            GraphicsDevice.GetNativeDevice.ImmediateContext.MapSubresource(NativeBuffer, MapMode.WriteDiscard, MapFlags.None, out var dataStream);
            Utilities.Write(dataStream.DataPointer, data, offset, data.Length);
            GraphicsDevice.GetNativeDevice.ImmediateContext.UnmapSubresource(NativeBuffer, 0);
            dataStream.Dispose();
        }

        public void Dispose()
        {
            Utilities.Dispose(ref NativeBuffer);

            GC.SuppressFinalize(this);
        }
    }
}
