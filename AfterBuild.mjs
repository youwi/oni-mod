
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
let replaceFileSync=function( filePath, sourceRegx, targetStr){
    var fillText= FS.readFileSync(filePath).toString();
    fillText = fillText.replace(sourceRegx,targetStr);
    FS.writeFileSync(filePath, fillText);
}

function dayVersion() {
    const date = new Date();
    const year = date.getFullYear(); // 年份，例如 2023
    const month = date.getMonth(); // 月份，0-11，0 表示一月，11 表示十二月
    const day = date.getDate(); // 日期，1-31
    return "version: "+year + "." + month + "." + day
}
var args = process.argv.slice(2);
var sourceDir="resource";

if (FS.existsSync("resource/mod_info.yaml")) {
    sourceDir="resource"
} else {
    sourceDir="../../resource"
}

replaceFileSync(sourceDir+"/mod_info.yaml", /version.*/, dayVersion())
console.log(args[1]+"-----Version 版本号已经修改\n: ");


//复制DLL
FS.copyFileSync(args[0],process.env.ONI_MOD_LOCAL+"/"+args[1]+"/"+args[1]+".dll")
console.log(args[1]+"-----DLL 文件已经复制");

//复制resource
FS.cpSync(sourceDir, process.env.ONI_MOD_LOCAL+"/"+args[1],{recursive: true})
console.log(args[1]+"-----resource 文件已经复制");



 
 