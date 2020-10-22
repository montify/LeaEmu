using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using ImGuiNET;
using SharpDX;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using SharpDX.Windows;

namespace VideoLib
{
	public class ImGuiRenderer : IDisposable
	{
		private GraphicsDevice graphicsDevice;
		private RenderForm m_RenderForm;
		VertexBuffer vertexBuffer;
		IndexBuffer indexBuffer;
		Shader effect;
		private IntPtr? _fontTextureId;
		private Dictionary<IntPtr, ShaderResourceView> _loadedTextures;
		private int _textureId;
		int _indexBufferSize;
		private SamplerState nativeSamplerState;
		//  LeaSamplerState sampler;
		private BlendState bs;
		private List<int> _keys = new List<int>();
		private byte[] _vertexData;
		private Texture2D fontTexture;
		private int _vertexBufferSize;

		private byte[] _indexData;

		private bool m_IsMouseLeftDown;

		private ShaderResourceView srv;

		public ImGuiRenderer(RenderForm renderForm, GraphicsDevice graphicsDevice)
		{
			this.graphicsDevice = graphicsDevice;
			m_RenderForm = renderForm;

			_loadedTextures = new Dictionary<IntPtr, ShaderResourceView>();
			// sampler = new LeaSamplerState();
			// sampler.GenerateSamplers(graphicsDevice);


			var desc1 = new BlendStateDescription();
			desc1.AlphaToCoverageEnable = false;
			desc1.RenderTarget[0].IsBlendEnabled = true;
			desc1.RenderTarget[0].SourceBlend = BlendOption.SourceAlpha;
			desc1.RenderTarget[0].DestinationBlend = BlendOption.InverseSourceAlpha;
			desc1.RenderTarget[0].BlendOperation = BlendOperation.Add;
			desc1.RenderTarget[0].SourceAlphaBlend = BlendOption.InverseSourceAlpha;
			desc1.RenderTarget[0].DestinationAlphaBlend = BlendOption.Zero;
			desc1.RenderTarget[0].AlphaBlendOperation = BlendOperation.Add;
			desc1.RenderTarget[0].RenderTargetWriteMask = ColorWriteMaskFlags.All;

			bs = new BlendState(graphicsDevice.GetNativeDevice, desc1);

			var inputElements = new InputElement[3];
			inputElements[0] = new InputElement("POSITION", 0, Format.R32G32_Float,
				InputElement.AppendAligned, 0, InputClassification.PerVertexData, 0);
			inputElements[1] = new InputElement("TEXCOORD", 0, Format.R32G32_Float,
				InputElement.AppendAligned, 0, InputClassification.PerVertexData, 0);
			inputElements[2] = new InputElement("COLOR", 0, Format.R8G8B8A8_UNorm,
				InputElement.AppendAligned, 0, InputClassification.PerVertexData, 0);

			effect = new Shader(graphicsDevice, inputElements, "D:/GIT/LeaEmu/VideoLib/src/imgui.fx");

			var context = ImGui.CreateContext();
			ImGui.SetCurrentContext(context);

			ImGui.GetIO().Fonts.AddFontDefault();
			ImGui.GetIO().BackendFlags |= ImGuiBackendFlags.RendererHasVtxOffset;
			ImGui.GetIO().ConfigFlags |= ImGuiConfigFlags.DockingEnable ;

			ImGui.GetIO().ConfigDockingWithShift = false;
			RebuildFontAtlas();
			ImGui.NewFrame();

			InputCallbacks();

		}

		protected virtual void InputCallbacks()
		{
			var io = ImGui.GetIO();

			m_RenderForm.MouseMove += (sender, args) =>
		   	{
				   io.MousePos = new System.Numerics.Vector2(args.X, args.Y);
		   	};

			m_RenderForm.MouseDown += (sender, args) =>
			{
				if (args.Button == System.Windows.Forms.MouseButtons.Left)
					io.MouseDown[0] = true;
			};

			m_RenderForm.MouseUp += (sender, args) =>
			{
				if (args.Button == System.Windows.Forms.MouseButtons.Left)
					io.MouseDown[0] = false;
			};

			m_RenderForm.KeyDown += (sender, args) =>
			{
				io.KeysDown[(int)args.KeyCode] = true;
				io.AddInputCharacter((uint)args.KeyCode);
			};

			m_RenderForm.KeyUp += (sender, args) =>
			{
				io.KeysDown[(int)args.KeyCode] = false;
				// io.AddInputCharacter((uint)args.KeyCode);
			};
		}

