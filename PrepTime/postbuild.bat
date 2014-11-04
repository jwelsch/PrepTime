@ECHO OFF
SET ConfigurationName=%~1
SET SolutionDir=%~2
SET ProjectDir=%~3
SET TargetDir=%~4
SET TargetPath=%~5
SET TargetName=%~6
IF /I "%ConfigurationName%" == "Release" (
  @ECHO ON
  "%SolutionDir%packages\ilmerge.2.13.0307\ILMerge.exe" /out:"%TargetDir%%TargetName%M.exe" "%TargetPath%" "%TargetDir%Newtonsoft.Json.dll"
  del "%TargetPath%"
  move "%TargetDir%%TargetName%M.exe" "%TargetPath%""
)