

kanimal-cli 需要安装.net 3.0 (不是3.5,安装3.5没用)
这里显示了一些脚本.

//生成SCML
./kanimal-cli.exe scml doors/door_external_0.png  doors/door_external_anim.bytes doors/door_external_build.bytes -o door_external_scml
./kanimal-cli.exe scml doors/door_internal_0.png  doors/door_internal_anim.bytes doors/door_internal_build.bytes -o door_internal_scml
./kanimal-cli.exe scml doors/door_manual_0.png    doors/door_manual_anim.bytes   doors/door_manual_build.bytes   -o door_manual_scml


//从SCML生成动画
./kanimal-cli.exe kanim door_external_scml/door_external.scml -o door_external_beauty
md /r ../GoodDoorMod/resource/anim\assets/door_external_beauty
copy door_external_beauty/* ../GoodDoorMod/resource/anim\assets/door_external_beauty

md /r ../GoodDoorMod/resource/anim\assets/door_external_beauty


./kanimal-cli.exe kanim door_external_scml/door_external.scml -o ../GoodDoorMod/resource/anim\assets/door_external_beauty
./kanimal-cli.exe kanim door_manual_scml/door_manual.scml -o ../GoodDoorMod/resource/anim\assets/door_manual_beauty
./kanimal-cli.exe kanim door_internal_scml/door_internal.scml -o ../GoodDoorMod/resource/anim\assets/door_internal_beauty

VS中使用的脚本有些不一样:
.\..\anim-project/kanimal-cli.exe kanim ../anim-project/door_external_scml/door_external.scml -o resource/anim\assets/door_external_beauty
.\..\anim-project/kanimal-cli.exe kanim ../anim-project/door_manual_scml/door_manual.scml -o  resource/anim\assets/door_manual_beauty
.\..\anim-project/kanimal-cli.exe kanim ../anim-project/door_internal_scml/door_internal.scml -o  resource/anim\assets/door_internal_beauty

