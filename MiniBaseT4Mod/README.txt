开始:

如果不合并Plib文件的话,估计也能用.

anim文件只是备份. 不使用.为的是兼容

修改内容:  4周删除边角.

Nuget下载: 
  Plib
  ILmerge
  ILrepack


# 配置脚本A:
(项目:属性-生成事件)
mkdir  $(ONI_MOD_LOCAL)\$(ProjectName)
"$(ILMergeConsolePath)"  /out:$(TargetName)-merged.dll $(TargetName).dll PLib.dll  
copy /y $(TargetDir)$(ProjectName)*-merged*  $(ONI_MOD_LOCAL)\$(ProjectName)\
xcopy /y /s $(ProjectDir)resource\*  $(ONI_MOD_LOCAL)\$(ProjectName)\


# 脚本B
$(SolutionDir)\packages\ilmerge.3.0.41\tools\net452\ILMerge.exe 
/lib:"C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed" 
/out:MiniBase-merged.dll /targetplatform:v4,C:\Windows\Microsoft.NET\Framework64\v4.0.30319 $(TargetPath) $(SolutionDir)\packages\PLib.4.13.0\lib\net471\PLib.dll

copy "$(TargetDir)\MiniBase-merged.dll" "%USERPROFILE%\Documents\Klei\OxygenNotIncluded\mods\local\MiniBase\$(TargetFileName)"
REM copy "$(TargetDir)\MiniBase.dll" "%USERPROFILE%\Documents\Klei\OxygenNotIncluded\mods\local\MiniBase\$(TargetFileName)"
copy "$(TargetDir)\mod.yaml" %USERPROFILE%\Documents\Klei\OxygenNotIncluded\mods\local\MiniBase\mod.yaml
copy "$(TargetDir)\mod_info.yaml" %USERPROFILE%\Documents\Klei\OxygenNotIncluded\mods\local\MiniBase\mod_info.yaml


node ../../../auto_version.mjs
mkdir  $(ONI_MOD_LOCAL)\$(ProjectName)
$(SolutionDir)\packages\ilmerge.3.0.41\tools\net452\ILMerge.exe  /lib:"G:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed"  /out:MiniBase-merged.dll /targetplatform:v4,C:\Windows\Microsoft.NET\Framework64\v4.0.30319 $(TargetPath) $(SolutionDir)\packages\PLib.4.13.0\lib\net471\PLib.dll

copy /y $(TargetDir)$(ProjectName)*-merged.dll  $(ONI_MOD_LOCAL)\$(ProjectName)\
xcopy /y /s $(ProjectDir)resource\*  $(ONI_MOD_LOCAL)\$(ProjectName)\