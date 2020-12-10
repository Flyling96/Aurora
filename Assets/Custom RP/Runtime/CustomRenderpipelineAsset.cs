using UnityEngine;
using UnityEngine.Rendering;

[CreateAssetMenu(menuName ="Rendering/Custom Render Pipeline")]
public class CustomRenderpipelineAsset : RenderPipelineAsset
{


    protected override RenderPipeline CreatePipeline()
    {
        return new CustomRenderPipeline();
    }

}
