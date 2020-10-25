using System;

using SharpDX;
using SharpDX.D3DCompiler;
using SharpDX.Direct3D11;

namespace VideoLib
{
	public class Shader : IDisposable
	{
		private Effect m_Effect;
		private readonly string m_Path;
		private readonly GraphicsDevice m_GraphicsDevice;
		private EffectPass m_EffectPass;
		private InputLayout m_InputLayout;
		internal InputElement[] m_InputElements;

		public Effect Effect => m_Effect;

		public Shader(GraphicsDevice graphicsDevice, InputElement[] inputElements, string path)
		{
			this.m_GraphicsDevice = graphicsDevice;
			this.m_Path = path;
			this.m_InputElements = inputElements;

			LoadEffectFile();
		}

		private void LoadEffectFile()
		{
#if DEBUG

			var effectByteCode = ShaderBytecode.CompileFromFile(m_Path, "fx_5_0", ShaderFlags.Debug, EffectFlags.None);
#else
            var effectByteCode = ShaderBytecode.CompileFromFile(path, "fx_5_0", ShaderFlags.OptimizationLevel3, EffectFlags.None );
#endif
 //A
			m_Effect = new Effect(m_GraphicsDevice.GetNativeDevice, effectByteCode);

			var technique = m_Effect.GetTechniqueByIndex(0);
			m_EffectPass = technique.GetPassByIndex(0);

			// if it is a ComputeShader, they have no inputElement
			if (m_InputElements == null) return;

			var passSignature = m_EffectPass.Description.Signature;
			m_InputLayout = new InputLayout(m_GraphicsDevice.GetNativeDevice, passSignature, m_InputElements);
			passSignature.Dispose();
			effectByteCode.Dispose();
		}

		public void Apply()
		{
			// if it is a ComputeShader, they have no inputElements
			if (m_InputLayout != null)
				m_GraphicsDevice.SetInputLayout(m_InputLayout);

			m_EffectPass.Apply(m_GraphicsDevice.GetNativeDevice.ImmediateContext);
		}

		public void Dispose()
		{
			Utilities.Dispose(ref m_EffectPass);
			Utilities.Dispose(ref m_Effect);
			Utilities.Dispose(ref m_InputLayout);
		}
	}
}
