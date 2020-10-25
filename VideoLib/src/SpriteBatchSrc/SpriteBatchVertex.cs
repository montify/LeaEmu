using System;
using SharpDX;
using SharpDX.Direct3D11;
using SharpDX.DXGI;

namespace VideoLib.src.D3D11Framework.SpriteBatchSrc
{
	public struct SpriteBatchVertex
	{
		public Vector2 Position;
		public Vector2 Size;
		public Vector4 Color;
		public Vector2 Offset;

		public static InputElement[] InputElements;


		public SpriteBatchVertex(Vector2 position, Vector2 size, Vector4 color, Vector2 offset)
		{
			Position = position;
			Size = size;
			Color = color;
			Offset = offset;

			InputElements = new InputElement[4];

			InputElements[0] = new InputElement("POSITION", 0, Format.R32G32_Float, InputElement.AppendAligned, 0, InputClassification.PerVertexData, 0);
			InputElements[1] = new InputElement("SIZE", 0, Format.R32G32_Float, InputElement.AppendAligned, 0, InputClassification.PerVertexData, 0);
			InputElements[2] = new InputElement("COLOR", 0, Format.R32G32B32A32_Float, InputElement.AppendAligned, 0, InputClassification.PerVertexData, 0);
			InputElements[3] = new InputElement("OFFSET", 0, Format.R32G32_Float, InputElement.AppendAligned, 0, InputClassification.PerVertexData, 0);
		}
	}
}
