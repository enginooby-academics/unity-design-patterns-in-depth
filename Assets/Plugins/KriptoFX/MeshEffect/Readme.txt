My email is "kripto289@gmail.com"
You can contact me for any questions.

My English is not very good, and if there are any translation errors, you can let me know :)


The package includes prefabs of mesh effects and demo scene for pc/mobiles with characters and environment.

This simple tutorial only for URP rendering. 

------------------------------------------------------------------------------------------------------------------------------------------

NOTE !!!!


	1) Unity does NOT supported multiple submeshes with multiple materials to each submesh. 1 submesh = 1 material!
	For example, if your character model with 1 big mesh (head/hands/body/etc) but with different materials per each part (head/hands/body/etc).
	You can not add new material (for example lightning material) to all parts of character. 
	So, if your model have more then 2 submeshes, then material will be added only for last submesh. (for example only to head but not for hands and body)
	
	You must use splitted meshes. There is no performance or draw calls difference. But then you can use additional materials for all meshes.



------------------------------------------------------------------------------------------------------------------------------------------

For correct effects working you must:

	
1) Use included postprocessing profile "\Assets\KriptoFX\MeshEffect\PostProcess Profile.asset" (If you want effects which look like in the video example)
2) For main camera you need set "Depth texture = ON" and "Opaque texture = ON"

------------------------------------------------------------------------------------------------------------------------------------------

Effect USING:


In editor mode:

	1) Just drag & drop prefab to your object (a prefab of effect should be as child to your mesh). 
	2) Set this object to the property "Mesh Object" of script "PSMeshRendererUpdater".
	3) Click "Update Mesh Renderer".
	Particles and materials will be added to your object automatically. But if you want, you can add material and particles manually. 


In runtime mode:

	var currentInstance = Instantiate(Effect) as GameObject; 
	var psUpdater = currentInstance.GetComponent<PSMeshRendererUpdater>();
	psUpdater.UpdateMeshEffect(MeshObject);


For SCALING just change transform scale of mesh or use "StartScaleMultiplier" of script "PSMeshRendererUpdater"

------------------------------------------------------------------------------------------------------------------------------------------
Supported platforms:

	PC/Consoles/VR/Mobiles with directx9/11, opengles 2.0/3.0 and gamma/linear color space
	Supported SRP rendering. LightWeight render pipeline (LWRP) and HighDefinition render pipeline (HDRP)
	All effects tested on Oculus Rift CV1 with single and dual mode rendering and works perfect. 

------------------------------------------------------------------------------------------------------------------------------------------


If you have some questions, you can write me to email "kripto289@gmail.com" 