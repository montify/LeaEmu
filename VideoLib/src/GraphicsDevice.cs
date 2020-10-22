
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
		private readonly RenderForm m_RenderForm;
		private Device m_Device;
		private SwapChain m_Swapchain;
		private RenderTargetView m_RenderView;
		private Texture2D m_BackBuffer;
		public Device GetDevice { get; private set; }

		public GraphicsDevice(RenderForm renderForm)
		{
			this.m_RenderForm = renderForm;
			CreateDeviceAndSwapChain();
		}

		public void ClearScreen()
		{
			m_Device.ImmediateContext.ClearRenderTargetView(m_RenderView, Color.Red);
		}

		public void Present()
		{
			m_Swapchain.Present(0, PresentFlags.None);
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
			m_Device.ImmediateContext.Rasterizer.SetViewport(new Viewport(0, 0, m_RenderForm.ClientSize.Width, m_RenderForm.ClientSize.Height, 0.0f, 1.0f));
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
