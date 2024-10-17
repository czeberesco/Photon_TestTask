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

		public static async Task<NetworkSceneInfo> GetNetworkSceneInfo(AssetReference assetReference)
		{
			SceneRef sceneRef = default;

			string scenePath = await GetScenePath(assetReference);

			if (scenePath.IsNullOrEmpty())
			{
				Debug.LogError("Path for provided asset reference is invalid");
			}
			else
			{
				Debug.Log($"Scene path successfully acquired. Path: \"{scenePath}\"");
				sceneRef = SceneRef.FromPath(scenePath);
			}

			var sceneInfo = new NetworkSceneInfo();

			if (sceneRef.IsValid)
			{
				sceneInfo.AddSceneRef(sceneRef);
			}

			return sceneInfo;
		}

		#endregion

		#region PrivateMethods

		private static async Task<string> GetScenePath(AssetReference assetReference)
		{
			AsyncOperationHandle<IList<IResourceLocation>> handle = Addressables.LoadResourceLocationsAsync(assetReference, typeof(SceneInstance));

			await handle.Task;

			if (handle.Status == AsyncOperationStatus.Failed || handle.Result.Count == 0)
			{
				Debug.LogError($"Failed to retrieve path from asset reference: {assetReference.AssetGUID}");

				return string.Empty;
			}

			return handle.Result[0].PrimaryKey;
		}

		#endregion
	}
}
