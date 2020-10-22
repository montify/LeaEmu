using System;
using System.IO;
using System.Runtime.InteropServices;

using SharpDX;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using StbImageSharp;


namespace VideoLib.src
{
	public class WTexture2D : IDisposable
	{
		private GraphicsDevice m_GraphicsDevice;
		private int m_Width;
		private int m_Height;
		private Texture2D m_NativeTexture;
		private UnorderedAccessView m_UnorderedAccessView;
		private ShaderResourceView m_ShaderResourceView;

		public int Width => m_Width;
		public int Height => m_Height;
		public ShaderResourceView ShaderResourceView => m_ShaderResourceView;

		public WTexture2D(GraphicsDevice graphicsDevice, string path)
		{
			m_GraphicsDevice = graphicsDevice;

			var buffer = File.ReadAllBytes(path);
			ImageResult image = ImageResult.FromMemory(buffer, ColorComponents.RedGreenBlueAlpha);

			m_Width = image.Width;
			m_Height = image.Height;

			GCHandle pinnedArray = GCHandle.Alloc(image.Data, GCHandleType.Pinned);
			IntPtr pointer = pinnedArray.AddrOfPinnedObject();

			m_NativeTexture = new Texture2D(m_GraphicsDevice.GetNativeDevice, new Texture2DDescription()
			{
				Width = m_Width,
				Height = m_Height,
				ArraySize = 1,
				BindFlags = BindFlags.ShaderResource | BindFlags.RenderTarget,
				Usage = ResourceUsage.Default,
				CpuAccessFlags = CpuAccessFlags.None,
				Format = Format.R8G8B8A8_UNorm,
				MipLevels = 0,
				OptionFlags = ResourceOptionFlags.GenerateMipMaps,
				SampleDescription = new SampleDescription(1, 0),
			});

			var dataBox = new DataBox(pointer, FormatHelper.SizeOfInBytes(Format.R8G8B8A8_UNorm) * m_Width, 0);
			m_GraphicsDevice.GetNativeDevice.ImmediateContext.UpdateSubresource(dataBox, m_NativeTexture);

			pinnedArray.Free();
			m_ShaderResourceView = new ShaderResourceView(m_GraphicsDevice.GetNativeDevice, m_NativeTexture);
			m_GraphicsDevice.GetNativeDevice.ImmediateContext.GenerateMips(m_ShaderResourceView);
			//SRV and UAV holds a reference to nativeTexture so we can delete that

		}

		public WTexture2D(GraphicsDevice graphicsDevice, int width, int height, Format textureFormat)
		{
			m_Width = width;
			m_Height = height;
			m_GraphicsDevice = graphicsDevice;

			m_NativeTexture = new Texture2D(m_GraphicsDevice.GetNativeDevice, new Texture2DDescription()
			{
				Width = m_Width,
				Height = m_Height,
				ArraySize = 1,
				BindFlags = BindFlags.UnorderedAccess | BindFlags.ShaderResource | BindFlags.RenderTarget, //RenderTarget need for mipMap
				Usage = ResourceUsage.Default,
				CpuAccessFlags = CpuAccessFlags.None,
				Format = textureFormat,
				MipLevels = 0,
				OptionFlags = ResourceOptionFlags.GenerateMipMaps,

				SampleDescription = new SampleDescription(1, 0),

			});

			m_ShaderResourceView = new ShaderResourceView(m_GraphicsDevice.GetNativeDevice, m_NativeTexture);
			m_UnorderedAccessView = new UnorderedAccessView(m_GraphicsDevice.GetNativeDevice, m_NativeTexture);

			//SRV and UAV holds a reference to nativeTexture so we can release that here
			Utilities.Dispose(ref m_NativeTexture);
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				Utilities.Dispose(ref m_NativeTexture);
				Utilities.Dispose(ref m_UnorderedAccessView);
				Utilities.Dispose(ref m_ShaderResourceView);
			}
		}
	}
}
