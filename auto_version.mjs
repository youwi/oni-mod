
//自动更新版本号 的脚本
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
    const year = date.getFullYear(); // 年份，例如 2023
    const month = date.getMonth(); // 月份，0-11，0 表示一月，11 表示十二月
    const day = date.getDate(); // 日期，1-31
    return "version: "+year + "." + month + "." + day
}
replaceFile("../../resource/mod_info.yaml", /version.*/, dayVersion())
 