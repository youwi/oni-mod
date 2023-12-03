


2个Hack 是替换用的,仅仅提供UI,建造成功以后就替换成原生的。 用的Hack展示UI,

原生动画没用 UI标记.所以新做了

建造异常:
NullReferenceException: Object reference not set to an instance of an object

ConduitSecondaryOutput.HasSecondaryConduitType (ConduitType type) (at <e519dd73da9048a8894ab7f073f3ead7>:0)

 NullReferenceException: Object reference not set to an instance of an object

拆队时崩溃
NullReferenceException: Object reference not set to an instance of an object
Deconstructable.OnCompleteWork (Worker worker) (at <e519dd73da9048a8894ab7f073f3ead7>:0)
 
NullReferenceException: Object reference not set to an instance of an object

Deconstructable.OnCompleteWork (Worker worker) (at <e519dd73da9048a8894ab7f073f3ead7>:0)
Workable.CompleteWork (Worker worker) (at <e519dd73da9048a8894ab7f073f3ead7>:0)
Worker.CompleteWork () (at <e519dd73da9048a8894ab7f073f3ead7>:0)
(wrapper dynamic-method) Worker.Worker.Work_Patch1(Worker,single)

(wrapper dynamic-method) Game.Game.Update_Patch2(Game)

No receiver world found for warp portal sender

  at UnityEngine.Debug.LogError (System.Object message) [0x00000] in <72b60a3dd8cd4f12a155b761a1af9144>:0 
  at Debug.LogError (System.Object obj) [0x00000] in <1e2b7d5db95c4d6b84eabb7ca0270927>:0 
  at WarpPortal.GetTargetWorldID () [0x00000] in <e519dd73da9048a8894ab7f073f3ead7>:0 
  at WarpPortal.WarpPortal.Discover_Patch1 (WarpPortal ) [0x00000] in <e519dd73da9048a8894ab7f073f3ead7>:0 
  at WarpPortal.OnObjectSelected (System.Object data) [0x00000] in <e519dd73da9048a8894ab7f073f3ead7>:0 
  at EventSystem.Trigger (UnityEngine.GameObject go, System.Int32 hash, System.Object data) [0x00000] in <1e2b7d5db95c4d6b84eabb7ca0270927>:0 
 
Build: U50-583750-SD