		public unsafe void RebuildFontAtlas()
		{
			var io = ImGui.GetIO();

			io.Fonts.GetTexDataAsRGBA32(out byte* pixelData, out int width, out int height, out int bytesPerPixel);

			var length = width * height * bytesPerPixel;


			using var stream = new DataStream(length, true, true);

			Utilities.CopyMemory(stream.DataPointer, (IntPtr)pixelData, length);

			fontTexture = new Texture2D(graphicsDevice.GetNativeDevice, new Texture2DDescription()
			{
				Width = width,
				Height = height,
				ArraySize = 1,
				BindFlags = BindFlags.ShaderResource,
				Usage = ResourceUsage.Default,
				CpuAccessFlags = CpuAccessFlags.None,
				Format = Format.R8G8B8A8_UNorm,
				MipLevels = 1,
				OptionFlags = ResourceOptionFlags.None,
				SampleDescription = new SampleDescription(1, 0)
			}, new DataRectangle(stream.DataPointer, width * 4));



			srv = new ShaderResourceView(graphicsDevice.GetNativeDevice, fontTexture);


			// Should a texture already have been build previously, unbind it first so it can be deallocated
			if (_fontTextureId.HasValue) UnbindTexture(_fontTextureId.Value);

			// Bind the new texture to an ImGui-friendly id
			_fontTextureId = BindTexture(srv);

			// Let ImGui know where to find the texture
			io.Fonts.SetTexID(_fontTextureId.Value);
			io.Fonts.ClearTexData(); // Clears CPU side texture data
		}

		public virtual IntPtr BindTexture(ShaderResourceView texture)
		{
			var id = new IntPtr(_textureId++);

			_loadedTextures.Add(id, texture);

			return id;
		}

		///// <summary>
		///// Removes a previously created texture pointer, releasing its reference and allowing it to be deallocated
		///// </summary>
		public virtual void UnbindTexture(IntPtr textureId)
		{
			_loadedTextures.Remove(textureId);

		}

		public virtual void BeforeLayout(/*GameTimer gameTime*/)
		{
			// ImGui.GetIO().DeltaTime = (float) gameTime.FrameTime;

			var io = ImGui.GetIO();
			io.DisplaySize = new System.Numerics.Vector2(m_RenderForm.ClientSize.Width, m_RenderForm.ClientSize.Height);




			io.DisplayFramebufferScale = System.Numerics.Vector2.One;

			//UpdateInput();

			ImGui.NewFrame();
		}

		public virtual void AfterLayout()
		{
			ImGui.Render();

			RenderDrawData(ImGui.GetDrawData());

		}


		private void RenderDrawData(ImDrawDataPtr drawData)
		{
			if (!drawData.Valid)
				throw new Exception("");

			UpdateBuffers(drawData);
			RenderCommandLists1(drawData);
		}

		private unsafe void UpdateBuffers(ImDrawDataPtr drawData)
		{
			if (drawData.TotalVtxCount == 0)
				return;

			if (drawData.TotalVtxCount > _vertexBufferSize)
			{
				vertexBuffer?.Dispose();
				_vertexBufferSize = (int)(drawData.TotalVtxCount * 1.5f);
				vertexBuffer = new VertexBuffer(graphicsDevice, BufferUsage.Dynamic);
				_vertexData = new byte[_vertexBufferSize * Marshal.SizeOf(typeof(ImDrawVert))];
				vertexBuffer.Create(_vertexData, Marshal.SizeOf(typeof(ImDrawVert)));
			}

			if (drawData.TotalIdxCount > _indexBufferSize)
			{
				indexBuffer?.Dispose();
				_indexBufferSize = (int)(drawData.TotalIdxCount * 1.5f);
				indexBuffer = new IndexBuffer(graphicsDevice, Format.R16_UInt, BufferUsage.Dynamic);
				_indexData = new byte[_indexBufferSize * sizeof(ushort)];
				indexBuffer.Create(_indexData);
			}

			// Copy ImGui's vertices and indices to a set of managed byte arrays
			int vtxOffset = 0;
			int idxOffset = 0;

			for (int n = 0; n < drawData.CmdListsCount; n++)
			{
				ImDrawListPtr cmdList = drawData.CmdListsRange[n];

				fixed (void* vtxDstPtr = &_vertexData[vtxOffset * Marshal.SizeOf(typeof(ImDrawVert))])
				fixed (void* idxDstPtr = &_indexData[idxOffset * sizeof(ushort)])
				{
					System.Buffer.MemoryCopy((void*)cmdList.VtxBuffer.Data, vtxDstPtr, _vertexData.Length,
						cmdList.VtxBuffer.Size * Marshal.SizeOf(typeof(ImDrawVert)));

					System.Buffer.MemoryCopy((void*)cmdList.IdxBuffer.Data, idxDstPtr, _indexData.Length,
						cmdList.IdxBuffer.Size * sizeof(ushort));
				}

				vtxOffset += cmdList.VtxBuffer.Size;
				idxOffset += cmdList.IdxBuffer.Size;
			}

			indexBuffer.Update(_indexData, 0);
			vertexBuffer.Update(_vertexData, 0);
		}

