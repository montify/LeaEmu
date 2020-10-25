using SharpDX;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using System;
using System.Collections.Generic;

using System.Linq;
using VideoLib.src.D3D11Framework.SpriteBatchSrc;

namespace VideoLib.src.D3D11Framework
{
	public class SpriteBatcher : IDisposable
	{
		private int maxBatchSize;

		//public  List<SpriteInfo> spriteList;

		public readonly List<RenderBatchInfo> renderBatches;
		public readonly List<SpriteInfo> spriteList;
		private readonly GraphicsDevice graphicsDevice;
		private readonly SpriteBatchVertex[] fontVertex;
		private readonly VertexBuffer vertexBuffer;
		//private readonly LeaSamplerState sampler;
		private Shader effect;
		public Matrix ScaleMatrix { get; set; }
		private Matrix MVP;
		private BlendState bs, bs1;
		private ShaderResourceView oldTexture, currentTexture;
		bool once;


		public SpriteBatcher(GraphicsDevice graphicsDevice, int maxBatchSize)
		{
			this.graphicsDevice = graphicsDevice;
			this.maxBatchSize = maxBatchSize;

			spriteList = new List<SpriteInfo>();
			fontVertex = new SpriteBatchVertex[maxBatchSize];

			renderBatches = new List<RenderBatchInfo>();

			vertexBuffer = new VertexBuffer(graphicsDevice, BufferUsage.Dynamic);
			vertexBuffer.Create(fontVertex);

			//sampler = new LeaSamplerState();
			//sampler.GenerateSamplers(graphicsDevice);

			CreateEffect();
			CreateBlendSates();

			//	effect.SetVariable("textureAtlasResWidthHeight", "startUp", 512, ShaderType.GeometryShader);
			//effect.SetSampler(sampler, 0, ShaderType.PixelShader);
		}

		private void CreateEffect()
		{
		
			effect = new Shader(graphicsDevice, SpriteBatchVertex.InputElements, "PAATH");
		}

		private void CreateBlendSates()
		{
			var desc1 = new BlendStateDescription();
			desc1.RenderTarget[0].IsBlendEnabled = true;

			desc1.RenderTarget[0].BlendOperation = BlendOperation.Add;
			desc1.RenderTarget[0].AlphaBlendOperation = BlendOperation.Add;

			desc1.RenderTarget[0].SourceBlend = BlendOption.One;
			desc1.RenderTarget[0].SourceAlphaBlend = BlendOption.One;

			desc1.RenderTarget[0].DestinationBlend = BlendOption.InverseSourceAlpha;
			desc1.RenderTarget[0].DestinationAlphaBlend = BlendOption.Zero;
			desc1.RenderTarget[0].RenderTargetWriteMask = ColorWriteMaskFlags.All;

			bs = new BlendState(graphicsDevice.GetNativeDevice, desc1);

			desc1.RenderTarget[0].DestinationBlend = BlendOption.Zero;

			bs1 = new BlendState(graphicsDevice.GetNativeDevice, desc1);


		}


		public void PrepareForRendering()
		{
			MVP = Matrix.OrthoOffCenterLH(0, graphicsDevice.Viewport.Width, graphicsDevice.Viewport.Height, 0, 0, 1);
			MVP = Matrix.Transpose(ScaleMatrix * MVP);

			spriteList.Clear();
			renderBatches.Clear();
		}

		public void AddSpriteInfo(SpriteInfo spriteInfo, ref int ptr)
		{
			spriteList.Add(spriteInfo);

			if (spriteList.Count >= maxBatchSize)
			{
				Draw();
				PrepareForRendering();
				ptr = 0;
			}
		}

		private void CreateRenderBatches()
		{
			//if (spriteList.Count == 0)
			//	return;

			//spriteList = spriteList.OrderBy(o => o.srv.GetHashCode()).ToList();

			renderBatches.Add(new RenderBatchInfo(spriteList[0].srv, 0, 1));



			int offset = 1;

			for (int i = 1; i < spriteList.Count; i++)
			{
				if (spriteList[i].srv == null)
					break;

				var currentSprite = spriteList[i];

				if (currentSprite.textureHashCode != spriteList[i - 1].textureHashCode)
					renderBatches.Add(new RenderBatchInfo(currentSprite.srv, offset, 1));
				else
					renderBatches.Last().numVertices += 1;

				fontVertex[i].Position = currentSprite.position;
				fontVertex[i].Size = currentSprite.size;
				fontVertex[i].Color = currentSprite.color;
				fontVertex[i].Offset = currentSprite.offset;

				offset++;
			}
			vertexBuffer.Update(fontVertex, 0);
		}

		private void DrawBatches()
		{

			foreach (var rb in renderBatches)
			{
				if (rb.texture != currentTexture)
				{
					effect.Effect.GetVariableByName("").AsShaderResource().SetResource(rb.texture);
					currentTexture = rb.texture;
				}

				effect.Apply();
				graphicsDevice.Draw(rb.numVertices, rb.offset);

			}
		}

		public void Draw()
		{
			if (spriteList.Count > 0)
			{
				CreateRenderBatches();
				DrawBatches();
			}
		}

		public void InternalBegin()
		{
			graphicsDevice.SetTopology(PrimitiveTopology.PointList);

			effect.Effect.GetVariableByName("ProjMatrix").AsMatrix().SetMatrix(MVP);
			//graphicsDevice.IsDepthEnable(false);
			//graphicsDevice.SetblendState(bs);
			graphicsDevice.SetVertexBuffer(vertexBuffer);
		}

		public void End()
		{
			//graphicsDevice.SetblendState(bs1);
			//graphicsDevice.IsDepthEnable(true);
		}

		public void Dispose()
		{
			//sampler.Dispose();
			vertexBuffer.Dispose();
			oldTexture?.Dispose();
			currentTexture?.Dispose();
			bs.Dispose();
			bs1.Dispose();
		}
	}
}
