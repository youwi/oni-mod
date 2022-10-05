# oni-mod


# 脚本

设置环境 :ONI_MOD_LOCAL  D:\Klei\OxygenNotIncluded\mods\local
```
		mkdir  $(ONI_MOD_LOCAL)\$(ProjectName)
		copy /y $(TargetDir)$(TargetFileName)  $(ONI_MOD_LOCAL)\$(ProjectName)\$(TargetFileName)
		copy /y $(ProjectDir)resource\*  $(ONI_MOD_LOCAL)\$(ProjectName)\
```

