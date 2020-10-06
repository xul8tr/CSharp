rem for support contact andras.varro@evosoft.com
@call "%VS100COMNTOOLS%\vsvars32.bat"
@echo building %1
@call msbuild %1 /p:Configuration=debug
@pause