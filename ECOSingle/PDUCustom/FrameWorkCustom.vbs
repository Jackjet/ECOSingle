Set fso = Wscript.CreateObject("Scripting.FileSystemObject")
set f=fso.opentextfile("FRAME.txt")

test = createobject("Scripting.FileSystemObject").GetFile(Wscript.ScriptFullName).ParentFolder.Path

s=replace(f.readall,"YYYYYYYYYYYYYY",test)'这里是你要替换的文字不知道你那个d后面是分号还是冒号

f.close
set r=fso.opentextfile("FrameWorkCustom.reg",2,true)
r.write s


set ws=CreateObject("wscript.shell") 
ws.Run "FrameWorkCustom.reg"

msgbox"framework4.0安装成功"
Wscript.quit