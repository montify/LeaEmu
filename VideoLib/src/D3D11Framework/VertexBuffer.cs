using SharpDX.Direct3D11;

namespace VideoLib
{
	public sealed class VertexBuffer : Buffer
	{
		public VertexBuffer(GraphicsDevice graphicsDevice, BufferUsage bufferUsage)
		 : base(graphicsDevice)
		{
			m_BufferUsage = bufferUsage;
			m_Format = default;
			m_BindFlags = BindFlags.IndexBuffer; 
		}
	}
}
