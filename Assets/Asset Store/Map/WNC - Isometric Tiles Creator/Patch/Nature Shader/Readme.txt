In our trailer video we use shader for nature objects. At the time of publication, this shader is available in the asset store and is free. Developer of this shader: Nicrom (https://assetstore.unity.com/publishers/12903)
Link to shader asset: https://assetstore.unity.com/packages/vfx/shaders/low-poly-wind-182586
You need to download this shader and install to your project with WNC - Isometric Tiles Creator. After that, you need to patch shader via patches, which included in asset. And after that you need to follow instruction below, to replace nature prefabs of WNC - Isometric Tiles Creator with new prefabs, on which already seted material with nature shader.

To install a patch for the Unity3D asset that will change the materials for your render pipeline, follow these steps:

Open your Unity project.
In the "Assets" menu, select "Import Package" and then "Custom Package."
Select the patch unitypackage file you want to install.
In the import window, select the materials you want to replace, and then click "Import."
The materials will be replaced in your project and the asset should now use the new materials with your render pipeline.

Note: These instructions assume that you have already imported the original asset into your Unity project and that the patch unitypackage is compatible with the version of the asset you are using.