
设置boot.config
以下2个参数比较重要
job-worker-count=5
gc-max-time-slice=3

# 在任务管理器中,  内存(提交大小>8.9G时 会卡屯)

gfx-enable-gfx-jobs=1
gfx-enable-native-gfx-jobs=1
gfx-disable-mt-rendering=1
wait-for-native-debugger=0
vr-enabled=0
hdr-display-enabled=0
gc-max-time-slice=10
job-worker-count=5
single-instance=
memorysetup-main-allocator-block-size=33554432


"memorysetup-bucket-allocator-granularity=16"
"memorysetup-bucket-allocator-bucket-count=8"
"memorysetup-bucket-allocator-block-size=4194304"
"memorysetup-bucket-allocator-block-count=1"
"memorysetup-main-allocator-block-size=16777216"
"memorysetup-thread-allocator-block-size=16777216"
"memorysetup-gfx-main-allocator-block-size=16777216"
"memorysetup-gfx-thread-allocator-block-size=16777216"
"memorysetup-cache-allocator-block-size=4194304"
"memorysetup-typetree-allocator-block-size=2097152"
"memorysetup-profiler-bucket-allocator-granularity=16"
"memorysetup-profiler-bucket-allocator-bucket-count=8"
"memorysetup-profiler-bucket-allocator-block-size=4194304"
"memorysetup-profiler-bucket-allocator-block-count=1"
"memorysetup-profiler-allocator-block-size=16777216"
"memorysetup-profiler-editor-allocator-block-size=1048576"
"memorysetup-temp-allocator-size-main=4194304"
"memorysetup-job-temp-allocator-block-size=2097152"
"memorysetup-job-temp-allocator-block-size-background=1048576"
"memorysetup-job-temp-allocator-reduction-small-platforms=262144"
"memorysetup-allocator-temp-initial-block-size-main=262144"
"memorysetup-allocator-temp-initial-block-size-worker=262144"
"memorysetup-temp-allocator-size-background-worker=32768"
"memorysetup-temp-allocator-size-job-worker=262144"
"memorysetup-temp-allocator-size-preload-manager=262144"
"memorysetup-temp-allocator-size-nav-mesh-worker=65536"
"memorysetup-temp-allocator-size-audio-worker=65536"
"memorysetup-temp-allocator-size-cloud-worker=32768"
"memorysetup-temp-allocator-size-gfx=262144"
Segmentation fault (core dumped)


My boot.config looks like:

gfx-enable-gfx-jobs=1
gfx-enable-native-gfx-jobs=1
wait-for-native-debugger=0
hdr-display-enabled=0
gc-max-time-slice=3
build-guid=89ed41df3faa40c7b96dd60845653084

Exception while loading mod Yu.PerformanceCapturePatch at D:/Doc/Klei/OxygenNotIncluded/mods/Local/PerformanceLogMod. 
HarmonyLib.HarmonyException: Patching exception in method static System.Boolean 
UnityEngine.Scripting.GarbageCollector::CollectIncremental(System.UInt64 nanoseconds) ---> System.FormatException:
Method static System.Boolean UnityEngine.Scripting.GarbageCollector::CollectIncremental(System.UInt64 nanoseconds) cannot be patched. Reason: 
Invalid IL code in (wrapper dynamic-method) UnityEngine.Scripting.GarbageCollector:UnityEngine.Scripting.GarbageCollector.CollectIncremental_Patch1 (ulong): IL_000b: unused42  

 