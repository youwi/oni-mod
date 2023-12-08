using PerformanceLogMod;
using System;
using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.Scripting;

public class GCManualControlS : MonoBehaviour
{
    // Perform an incremental collection every time we allocate more than 8 MB
    const long kCollectAfterAllocating = 128 * 1024 * 1024;

    // Perform an instant, full GC if we have more than 128 MB of managed heap.
    const long kHighWater = 4096L * 1024L * 1024L*2;

    long lastFrameMemory = 0;
    long lastGCTime = 0;
    long nextCollectAt = 0;
    void Awake()
    {
        Debug.Log("---> GCManualControlS Awake");

    }

    void Start()
    {
        // Set GC to manual collections only.
        GarbageCollector.GCMode = GarbageCollector.Mode.Manual;
        Debug.Log("----> GCManualControlS 手动处理垃圾init  --<");

        //方案1:
        InvokeRepeating("UpdateMy", 2, 60);//2秒后,每60秒执行. 
        //方案2:直接使用update
    }
    void Update() { }

    void UpdateMy()
    {
        if (GarbageCollector.GCMode == GarbageCollector.Mode.Enabled)
        {
            return;
        }
        long mem = Profiler.GetMonoUsedSizeLong();
        if (mem < lastFrameMemory)
        {
            // GC happened.
            nextCollectAt = mem + kCollectAfterAllocating;
        }
        if (mem > kHighWater)
        {
            // Trigger immediate GC
            Debug.Log($"----> GCManualControlS {mem}M {GC.GetTotalMemory(false)}M >4096M 标记回收垃圾 --<");
            System.GC.Collect(0);
        }
        else if (mem >= nextCollectAt)
        {
            if (Time.time > lastGCTime + 60) //最多60秒执行一次.
            {
                // Trigger incremental GC
                UnityEngine.Scripting.GarbageCollector.CollectIncremental();
                // Debug.Log("----> GCManualControlS 标记回收垃圾B  --<"); 日志太多
                GCAllMyPatches.cache.Add(System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.fff"));
                lastFrameMemory = mem + kCollectAfterAllocating;
                lastGCTime = (long)Time.time;
            }

        }
        //Debug.Log("----> GCManualControlS ....update .... --<");
        lastFrameMemory = mem;

    }
}