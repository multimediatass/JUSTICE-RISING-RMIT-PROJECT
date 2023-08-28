using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tproject.Tools
{
    // Creator Instagram: @shantaufiq

    public class GOMod : MonoBehaviour
    {
        public static bool RemoveGOChildren(Transform parent)
        {
            // Iterasi melalui semua child GameObject
            for (int i = parent.childCount - 1; i >= 0; i--)
            {
                Transform child = parent.GetChild(i);
                // Hapus child GameObject
                Destroy(child.gameObject);
            }

            return true;
            // Debug.Log($"Clear {parent.gameObject.name} 's childern");
        }
    }
}
