using SharpDX.Direct3D11;
using SharpDX.DXGI;

namespace VideoLib
{
	public sealed class IndexBuffer : Buffer
	{
		public IndexBuffer(GraphicsDevice graphicsDevice, Format format, BufferUsage bufferUsage)
		    : base(graphicsDevice)
		{
			m_BufferUsage = bufferUsage;
			m_Format = format;
			m_BindFlags = BindFlags.IndexBuffer;
		}
	}
}
