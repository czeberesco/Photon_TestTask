using System.Collections.Generic;
using System.Threading.Tasks;
using Fusion;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;
using UnityEngine.ResourceManagement.ResourceProviders;
using WebSocketSharp;

namespace Utils
{
	public static class AddressableUtils
	{
		#region PublicMethods

		public static async Task<NetworkSceneInfo> GetNetworkSceneInfoFromAssetReference(AssetReference assetReference)
		{
			string scenePath = await GetScenePath(assetReference);

			return GetNetworkSceneInfoFromPath(scenePath);
		}

		public static NetworkSceneInfo GetNetworkSceneInfoFromPath(string path)
		{
			SceneRef sceneRef = default;

			if (path.IsNullOrEmpty())
			{
				Debug.LogError("Path for provided asset reference is invalid");
			}
			else
			{
				Debug.Log($"Scene path successfully acquired. Path: \"{path}\"");
				sceneRef = SceneRef.FromPath(path);
			}

			var sceneInfo = new NetworkSceneInfo();

			if (sceneRef.IsValid)
			{
				sceneInfo.AddSceneRef(sceneRef);
			}

			return sceneInfo;
		}

		public static async Task<string> GetScenePath(AssetReference assetReference)
		{
			AsyncOperationHandle<IList<IResourceLocation>> handle = Addressables.LoadResourceLocationsAsync(assetReference, typeof(SceneInstance));

			await handle.Task;

			string result = handle.Status == AsyncOperationStatus.Failed || handle.Result.Count == 0 ? string.Empty : handle.Result[0].PrimaryKey;

			handle.Release();

			if (string.IsNullOrEmpty(result))
			{
				Debug.LogError($"Failed to retrieve path from asset reference: {assetReference.AssetGUID}");

				return string.Empty;
			}

			return result;
		}

		#endregion
	}
}
