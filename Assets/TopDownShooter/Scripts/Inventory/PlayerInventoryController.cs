﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace TopDownShooter.Inventory
{
    public class PlayerInventoryController : MonoBehaviour
    {
        [SerializeField] private AbstractBasePlayerInventoryItemData[] _inventoryItemDataArray;
        private List<AbstractBasePlayerInventoryItemData> _instantiatedItemDataList;
        public Transform BodyParent;
        public Transform CannonParent;

        public ReactiveCommand ReactiveShootCommand { get; private set; }
        public int Id { get; set; }

        private void Start()
        {
            //FOR TESTING PURPOSES ONLY
            InitializeInventory(_inventoryItemDataArray);
        }

        private void OnDestroy()
        {
            ClearInventory();
        }


        public void InitializeInventory(AbstractBasePlayerInventoryItemData[] inventoryItemDataArray)
        {
            if (ReactiveShootCommand != null)
            {
                //adjusting reactive command
                ReactiveShootCommand.Dispose();
            }
            ReactiveShootCommand = new ReactiveCommand();

            //clearing old inventory and creating new one
            ClearInventory();
            _instantiatedItemDataList = new List<AbstractBasePlayerInventoryItemData>(inventoryItemDataArray.Length);
            for (int i = 0; i < inventoryItemDataArray.Length; i++)
            {
                var instantiated = Instantiate(inventoryItemDataArray[i]);
                instantiated.Initialize(this);
                _instantiatedItemDataList.Add(instantiated);
            }
        }

        private void ClearInventory()
        {
            if (_instantiatedItemDataList != null)
            {
                for (int i = 0; i < _instantiatedItemDataList.Count; i++)
                {
                    _instantiatedItemDataList[i].Destroy();
                }
            }
        }


    }
}
