using System;
using SharpDX;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using SharpDX.Windows;


namespace VideoLib
{
	using Device = SharpDX.Direct3D11.Device;
	public class GraphicsDevice : IDisposable
	{
		public Device GetNativeDevice => m_Device;
		private readonly RenderForm m_RenderForm;
		private Device m_Device;
		private SwapChain m_Swapchain;
		private RenderTargetView m_RenderView;
		private Texture2D m_BackBuffer;
		private Viewport m_ViewPort;

<<<<<<< HEAD:VideoLib/src/D3D11Framework/GraphicsDevice.cs
		public Viewport Viewport => m_ViewPort;
		public Device GetNativeDevice => m_Device;

=======
>>>>>>> parent of 2e6e8f2... Refactor this mess ;):VideoLib/src/GraphicsDevice.cs
		public GraphicsDevice(RenderForm renderForm)
		{
			this.m_RenderForm = renderForm;
			CreateDeviceAndSwapChain();

			m_ViewPort = new Viewport(0, 0, m_RenderForm.ClientSize.Width, m_RenderForm.ClientSize.Height, 0.0f, 1.0f);
		}

		public void ClearScreen()
		{
			m_Device.ImmediateContext.ClearRenderTargetView(m_RenderView, Color.Black);
		}

		public void SetTopology(PrimitiveTopology topology)
		{
			m_Device.ImmediateContext.InputAssembler.PrimitiveTopology = topology;
		}
		internal void SetInputLayout(InputLayout inputLayout)
		{
			m_Device.ImmediateContext.InputAssembler.InputLayout = inputLayout;
		}

		public void SetVertexBuffer(VertexBuffer vertexBuffer, int offset = 0)
		{
			m_Device.ImmediateContext.InputAssembler.SetVertexBuffers(0, new VertexBufferBinding(vertexBuffer.NativeBuffer, vertexBuffer.Stride, offset));
		}

		public void SetIndexBuffer(IndexBuffer indexBuffer, int offset = 0)
		{
			m_Device.ImmediateContext.InputAssembler.SetIndexBuffer(indexBuffer.NativeBuffer, indexBuffer.format, offset);
		}
		public void DrawIndexed(int count, int startIndexLocation, int baseVertexLocation)
		{
			if (count <= 0)
				throw new ArgumentException("Count cant be 0");

			m_Device.ImmediateContext.DrawIndexed(count, startIndexLocation, baseVertexLocation);
<<<<<<< HEAD:VideoLib/src/D3D11Framework/GraphicsDevice.cs
		}
		public void Draw(int vertexCount, int startLocation)
		{
			m_Device.ImmediateContext.Draw(vertexCount, startLocation);
=======

>>>>>>> parent of 2e6e8f2... Refactor this mess ;):VideoLib/src/GraphicsDevice.cs
		}
		public void Present()
		{
			m_Swapchain.Present(1, PresentFlags.None);
		}

		private void CreateDeviceAndSwapChain()
		{
			var desc = new SwapChainDescription()
			{
				BufferCount = 1,
				ModeDescription =
								  new ModeDescription(m_RenderForm.ClientSize.Width, m_RenderForm.ClientSize.Height,
													  new Rational(60, 1), Format.R8G8B8A8_UNorm),
				IsWindowed = true,
				OutputHandle = m_RenderForm.Handle,
				SampleDescription = new SampleDescription(1, 0),
				SwapEffect = SwapEffect.Discard,
				Usage = Usage.RenderTargetOutput
			};

			Device.CreateWithSwapChain(DriverType.Hardware, DeviceCreationFlags.None, desc, out m_Device, out m_Swapchain);

			m_BackBuffer = Texture2D.FromSwapChain<Texture2D>(m_Swapchain, 0);
			m_RenderView = new RenderTargetView(m_Device, m_BackBuffer);
			m_Device.ImmediateContext.Rasterizer.SetViewport(m_ViewPort);
			m_Device.ImmediateContext.OutputMerger.SetTargets(m_RenderView);
		}

		public void Dispose()
		{
			GC.SuppressFinalize(true);
			Utilities.Dispose(ref m_BackBuffer);
			Utilities.Dispose(ref m_RenderView);
			Utilities.Dispose(ref m_Swapchain);
			Utilities.Dispose(ref m_Device);
		}
	}
}
