/*
 *Written by Maxi Levi <maxilevi@live.com>, November 2017
*/

using System;
using System.Linq;
using System.Threading;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

namespace Assets.Generation
{
    public class ChunkLoader : NetworkBehaviour
    {
        [SyncVar]
        public Vector3 Offset;
        [SyncVar]
        public bool Enabled = true;
        public GameObject Player;
        public World World;
        [SyncVar]
        private int _activeChunks;
        [SyncVar]
        private float _targetMin = 1;
        [SyncVar]
        private float _targetMax = 1;
        [SyncVar]
        private float _minFog;
        [SyncVar]
        private float _maxFog;
        [SyncVar]
        private float _left = 0;
        [SyncVar]
        private Vector3 _lastOffset = new Vector3(float.MinValue, float.MinValue, float.MinValue);
        [SyncVar]
        private int _prevChunkCount;
        [SyncVar]
        private float _lastRadius;
        [SyncVar]
        private Vector3 _playerPosition, _position;
        private Thread _t1, _t2;
        [SyncVar]
        private bool Stop;

        //[ServerCallback]
        void Awake()
        {
            //Debug.Log(_lastOffset.ToString());
            World = GameObject.FindGameObjectWithTag("World").GetComponent<World>();
            StartCoroutine(this.LoadChunks());
            StartCoroutine(this.ManageChunksMesh());
            //Debug.Log(_lastOffset.ToString());
            Debug.Log("Mapa Cargado :)");
        }

        void OnApplicationQuit()
        {
            Stop = true;
        }

        void Update()
        {
            _playerPosition = Player.transform.position;
            _position = transform.position;
        }

        private IEnumerator LoadChunks()
        {
            while (true)
            {
                //break;
                if (Stop) break;


                if (!Enabled || !World.Loaded)
                    goto SLEEP;

                Offset = World.ToChunkSpace(_playerPosition);

                if (Offset != _lastOffset)
                {

                    for (int _x = -World.ChunkLoaderRadius / 2; _x < World.ChunkLoaderRadius / 2; _x++)
                    {
                        //yield return null;
                        for (int _z = -World.ChunkLoaderRadius / 2; _z < World.ChunkLoaderRadius / 2; _z++)
                        {
                            for (int _y = -World.ChunkLoaderRadius / 2; _y < World.ChunkLoaderRadius / 2; _y++)
                            {
                                int x = _x, y = _y, z = _z;

                                if (World.GetChunkByOffset(Offset + Vector3.Scale(new Vector3(x, y, z), new Vector3(Chunk.ChunkSize, Chunk.ChunkSize, Chunk.ChunkSize))) == null)
                                {
                                    Vector3 chunkPos = Offset + Vector3.Scale(new Vector3(x, y, z), new Vector3(Chunk.ChunkSize, Chunk.ChunkSize, Chunk.ChunkSize));
                                    GameObject NewChunk = new GameObject("Chunk " + (chunkPos.x) + " " + (chunkPos.y) + " " + (chunkPos.z));
                                    NewChunk.transform.position = chunkPos;
                                    NewChunk.transform.SetParent(World.gameObject.transform);
                                    Chunk chunk = NewChunk.AddComponent<Chunk>();
                                    chunk.Init(chunkPos, World);
                                    chunk.Lod = 2;
                                    World.AddChunk(chunkPos, chunk);
                                }
                            }
                        }
                    }
                    _lastRadius = Options.ChunkLoaderRadius;
                    _lastOffset = Offset;
                    World.SortGenerationQueue();
                }
            SLEEP:
                yield return null;
            }
        }

        private IEnumerator ManageChunksMesh()
        {
            while (true)
            {
                if (Stop) break;

                yield return null;


                Chunk[] Chunks;
                lock (World.Chunks)
                    Chunks = World.Chunks.Values.ToList().ToArray();

                bool AddedNew = false;
                for (int i = Chunks.Length - 1; i > -1; i--)
                {

                    if (Chunks[i].Disposed)
                    {
                        continue;
                    }

                    if ((Chunks[i].Position - _playerPosition).sqrMagnitude > (Options.ChunkLoaderRadius) * .5f * Chunk.ChunkSize * (Options.ChunkLoaderRadius) * Chunk.ChunkSize * .5f)
                    {
                        World.RemoveChunk(Chunks[i]);
                        continue;
                    }

                    if (Chunks[i].ShouldBuild && Chunks[i].IsGenerated && !World.ContainsMeshQueue(Chunks[i]) && Chunks[i].NeighboursExists)
                    {
                        World.AddToQueue(Chunks[i], true);
                        AddedNew = true;
                    }
                }
                //if (AddedNew)
                //	World.SortMeshQueue ();
            }
        }
    }
}
