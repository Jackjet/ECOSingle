Set fso = Wscript.CreateObject("Scripting.FileSystemObject")
set f=fso.opentextfile("PDU.txt")

test = createobject("Scripting.FileSystemObject").GetFile(Wscript.ScriptFullName).ParentFolder.Path
t=replace(test,"\","|") 
s=replace(f.readall,"TTTTTTTT",t)'这里是你要替换的文字不知道你那个d后面是分号还是冒号
h=replace(s,"XXXXXXXXX",test)
f.close
set r=fso.opentextfile("PDU.reg",2,true)
r.write h


set ws=CreateObject("wscript.shell") 
ws.Run "PDU.reg",vbhide

ws.Run "Service.bat",vbhide

msgbox"电源管理环境安装成功"
Wscript.quit