using System;
using SharpDX.Direct3D11;

namespace VideoLib.src.D3D11Framework
{
	public class RenderBatchInfo
	{
		public int offset;
		public int numVertices;
		public ShaderResourceView texture;


		public RenderBatchInfo(ShaderResourceView texture, int offset, int numVertices)
		{
			this.texture = texture;
			this.offset = offset;
			this.numVertices = numVertices;
		}
	}
}
