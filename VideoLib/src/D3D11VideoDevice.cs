using System;
using SharpDX;
using SharpDX.Windows;

namespace VideoLib
{
	public class D3D11VideoDevice : IDisposable
	{
		private GraphicsDevice m_GraphicsDevice;
		private RenderForm m_RenderForm;

		public D3D11VideoDevice()
		{
			m_RenderForm = new RenderForm("Emulator");
			m_GraphicsDevice = new GraphicsDevice(m_RenderForm);
		}

		public void Run(Action callback)
		{
			RenderLoop.Run(m_RenderForm, () =>
			{
				m_GraphicsDevice.ClearScreen();
				callback();

				m_GraphicsDevice.Present();
			});
		}

		public void Dispose()
		{
			m_GraphicsDevice.Dispose();
			Utilities.Dispose(ref m_RenderForm);
		}
	}
}
