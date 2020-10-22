using System;

using SharpDX;
using SharpDX.D3DCompiler;
using SharpDX.Direct3D11;

namespace VideoLib
{
   public class Shader : IDisposable
    {
        private Effect effect;
        private readonly string path;
        private readonly GraphicsDevice graphicsDevice;
        private EffectPass pass;
        private InputLayout inputLayout;

        internal InputElement[] inputElements;

        public Effect Effect => effect;

        public Shader(GraphicsDevice graphicsDevice, InputElement[] inputElements, string path)
        {
            this.graphicsDevice = graphicsDevice;
            this.path = path;
            this.inputElements = inputElements;

            LoadEffectFile();

        }

        private void LoadEffectFile()
        {
#if DEBUG
         
            var effectByteCode = ShaderBytecode.CompileFromFile(path, "fx_5_0", ShaderFlags.Debug , EffectFlags.None);
#else
            var effectByteCode = ShaderBytecode.CompileFromFile(path, "fx_5_0", ShaderFlags.OptimizationLevel3, EffectFlags.None );
#endif

            if(effectByteCode.HasErrors)
            {
                Console.WriteLine();
            }
            effect = new Effect(graphicsDevice.GetNativeDevice, effectByteCode);
            var technique = effect.GetTechniqueByIndex(0);
            pass = technique.GetPassByIndex(0);

            // if it is a ComputeShader, they have no inputElements, dont try to create, just return
            if (inputElements == null) return;

            var passSignature = pass.Description.Signature;
            inputLayout = new InputLayout(graphicsDevice.GetNativeDevice, passSignature, inputElements);
            passSignature.Dispose();
            effectByteCode.Dispose();
        }

        public void Apply()
        {
            // if it is a ComputeShader, they have no inputElements
            if (inputLayout != null)
                graphicsDevice.SetInputLayout(inputLayout);

            pass.Apply(graphicsDevice.GetNativeDevice.ImmediateContext);
        }

        public void Dispose()
        {
            Utilities.Dispose(ref pass);
            Utilities.Dispose(ref effect);
            Utilities.Dispose(ref inputLayout);

        }
    }
}
