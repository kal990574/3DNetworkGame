using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.RenderGraphModule;
using UnityEngine.Rendering.RenderGraphModule.Util;
using UnityEngine.Rendering.Universal;

public class ScanPulseRendererFeature : ScriptableRendererFeature
{
    [SerializeField] private Material _scanMaterial;

    private ScanPulseRenderPass _scanPass;

    public override void Create()
    {
        _scanPass = new ScanPulseRenderPass();
        _scanPass.renderPassEvent = RenderPassEvent.AfterRenderingTransparents;
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        if (_scanMaterial == null) return;
        if (renderingData.cameraData.cameraType == CameraType.Preview) return;

        _scanPass.SetMaterial(_scanMaterial);
        renderer.EnqueuePass(_scanPass);
    }

    private class ScanPulseRenderPass : ScriptableRenderPass
    {
        private Material _material;

        public void SetMaterial(Material material)
        {
            _material = material;
        }

        public override void RecordRenderGraph(RenderGraph renderGraph, ContextContainer frameData)
        {
            if (_material == null) return;

            UniversalResourceData resourceData = frameData.Get<UniversalResourceData>();
            TextureHandle source = resourceData.cameraColor;

            TextureDesc desc = renderGraph.GetTextureDesc(source);
            desc.name = "_ScanPulseTemp";
            desc.clearBuffer = false;
            TextureHandle destination = renderGraph.CreateTexture(desc);

            RenderGraphUtils.BlitMaterialParameters blitParams =
                new(source, destination, _material, 0);
            renderGraph.AddBlitPass(blitParams, "ScanPulsePass");

            resourceData.cameraColor = destination;
        }
    }
}