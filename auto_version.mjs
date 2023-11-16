
//�Զ����°汾�� �Ľű�
import FS from 'fs';

process.argv
let replaceFile = function (filePath, sourceRegx, targetStr) {
    FS.readFile(filePath,function(err,data){
        if (err) {
            console.log(err);
            return;
        }
        let str = data.toString();
        str = str.replace(sourceRegx,targetStr);
        FS.writeFile(filePath, str, function (err) {
            console.log(err);
            return;
        });
    });
}

function dayVersion() {
    const date = new Date();
    const year = date.getFullYear(); // ��ݣ����� 2023
    const month = date.getMonth(); // �·ݣ�0-11��0 ��ʾһ�£�11 ��ʾʮ����
    const day = date.getDate(); // ���ڣ�1-31
    return "version: "+year + "." + month + "." + day
}
replaceFile("../../resource/mod_info.yaml", /version.*/, dayVersion())
 