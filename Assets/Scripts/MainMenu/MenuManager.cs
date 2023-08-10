using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JusticeRising
{
    public class MenuManager : MonoBehaviour
    {
        int skinIndex = 0;
        [SerializeField] private List<GameObject> playerSkin = new List<GameObject>();

        private void Start()
        {
            CheckSkinIndex();
        }

        public void Click_Next()
        {
            skinIndex = skinIndex < playerSkin.Count - 1 ? skinIndex + 1 : 0;
            // Debug.Log($"this right: {skinIndex}");
            CheckSkinIndex();
        }

        public void Click_Prev()
        {
            skinIndex = skinIndex > 0 ? skinIndex - 1 : playerSkin.Count - 1;
            // Debug.Log($"this left: {skinIndex}");
            CheckSkinIndex();
        }

        public void CheckSkinIndex()
        {
            for (int i = 0; i < playerSkin.Count; i++)
            {
                if (i == skinIndex)
                    playerSkin[i].SetActive(true);
                else
                    playerSkin[i].SetActive(false);
            }
        }
    }
}
