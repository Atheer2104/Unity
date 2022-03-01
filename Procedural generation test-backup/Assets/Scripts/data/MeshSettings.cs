using UnityEngine;
using System.Collections;

[CreateAssetMenu()]
public class MeshSettings : UpdatableData {

	// 0, 1, 2, 3, 4
	public const int numSupportedLODs = 5;
	// length of supportedChunkSizes
	public const int numSupportedChunkSizes = 9;
	public const int numSupportedFlatshadedChunkSizes = 3;
	// the size of chunks that we can use that divisble by our supportedLevelOfDetails
	public static readonly int[] supportedChunkSizes = {48,72,96,120,144,168,192,216,240};
	
	public float meshScale = 2.5f;
	public bool useFlatShading;

	[Range(0,numSupportedChunkSizes-1)]
	public int chunkSizeIndex;
	[Range(0,numSupportedFlatshadedChunkSizes-1)]
	public int flatshadedChunkSizeIndex;


	// NumberOfVerticesPerLine of mesh rendered at LevelOfDetail = 0
    // includes the 2 extra vertices that are excluded from final mesh
    // but used for calculating normals 
	public int numVertsPerLine {
		get {
			return supportedChunkSizes [(useFlatShading) ? flatshadedChunkSizeIndex : chunkSizeIndex] + 5;
		}
	}

	public float meshWorldSize {
		get {
			return (numVertsPerLine - 3) * meshScale;
		}
	}


}