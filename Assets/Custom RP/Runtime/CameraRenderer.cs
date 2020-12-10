using UnityEngine;
using UnityEngine.Rendering;

public class CameraRenderer
{
    const string bufferName = "Render Camera";

    CommandBuffer buffer = new CommandBuffer
    {
        name = bufferName
    };

    ScriptableRenderContext context;
    CullingResults cullingResults;

    static ShaderTagId unlitShaderTagID = new ShaderTagId("SRPDefaultUnlit");

    Camera camera;

    public void Render(ScriptableRenderContext context, Camera camera)
    {
        this.context = context;
        this.camera = camera;

        if (!Cull())
        {
            return;
        }

        Setup();
        DrawVisibleGeomertry();
        Submit();
    }

    bool Cull()
    {
        ScriptableCullingParameters p;
        if(camera.TryGetCullingParameters(out p))
        {
            cullingResults = context.Cull(ref p);
            return true;
        }
        return false;
    }

    void Setup()
    {
        context.SetupCameraProperties(camera);
        buffer.ClearRenderTarget(true, true, Color.clear);
        //BeginSample 与 EndSample成对使用，为了在FrameDebugger中有层级显示
        buffer.BeginSample(bufferName);
        ExecuteBuffer();
    }

    void DrawVisibleGeomertry()
    {
        var sortingSettings = new SortingSettings(camera);
        var drawingSettings = new DrawingSettings(unlitShaderTagID,sortingSettings);
        var filteringSettings = new FilteringSettings(RenderQueueRange.all);

        context.DrawRenderers(cullingResults, ref drawingSettings, ref filteringSettings);

        context.DrawSkybox(camera);
    }

    void Submit()
    {
        buffer.EndSample(bufferName);
        ExecuteBuffer();
        context.Submit();
    }

    void ExecuteBuffer()
    {
        context.ExecuteCommandBuffer(buffer);
        buffer.Clear();
    }
}
