using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace MagmaHeart.Core.Dungeon
{
    public class LocationRenderer : MonoBehaviour
    {
        [SerializeField] private int m_tilesPerFrame = 256;
        private HashSet<Tilemap> m_usedTilemaps = new HashSet<Tilemap>();

        public Action RenderedAllTiles;

        private struct RendererJob
        {
            public HashSet<DungeonTile> tiles;
            public Tilemap tilemap;
            public TileBase tileBase;

            public RendererJob(Tilemap tilemap, HashSet<DungeonTile> tiles, TileBase tileBase)
            {
                this.tiles = tiles;
                this.tilemap = tilemap;
                this.tileBase = tileBase;
            }
        }

        private List<RendererJob> m_jobs = new List<RendererJob>();
        private int m_completedJobs = 0;

        public void AddTilesToDraw(HashSet<DungeonTile> tiles, Tilemap tilemap, TileBase tileBase)
        {
            m_usedTilemaps.Add(tilemap);
            RendererJob job = new RendererJob(tilemap, tiles, tileBase);
            m_jobs.Add(job);
        }

        public void DrawTiles()
        {
            foreach (RendererJob job in m_jobs)
                StartCoroutine(DrawTiles(job));
        }

        private IEnumerator DrawTiles(RendererJob job)
        {
            int renderedTiles = 0;
            foreach (DungeonTile tile in job.tiles)
            {
                Vector3Int tilePosition = job.tilemap.WorldToCell((Vector3Int)tile.Position);
                job.tilemap.SetTile(tilePosition, job.tileBase);

                ++renderedTiles;

                if (renderedTiles % m_tilesPerFrame == 0)
                    yield return new WaitForEndOfFrame();
            }

            ++m_completedJobs;
            if (m_completedJobs == m_jobs.Count)
            {
                m_jobs.Clear();
                m_completedJobs = 0;
                RenderedAllTiles?.Invoke();
            }
        }

        public void Clear()
        {
            foreach (Tilemap tilemap in m_usedTilemaps)
                tilemap.ClearAllTiles();
        }
    }
}