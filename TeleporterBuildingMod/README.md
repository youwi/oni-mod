


2个Hack 是替换用的,仅仅提供UI,建造成功以后就替换成原生的。 用的Hack展示UI,

原生动画没用 UI标记.所以新做了

建造异常:
NullReferenceException: Object reference not set to an instance of an object

ConduitSecondaryOutput.HasSecondaryConduitType (ConduitType type) (at <e519dd73da9048a8894ab7f073f3ead7>:0)

 NullReferenceException: Object reference not set to an instance of an object
 拆队时崩溃

Deconstructable.OnCompleteWork (Worker worker) (at <e519dd73da9048a8894ab7f073f3ead7>:0)
Deconstructable.QueueDeconstruction (System.Boolean userTriggered) (at <e519dd73da9048a8894ab7f073f3ead7>:0)
Deconstructable.QueueDeconstruction () (at <e519dd73da9048a8894ab7f073f3ead7>:0)
Deconstructable.OnDeconstruct () (at <e519dd73da9048a8894ab7f073f3ead7>:0)
UserMenu+<>c__DisplayClass15_0.<AddButton>b__0 () (at <e519dd73da9048a8894ab7f073f3ead7>:0)
KButton.SignalClick (KKeyCode btn) (at <1e2b7d5db95c4d6b84eabb7ca0270927>:0)
KButton.OnPointerClick (UnityEngine.EventSystems.PointerEventData eventData) (at <1e2b7d5db95c4d6b84eabb7ca0270927>:0)
UnityEngine.EventSystems.ExecuteEvents.Execute (UnityEngine.EventSystems.IPointerClickHandler handler, UnityEngine.EventSystems.BaseEventData eventData) (at <e9635660f69b4d9d84c085aeda3be353>:0)
UnityEngine.EventSystems.ExecuteEvents.Execute[T] (UnityEngine.GameObject target, UnityEngine.EventSystems.BaseEventData eventData, UnityEngine.EventSystems.ExecuteEvents+EventFunction`1[T1] functor) (at <e9635660f69b4d9d84c085aeda3be353>:0)
UnityEngine.EventSystems.EventSystem:Update()

Build: U50-583750-SD