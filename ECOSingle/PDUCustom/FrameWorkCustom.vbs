Set fso = Wscript.CreateObject("Scripting.FileSystemObject")
set f=fso.opentextfile("FRAME.txt")

test = createobject("Scripting.FileSystemObject").GetFile(Wscript.ScriptFullName).ParentFolder.Path

s=replace(f.readall,"YYYYYYYYYYYYYY",test)'��������Ҫ�滻�����ֲ�֪�����Ǹ�d�����ǷֺŻ���ð��

f.close
set r=fso.opentextfile("FrameWorkCustom.reg",2,true)
r.write s


set ws=CreateObject("wscript.shell") 
ws.Run "FrameWorkCustom.reg"

msgbox"framework4.0��װ�ɹ�"
Wscript.quit