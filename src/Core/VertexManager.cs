using System;
using System.Diagnostics;

namespace Core
{
    public struct Pair<T>
    {
        public T First, Second;

        public T this[int index]
        {
            get { return index==0 ? First : Second; }
            set
            {
                if (index==0)
                    First = value;
                else
                    Second = value;
            }
        }

        public void Reset() { First = Second = default(T); }
    }

    public class VertexManager<TEdge> : EdgePath<TEdge>
        where TEdge : IGraphEdge
    {
        protected struct IndexVertex
        {
            public int PathId;
            public Vertex<TEdge> Vertex;

            public void Reset()
            {
                PathId = 0;
                Vertex = null;
            }
        };
        // manager data
        protected VertexAllocator<TEdge> Allocator;
        protected int MaxVertexCount;
        protected IndexVertex[] Indices;
        // priority queue data
        private Pair<Vertex<TEdge>> listData;
        private Vertex<TEdge> listHead;
        private Vertex<TEdge> listTail;
        private double maxDistance = -1;
        private double minBucketValue = 0;
        private double maxBucketValue = 1000;
        private Vector<Vertex<TEdge>> buckets;
        private int minBucketId;
        private bool clearBuckets;

        public VertexManager(int vertexCount, int bucketCount, bool clearBuckets = false)
        {
            Allocator = new VertexAllocator<TEdge>(vertexCount);
            MaxVertexCount = vertexCount;
            Indices = new IndexVertex[vertexCount];
            buckets = new Vector<Vertex<TEdge>>(bucketCount);
            buckets.Count = bucketCount;
            this.clearBuckets = clearBuckets;
        }
        public override void Initialize()
        {
            base.Initialize();
            Allocator.Initialize();
            CurrentPathId++;
            if (CurrentPathId==0)
            {
                for (int i = 0; i<Indices.Length; i++)
                    Indices[i].Reset();
                CurrentPathId++;
            }
            listData.First = new Vertex<TEdge>();
            listData.Second = new Vertex<TEdge>();
            listHead = listData[0];
            listTail = listData[1];
            listHead.Next = listTail;
            listTail.F = maxDistance;
            listTail.Prev = listHead;
            minBucketId = buckets.Count;
            if (clearBuckets)
                buckets.Data.Initialize();
        }
        public bool IsOpen(Vertex<TEdge> vertex)
        { return vertex.Open!=0; }
        public bool IsVisited(int vertexId)
        {
            Debug.Assert(vertexId<MaxVertexCount);
            return Indices[vertexId].PathId==CurrentPathId;
        }
        public bool IsClosed(Vertex<TEdge> vertex)
        { return IsVisited(vertex.Index) && !IsOpen(vertex); }
        public Vertex<TEdge> GetVertex(int index)
        {
            Debug.Assert(index<MaxVertexCount);
            Debug.Assert(IsVisited(index));
            return Indices[index].Vertex;
        }
        public int VisitedVertexCount
        { get { return Allocator.VisitedVertexCount; } }
        public Vertex<TEdge> CreateVertex()
        { return Allocator.CreateVertex(); }
        public Vertex<TEdge> CreateVertex(Vertex<TEdge> vertex, int index)
        {
            Debug.Assert(index<MaxVertexCount);
            Indices[index].Vertex = vertex;
            Indices[index].PathId = CurrentPathId;
            vertex.Index = index;
            return vertex;
        }
        public Vertex<TEdge> CreateVertex(int index)
        { return CreateVertex(Allocator.CreateVertex(), index); }
        public void AddOpen(Vertex<TEdge> vertex)
        {
            vertex.Open = 1;
            AddToBucket(vertex, ComputeBucketId(vertex));
            VerifyBuckets();
        }
        public void AddClosed(Vertex<TEdge> vertex)
        { vertex.Open = 0; }
        public int CurrentPathId { get; protected set; }
        
        public void AddBestClosed()
        {
            Debug.Assert(!IsOpenEmpty());
            AddClosed(buckets[minBucketId]);
        }
        public bool IsOpenEmpty()
        {
            if (minBucketId==buckets.Count)
                return true;
            if (buckets[minBucketId]==null)
            {
                minBucketId++;
                if (!clearBuckets)
                {
                    while (minBucketId<buckets.Count)
                    {
                        var bucket = buckets[minBucketId];
                        if (bucket==null || bucket.PathId!=CurrentPathId ||
                            bucket.BucketId!=minBucketId)
                        {
                            minBucketId++;
                            continue;
                        }
                        break;
                    }
                }
                else
                {
                    while (minBucketId<buckets.Count && buckets[minBucketId]==null)
                        minBucketId++;
                }
                return minBucketId>=buckets.Count;
            }
            return false;
        }
        public int ComputeBucketId(Vertex<TEdge> vertex)
        {
            double dist = vertex.F;
            if (dist>=maxBucketValue)
                return buckets.Count-1;
            if (dist<=minBucketValue)
                return 0;
            double id = buckets.Count*(dist-minBucketValue)/(maxBucketValue-minBucketValue);
            return (int)id;
        }
        public void VerifyBuckets()
        {}
        public void AddToBucket(Vertex<TEdge> vertex, int bucketId)
        {
            if (bucketId<minBucketId)
                minBucketId = bucketId;
            Vertex<TEdge> i = buckets[bucketId];
            if (i==null || !clearBuckets && (i.PathId!=CurrentPathId || i.BucketId!=bucketId))
            {
                vertex.BucketId = bucketId;
                vertex.PathId = CurrentPathId;
                buckets[bucketId] = vertex;
                vertex.Next = vertex.Prev = null;
                VerifyBuckets();
                return;
            }
            vertex.BucketId = bucketId;
            vertex.PathId = CurrentPathId;
            if (i.F>=vertex.F)
            {
                buckets[bucketId] = vertex;
                vertex.Next = i;
                vertex.Prev = null;
                i.Prev = vertex;
                VerifyBuckets();
                return;
            }
            if (i.Next==null)
            {
                vertex.Prev = i;
                vertex.Next = null;
                i.Next = vertex;
                VerifyBuckets();
                return;
            }
            for (i = i.Next; i.Next!=null; i = i.Next)
            {
                if (i.F>=vertex.F)
                {
                    vertex.Next = i;
                    vertex.Prev = i.Prev;
                    i.Prev.Next = vertex;
                    i.Prev = vertex;
                    VerifyBuckets();
                    return;
                }
            }
            if (i.F>=vertex.F)
            {
                vertex.Next = i;
                vertex.Prev = i.Prev;
                i.Prev.Next = vertex;
                i.Prev = vertex;
                VerifyBuckets();
                return;
            }
            else
            {
                vertex.Next = null;
                vertex.Prev = i;
                i.Next = vertex;
                VerifyBuckets();
                return;
            }
        }
        public void DecreaseOpen(Vertex<TEdge> vertex, double value)
        {
            Debug.Assert(!IsOpenEmpty());
            int vertexBucketId = ComputeBucketId(vertex);
            if (vertex.Prev!=null)
                vertex.Prev.Next = vertex.Next;
            else
            {
                Debug.Assert(buckets[vertex.BucketId]==vertex);
                buckets[vertex.BucketId] = vertex.Next;
            }
            if (vertex.Next!=null)
                vertex.Next.Prev = vertex.Prev;
            VerifyBuckets();
            AddToBucket(vertex, vertexBucketId);
            VerifyBuckets();
        }
        public void RemoveBestOpen()
        {
            Debug.Assert(!IsOpenEmpty());
            VerifyBuckets();
            Debug.Assert(buckets[minBucketId]!=null && IsVisited(buckets[minBucketId].Index));
            buckets[minBucketId] = buckets[minBucketId].Next;
            if (buckets[minBucketId]!=null)
                buckets[minBucketId].Prev = null;
            VerifyBuckets();
        }
        public Vertex<TEdge> GetBest()
        {
            Debug.Assert(!IsOpenEmpty());
            return buckets[minBucketId];
        }
        public void SetMinBucketValue(double value)
        { minBucketValue = value; }
        public void SetMaxBucketValue(double value)
        { maxBucketValue = value; }
    }
}
