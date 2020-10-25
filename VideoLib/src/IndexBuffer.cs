using SharpDX;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using System.Runtime.InteropServices;
using Buffer = SharpDX.Direct3D11.Buffer;

namespace VideoLib
{
    public class IndexBuffer : BufferBase
    {
        public Format format;
        public IndexBuffer(GraphicsDevice graphicsDevice, Format format, BufferUsage bufferUsage)
        {
            this.GraphicsDevice = graphicsDevice;
            this.BufferUsage = bufferUsage;
            this.format = format;
        }

        public void Create<T>(T[] data) where T : struct
        {
            Stride = Marshal.SizeOf(data[0]);

            if (BufferUsage == BufferUsage.Normal)
                NativeBuffer = Buffer.Create(GraphicsDevice.GetNativeDevice, BindFlags.IndexBuffer, data, Utilities.SizeOf(data));
            else
                NativeBuffer = Buffer.Create(GraphicsDevice.GetNativeDevice, BindFlags.IndexBuffer, data, Utilities.SizeOf(data), ResourceUsage.Dynamic, CpuAccessFlags.Write);
        }
   
    }
}
