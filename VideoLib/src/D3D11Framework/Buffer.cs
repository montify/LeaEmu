using SharpDX;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using System;
using System.Runtime.InteropServices;
using NativeBuffer = SharpDX.Direct3D11.Buffer;

namespace VideoLib
{
	public abstract class Buffer : IDisposable
	{
		protected NativeBuffer m_NativeBuffer;
		protected GraphicsDevice m_GraphicsDevice;
		protected BufferUsage m_BufferUsage;
		protected Format m_Format;
		protected int m_Stride;
		protected int m_SizeInBytes;
		protected BindFlags m_BindFlags;

		public NativeBuffer NativeBuffer => m_NativeBuffer;
		public int Stride => m_Stride;
		public Format Format => m_Format;

		protected Buffer(GraphicsDevice graphicsDevice)
		{
			m_GraphicsDevice = graphicsDevice;
		}

		public void Create<T>(T[] data) where T : struct
		{
			m_Stride = Marshal.SizeOf(data[0]);

			if (m_BufferUsage == BufferUsage.Normal)
				m_NativeBuffer = SharpDX.Direct3D11.Buffer.Create(m_GraphicsDevice.GetNativeDevice, m_BindFlags, data, Utilities.SizeOf(data));
			else
				m_NativeBuffer = SharpDX.Direct3D11.Buffer.Create(m_GraphicsDevice.GetNativeDevice, m_BindFlags, data, Utilities.SizeOf(data), ResourceUsage.Dynamic, CpuAccessFlags.Write);
		}

		public void Create<T>(T[] data, int stride) where T : struct
		{
			m_Stride = stride;

			m_SizeInBytes = data.Length;

			if (m_BufferUsage == BufferUsage.Normal)
				m_NativeBuffer = SharpDX.Direct3D11.Buffer.Create(m_GraphicsDevice.GetNativeDevice, BindFlags.VertexBuffer, data, Utilities.SizeOf(data));
			else
				m_NativeBuffer = SharpDX.Direct3D11.Buffer.Create(m_GraphicsDevice.GetNativeDevice, BindFlags.VertexBuffer, data, Utilities.SizeOf(data), ResourceUsage.Dynamic, CpuAccessFlags.Write);
		}

		public void Update<T>(T[] data, int offset)
			where T : struct
		{
			if (m_BufferUsage == BufferUsage.Normal)
				throw new Exception("Buffer is not dynamic!");

			m_GraphicsDevice.GetNativeDevice.ImmediateContext.MapSubresource(NativeBuffer, MapMode.WriteDiscard, SharpDX.Direct3D11.MapFlags.None, out var dataStream);
			Utilities.Write(dataStream.DataPointer, data, offset, data.Length);
			m_GraphicsDevice.GetNativeDevice.ImmediateContext.UnmapSubresource(NativeBuffer, 0);
			dataStream.Dispose();
		}

		public void Dispose()
		{
			Utilities.Dispose(ref m_NativeBuffer);

			GC.SuppressFinalize(this);
		}
	}
}
