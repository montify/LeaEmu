using System;
using System.Diagnostics;
using System.Threading;
using SharpDX;
using SharpDX.Direct3D11;
using SharpDX.Windows;
using VideoLib.src;

namespace VideoLib
{
	public class D3D11VideoDriver : IDisposable
	{
		private GraphicsDevice m_GraphicsDevice;
		private RenderForm m_RenderForm;
		private ImGuiRenderer m_ImGuiRenderer;
		private Stopwatch m_StopWatch = new Stopwatch();
		private WTexture2D m_Texture2D;

		public D3D11VideoDriver()
		{
			m_RenderForm = new RenderForm("Emulator");
			m_RenderForm.Width = 1280;
			m_RenderForm.Height = 720;
			
			m_GraphicsDevice = new GraphicsDevice(m_RenderForm);
			m_ImGuiRenderer = new ImGuiRenderer(m_RenderForm, m_GraphicsDevice);
			m_Texture2D = new WTexture2D(m_GraphicsDevice, "D:/g.png");
			m_ImGuiRenderer.BindTexture(m_Texture2D.ShaderResourceView);
		}

		private void Sleep(int targetMS, Action callback)
		{
			if (m_StopWatch.Elapsed.Milliseconds < targetMS)
			{
				var sleeptime = targetMS - m_StopWatch.Elapsed.Milliseconds;
				Thread.Sleep(sleeptime);
			}

			m_StopWatch.Reset();
			m_StopWatch.Start();
			callback();
			m_StopWatch.Stop();
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

					ImGuiNET.ImGui.Begin("IMAGE");
					ImGuiNET.ImGui.Image(m_Texture2D.ShaderResourceView.NativePointer, new System.Numerics.Vector2(m_Texture2D.Width, m_Texture2D.Height));
					ImGuiNET.ImGui.End();

					callback();

					m_ImGuiRenderer.AfterLayout();
					m_GraphicsDevice.Present();
				});

			});
			m_ImGuiRenderer.UnbindTexture(m_Texture2D.ShaderResourceView.NativePointer);

		}


		public void Dispose()
		{
			m_Texture2D.Dispose();
			m_ImGuiRenderer.Dispose();
			m_GraphicsDevice.Dispose();
			Utilities.Dispose(ref m_RenderForm);	
		}
	}
}
