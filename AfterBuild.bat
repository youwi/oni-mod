

# 脚本没有生效.
node ../../../auto_version.mjs
mkdir  $(ONI_MOD_LOCAL)\$(ProjectName)
copy /y $(TargetPath)  $(ONI_MOD_LOCAL)\$(ProjectName)\
copy /y $(ProjectDir)resource\*  $(ONI_MOD_LOCAL)\$(ProjectName)\
xcopy /y /s $(ProjectDir)resource\*  $(ONI_MOD_LOCAL)\$(ProjectName)\