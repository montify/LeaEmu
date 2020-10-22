using SharpDX;
using SharpDX.Direct3D11;
using System.Runtime.InteropServices;
using Buffer = SharpDX.Direct3D11.Buffer;

namespace VideoLib
{
    public class VertexBuffer : BufferBase
    {
        public VertexBuffer(GraphicsDevice graphicsDevice, BufferUsage bufferUsage)
        {
            this.GraphicsDevice = graphicsDevice;
            this.BufferUsage = bufferUsage;
        }

        public void Create<T>(T[] data, int stride) where T : struct
        {
            Stride = stride;

            SizeInBytes = data.Length;
            if (BufferUsage == BufferUsage.Normal)
                NativeBuffer = Buffer.Create(GraphicsDevice.GetNativeDevice, BindFlags.VertexBuffer, data, Utilities.SizeOf(data));
            else
                NativeBuffer = Buffer.Create(GraphicsDevice.GetNativeDevice, BindFlags.VertexBuffer, data, Utilities.SizeOf(data), ResourceUsage.Dynamic, CpuAccessFlags.Write);
        }

        public void Create<T>(T[] data) where T : struct
        {
            Stride = Marshal.SizeOf(data[0]);
            
            if (BufferUsage == BufferUsage.Normal)
                NativeBuffer = Buffer.Create(GraphicsDevice.GetNativeDevice, BindFlags.VertexBuffer, data, Utilities.SizeOf(data));
            else
                NativeBuffer = Buffer.Create(GraphicsDevice.GetNativeDevice, BindFlags.VertexBuffer, data, Utilities.SizeOf(data), ResourceUsage.Dynamic);
        }
    }
}
