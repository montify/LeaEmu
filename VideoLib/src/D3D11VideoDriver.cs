using System;
using System.Diagnostics;
using System.Threading;
using SharpDX;
using SharpDX.Windows;

namespace VideoLib
{
	public class D3D11VideoDriver : IDisposable
	{
		private GraphicsDevice m_GraphicsDevice;
		private RenderForm m_RenderForm;
		private ImGuiRenderer m_ImGuiRenderer;
		private Stopwatch sw = new Stopwatch();

		public D3D11VideoDriver()
		{
			m_RenderForm = new RenderForm("Emulator");
			m_RenderForm.Width = 1280;
			m_RenderForm.Height = 720;
			m_GraphicsDevice = new GraphicsDevice(m_RenderForm);
			m_ImGuiRenderer = new ImGuiRenderer(m_RenderForm, m_GraphicsDevice);
		}

		private void Sleep(int targetMS, Action callback)
		{
			if (sw.Elapsed.Milliseconds < targetMS)
			{
				var sleeptime = targetMS - sw.Elapsed.Milliseconds;
				Thread.Sleep(sleeptime);
			}

			sw.Reset();
			sw.Start();
			callback();
			sw.Stop();
		}

		public void Run(Action callback)
		{
			int targetMS = 16;

			RenderLoop.Run(m_RenderForm, () =>
			{
				Sleep(targetMS, () =>
				{
					m_GraphicsDevice.ClearScreen();
					m_ImGuiRenderer.BeforeLayout();
					callback();
					m_ImGuiRenderer.AfterLayout();
					m_GraphicsDevice.Present();
				});

			});

			
		}

		public void Dispose()
		{
			m_ImGuiRenderer.Dispose();
			m_GraphicsDevice.Dispose();
			Utilities.Dispose(ref m_RenderForm);
		}
	}
}
