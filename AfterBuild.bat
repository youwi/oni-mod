
::脚本改了
::# 脚本没有生效.
::node ../../../auto_version.mjs
::node %%~dpf/auto_version.mjs
echo 参数: %1 %2
mkdir  %ONI_MOD_LOCAL%\%2
copy /y %1 %ONI_MOD_LOCAL%\%2\
copy /y resource\*  %ONI_MOD_LOCAL%\%2\
xcopy /y /s resource\*   %ONI_MOD_LOCAL%\%2\

:: TargetPath
:: mkdir  $(ONI_MOD_LOCAL)\$(ProjectName)
:: copy /y $(TargetPath)  $(ONI_MOD_LOCAL)\$(ProjectName)\
:: copy /y $(ProjectDir)resource\*  $(ONI_MOD_LOCAL)\$(ProjectName)\
:: xcopy /y /s $(ProjectDir)resource\*  $(ONI_MOD_LOCAL)\$(ProjectName)\