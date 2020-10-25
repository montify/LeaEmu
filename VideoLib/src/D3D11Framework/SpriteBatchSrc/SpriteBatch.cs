using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SharpDX;

namespace VideoLib.src.D3D11Framework
{
	public enum SortMode { BackToFront, FrontToBack, Texture, Immediate }

	public class SpriteBatch : IDisposable
	{
		private GraphicsDevice graphicsDevice;
		private SortMode sortMode;
		private readonly SpriteBatcher spriteBatcher;
		SpriteInfo[] spriteInfoList;
		int ptr;

		public SpriteBatch(GraphicsDevice graphicsDevice, int maxBatchSize = 1024)
		{
			this.graphicsDevice = graphicsDevice;
			spriteInfoList = new SpriteInfo[maxBatchSize];

			for (int i = 0; i < spriteInfoList.Length; i++)
			{
				spriteInfoList[i] = new SpriteInfo();
			}
			spriteBatcher = new SpriteBatcher(graphicsDevice, maxBatchSize);


		}

		public void Begin(Matrix scaleMatrix, SortMode sortMode = SortMode.Texture)
		{
			this.sortMode = sortMode;

			spriteBatcher.ScaleMatrix = scaleMatrix;

			spriteBatcher.PrepareForRendering();
			spriteBatcher.InternalBegin();
		}

		public void Submit(WTexture2D tex, Vector2 position, Vector2 size, Color color)
		{
			spriteInfoList[ptr].position = position;
			spriteInfoList[ptr].size = new Vector2(tex.Width, tex.Height);
			spriteInfoList[ptr].offset = Vector2.Zero;
			spriteInfoList[ptr].color = color.ToVector4();
			spriteInfoList[ptr].srv = tex.ShaderResourceView;
			spriteInfoList[ptr].textureHashCode = tex.GetHashCode();

			//var tmpSprite = new SpriteInfo
			//{
			//	position = position,
			//	size = new Vector2(tex.Width, tex.Height),
			//	offset = Vector2.Zero,
			//	color = color.ToVector4(),
			//	srv = tex.ShaderResourceView,
			//	textureHashCode = tex.GetHashCode()
			//};


			spriteBatcher.AddSpriteInfo(spriteInfoList[ptr], ref ptr);

			ptr++;
		}

		public void End()
		{
			spriteBatcher.Draw();
			spriteBatcher.End();
			ptr = 0;

		}

		public void Dispose()
		{
			spriteBatcher.Dispose();

		}
	}
}
