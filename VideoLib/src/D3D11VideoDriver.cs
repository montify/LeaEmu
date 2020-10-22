using System;
using ImGuiNET;
using SharpDX;
using SharpDX.Windows;
using MemoryLib;

namespace VideoLib
{
	public class D3D11VideoDriver : IDisposable
	{
		private GraphicsDevice m_GraphicsDevice;
		private RenderForm m_RenderForm;

		ImGuiRenderer m_ImGuiRenderer;

		public D3D11VideoDriver()
		{
			m_RenderForm = new RenderForm("Emulator");
			m_RenderForm.Width = 1280;
			m_RenderForm.Height = 720;
			m_GraphicsDevice = new GraphicsDevice(m_RenderForm);
			m_ImGuiRenderer = new ImGuiRenderer(m_RenderForm, m_GraphicsDevice);
		}

		public void Run(Action callback)
		{
			RenderLoop.Run(m_RenderForm, () =>
			{
				m_GraphicsDevice.ClearScreen();

				m_ImGuiRenderer.BeforeLayout();
				callback();
				m_ImGuiRenderer.AfterLayout();

				m_GraphicsDevice.Present();
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
