# oni-mod 备注
说明:
	StampTool 图章工具
	TemplateCache  Yaml处理工具

# 其它

	https://code.ecool.dev/ONI-Mods/BuildableNaturalTile
	https://bbs.3dmgame.com/thread-6226681-1-2.html
	https://robophred.github.io/oni-duplicity/#/raw
	https://steamcommunity.com/sharedfiles/filedetails/?id=1883272681

## TODO
	真空变太空
	粒子满了以自动发陨石

## 翻译
PoissonDisk 不规则圆区?
	CreateChore

# 脚本

设置环境 :ONI_MOD_LOCAL  D:\Klei\OxygenNotIncluded\mods\local
```
mkdir  $(ONI_MOD_LOCAL)\$(ProjectName)
copy /y $(TargetDir)$(ProjectName).*  $(ONI_MOD_LOCAL)\$(ProjectName)\
copy /y $(ProjectDir)resource\*  $(ONI_MOD_LOCAL)\$(ProjectName)\
xcopy /y /s $(ProjectDir)resource\*  $(ONI_MOD_LOCAL)\$(ProjectName)\
```
TeleporterBuild 传送器可建造


Space exposure  STATUSITEMS.SPACE  太空暴露  MISC.STATUSITEMS.SPACE

# 动画
https://github.com/skairunner/kanimal-SE
	动画太麻烦了
	AssetStudio 批量提取文件
	得到 TextAsset,Texture2D文件夹
```
cd TextAsset && rename *.prefab *.bytes 
kanimal-cli batch-convert -o .
 
kanimal-cli.exe scml warp_portal_sender_0.png warp_portal_sender_anim.bytes warp_portal_sender_build.bytes -o warp_portal_sender_scml
kanimal-cli.exe scml warp_portal_receiver_0.png warp_portal_receiver_anim.bytes warp_portal_receiver_build.bytes -o warp_portal_receiver_scml
kanimal-cli.exe scml temporal_tear_opener_0.png temporal_tear_opener_anim.bytes temporal_tear_opener_build.bytes -o temporal_tear_opener_scml

kanimal-cli.exe kanim warp_portal_sender/warp_portal_sender.scml -o warp_portal_sender_new
copy warp_portal_receiver.png warp_portal_receiver_anim.bytes warp_portal_receiver_build.bytes ../

kanimal-cli.exe kanim warp_portal_receiver/warp_portal_receiver.scml -o warp_portal_receiver_new
kanimal-cli.exe kanim bomb_build_s/bomb_build_s.scml -o bomb_build_s_new
kanimal-cli.exe kanim bomb_build/bomb_build.scml -o bomb_build_new


```
	printing_pre
	printing_loop
	printing_pst
	idle
	off
	place
	ui

## ClipperLib 
使用这个库切割多边形,太麻烦不搞了.
自带: Voronoi lib可以使用.
PoissonDisk  6边形 圆盘

通知消息
Notification
logical armsidescreen

 
 
clusterCategory:1/2 小行星风格群 (经典和眼冒金星)
 
 AssignClusterLocations 创建星图位置

 DistanceFromTag 这种Tag标记好像在生成世界时按结构走 在标记的相对位置生成

 WorldBorderThickness 生成的中子物质边界大小

 TemplateRules: Tag命令和list对应. 
 位置坐标

 StartMedium
 StartFar
 NearEdge
 NearSurface
 NearDepths 
 AtStart   在开始区生成
 AtSurface 在上层生成
 AtDepths  在下层生成
 AtEdge   在边缘生成(是区块边还是地图边不清)
 
