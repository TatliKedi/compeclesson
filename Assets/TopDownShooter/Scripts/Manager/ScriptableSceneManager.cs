﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using TopDownShooter.Network;
using System;
using UnityEngine.SceneManagement;

namespace TopDownShooter
{
    [CreateAssetMenu(menuName = "Topdown Shooter/Manager/ScriptableSceneManager")]
    public class ScriptableSceneManager : AbstractScriptableManager<ScriptableSceneManager>
    {

        [SerializeField] private string _menuScene;
        [SerializeField] private string _gameScene;
        public override void Initialize()
        {
            base.Initialize();
            SceneManager.LoadScene(_menuScene);
            MessageBroker.Default.Receive<EventPlayerNetworkStateChange>().Subscribe(OnPlayerNetworkState).AddTo(_compositeDisposable);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }


        public override void Destroy()
        {
            base.Destroy();
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
        {
            MessageBroker.Default.Publish(new EventSceneLoaded(arg0.name));
        }

        private void OnPlayerNetworkState(EventPlayerNetworkStateChange obj)
        {
            //when network state change
            Debug.Log("NETWORK STATE CHANGE ON SCENE MANAGER TO : " + obj.PlayerNetworkState);
            switch (obj.PlayerNetworkState)
            {
                case PlayerNetworkState.Offline:
                    break;
                case PlayerNetworkState.Connecting:
                    break;
                case PlayerNetworkState.Connected:
                    break;
                case PlayerNetworkState.JoiningRoom:
                    break;
                case PlayerNetworkState.LeavingRoom:
                    SceneManager.LoadScene(_menuScene);
                    break;
                case PlayerNetworkState.InRoom:
                    PhotonNetwork.isMessageQueueRunning = false;
                    SceneManager.LoadScene(_gameScene);
                    break;
                default:
                    break;
            }
        }
    }
}