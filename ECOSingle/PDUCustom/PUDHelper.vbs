Set fso = Wscript.CreateObject("Scripting.FileSystemObject")
set f=fso.opentextfile("PDU.txt")

test = createobject("Scripting.FileSystemObject").GetFile(Wscript.ScriptFullName).ParentFolder.Path
t=replace(test,"\","|") 
s=replace(f.readall,"TTTTTTTT",t)'��������Ҫ�滻�����ֲ�֪�����Ǹ�d�����ǷֺŻ���ð��
h=replace(s,"XXXXXXXXX",test)
f.close
set r=fso.opentextfile("PDU.reg",2,true)
r.write h


set ws=CreateObject("wscript.shell") 
ws.Run "PDU.reg",vbhide

ws.Run "Service.bat",vbhide

msgbox"��Դ��������װ�ɹ�"
Wscript.quit