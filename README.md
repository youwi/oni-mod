# oni-mod
说明:
StampTool 图章工具
TemplateCache  Yaml处理工具



# 脚本

设置环境 :ONI_MOD_LOCAL  D:\Klei\OxygenNotIncluded\mods\local
```
		mkdir  $(ONI_MOD_LOCAL)\$(ProjectName)
		copy /y $(TargetDir)$(TargetFileName)  $(ONI_MOD_LOCAL)\$(ProjectName)\$(TargetFileName)
		copy /y $(ProjectDir)resource\*  $(ONI_MOD_LOCAL)\$(ProjectName)\
		xcopy /y /s $(ProjectDir)resource\*  $(ONI_MOD_LOCAL)\$(ProjectName)\
```
TeleporterBuild 传送器可建造


Space exposure  STATUSITEMS.SPACE  太空暴露  MISC.STATUSITEMS.SPACE

// 动画太麻烦了
//AssetStudio 批量提取文件
得到 TextAsset,Texture2D文件夹
cd TextAsset && rename *.prefab *.bytes 
kanimal-cli batch-convert -o .
 

kanimal-cli.exe scml warp_portal_sender_0.png warp_portal_sender_anim.bytes warp_portal_sender_build.bytes -o warp_portal_sender_scml
kanimal-cli.exe scml warp_portal_receiver_0.png warp_portal_receiver_anim.bytes warp_portal_receiver_build.bytes -o warp_portal_receiver_scml
kanimal-cli.exe scml temporal_tear_opener_0.png temporal_tear_opener_anim.bytes temporal_tear_opener_build.bytes -o temporal_tear_opener_scml

kanimal-cli.exe kanim warp_portal_sender/warp_portal_sender.scml -o .