		private unsafe void RenderCommandLists1(ImDrawDataPtr drawData)
		{
			if (drawData.TotalVtxCount == 0)
				return;

			graphicsDevice.SetVertexBuffer(vertexBuffer);
			graphicsDevice.SetIndexBuffer(indexBuffer);

			int vtxOffset = 0;
			int idxOffset = 0;

			for (int n = 0; n < drawData.CmdListsCount; n++)
			{
				ImDrawListPtr cmdList = drawData.CmdListsRange[n];

				for (int cmdi = 0; cmdi < cmdList.CmdBuffer.Size; cmdi++)
				{
					ImDrawCmdPtr drawCmd = cmdList.CmdBuffer[cmdi];

					if (!_loadedTextures.ContainsKey(drawCmd.TextureId))
					{
						throw new InvalidOperationException(
							$"Could not find a texture with id '{drawCmd.TextureId}', please check your bindings");
					}

					var io = ImGui.GetIO();

					io.DisplaySize =
						new System.Numerics.Vector2(m_RenderForm.ClientSize.Width, m_RenderForm.ClientSize.Height);

					effect.Effect.GetVariableByName("Projection").AsMatrix().SetMatrix(Matrix.OrthoOffCenterLH(0, io.DisplaySize.X, io.DisplaySize.Y, 0, 0, 1f));

					effect.Effect.GetVariableByName("fontAtlas").AsShaderResource().SetResource(_loadedTextures[drawCmd.TextureId]);
					DepthStencilStateDescription d = new DepthStencilStateDescription();
					d.IsDepthEnabled = false;
					d.DepthWriteMask = DepthWriteMask.All;
					d.DepthComparison = Comparison.Always;
					d.IsStencilEnabled = false;
					d.BackFace = d.FrontFace;

					var x = new DepthStencilState(graphicsDevice.GetNativeDevice, d);
					graphicsDevice.GetNativeDevice.ImmediateContext.OutputMerger.SetDepthStencilState(x);
					graphicsDevice.GetNativeDevice.ImmediateContext.OutputMerger.BlendState = bs;

					// graphicsDevice.device1.ImmediateContext1.Rasterizer.SetScissorRectangle(
					//  0, 0, graphicsDevice.RenderForm.Width, graphicsDevice.RenderForm.Height);

					effect.Apply();
					graphicsDevice.SetTopology(PrimitiveTopology.TriangleList);
					graphicsDevice.DrawIndexed((int)drawCmd.ElemCount, idxOffset, vtxOffset);

					Utilities.Dispose(ref x);
					idxOffset += (int)drawCmd.ElemCount;
				}

				vtxOffset += cmdList.VtxBuffer.Size;
			}
			graphicsDevice.GetNativeDevice.ImmediateContext.OutputMerger.SetDepthStencilState(null);
			graphicsDevice.GetNativeDevice.ImmediateContext.OutputMerger.BlendState = null;
		}

		public void Dispose()
		{
			indexBuffer.Dispose();
			vertexBuffer.Dispose();
			effect.Dispose();

			Utilities.Dispose(ref nativeSamplerState);
			Utilities.Dispose(ref bs);
			Utilities.Dispose(ref srv);
			Utilities.Dispose(ref fontTexture);
			GC.SuppressFinalize(this);
		}
	}
}
