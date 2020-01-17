using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewFinder : MonoBehaviour
{

    public enum ColorDepth {
        Depth0 = 0,
        Depth16 = 16,
        Deph24 = 24
    };

    public int textureWidth = 1024;
    public ColorDepth colorDepth = ColorDepth.Depth16;
    public RenderTextureFormat textureFormat = RenderTextureFormat.ARGB32;

    public Camera vfCamera;
    public GameObject vfCameraFov;
    public GameObject vfPlane;

    private RenderTexture texture;

    void Start()
    {

        int width = vfCamera.pixelWidth;
        int height = vfCamera.pixelHeight;

        if (width + height == 0) {
            Debug.Log($"Camera size is {width}x{height}, not doing the thing");
            return;
        }

        float ar = (float)height / width;

        int tWidth = textureWidth;
        int tHeight = (int)(tWidth * ar);

        this.texture = new RenderTexture(tWidth, tHeight, (int)colorDepth, textureFormat);

        this.vfPlane.transform.localScale = new Vector3(
            1,
            ar,
            1
        );

        this.vfCameraFov.transform.localScale = new Vector3(
            1,
            1,
            ar
        );

        this.vfCamera.targetTexture = this.texture;

        Renderer planeRenderer = this.vfPlane.GetComponent<Renderer>();
        Material mat = planeRenderer.material;
        // mat.SetTexture("_BaseMap", this.texture);
        mat.mainTexture = this.texture;

    }

}
